using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyStatusManager))]
public class CatController : MonoBehaviour {

    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    private GameObject PlayerObject;
    /// <summary>
    /// 歩く速さ
    /// </summary>
    [SerializeField]
    [Range(0,10)]
    private float WalkSpeed;
	/// <summary>
	/// 歩く速さ
	/// </summary>
	[SerializeField]
	[Range(0,10)]
	private float RunSpeed;
	/// <summary>
	/// ジャンプ力
	/// </summary>
	[SerializeField]
	[Range(0,100)]
	private float JumpPower;
    /// <summary>
    /// 走る行動を取る距離
    /// </summary>
    [SerializeField]
    private float RunDistance;
    /// <summary>
    /// 攻撃可能距離
    /// </summary>
    [SerializeField]
    private float AttackDistance;
    /// <summary>
    /// 回避行動を取る距離
    /// </summary>
    [SerializeField]
    private float AvoidDistance;
    /// <summary>
    /// 前方
    /// </summary>
    private Vector3 forward;
    /// <summary>
    /// 後方
    /// </summary>
    private Vector3 back;
    /// <summary>
    /// 右方
    /// </summary>
    private Vector3 right;
    /// <summary>
    /// 左方
    /// </summary>
    private Vector3 left;
    /// <summary>
    /// 上方
    /// </summary>
    private Vector3 up;
    /// <summary>
    /// 移動スピード
    /// </summary>
    private float MoveSpeed = 0;
    /// <summary>
    /// アニメーションコントローラ
    /// </summary>
    private Animator animator;
    /// <summary>
    /// 攻撃フラグ
    /// </summary>
    private bool AttackFlag = false;
    /// <summary>
    /// 攻撃可能か
    /// </summary>
    private bool CanAttack = false;
    /// <summary>
    /// 攻撃の矛先が自分に向いているか
    /// </summary>
    private bool AvoidFlag = false;
    /// <summary>
    /// 地面に付いているか
    /// </summary>
    private bool isGround = true;
    /// <summary>
    /// ジャンプ中か
    /// </summary>
    private bool isJump = false;
    /// <summary>
    /// 走るフラグ
    /// </summary>
    private bool RunFlag = false;
    /// <summary>
    /// 経過時間
    /// </summary>
    private float SecondTime = 0;
    /// <summary>
    /// ランダムな時間
    /// </summary>
    private float RandTime = 0;
    /// <summary>
    /// ランダム時間をセットしたか
    /// </summary>
    private bool isSetRand = false;
    /// <summary>
    /// base layerで使われる、アニメーターの現在の状態の参照
    /// </summary>
    private AnimatorStateInfo currentBaseState;
    //各アニメーションへのステート
    static int WalkState = Animator.StringToHash("Base Layer.Walk_Blend");
    static int RunState = Animator.StringToHash("Base Layer.Run");
    static int AttackRunRunState = Animator.StringToHash("Base Layer.AttackRunRun");
    static int AttackState = Animator.StringToHash("Base Layer.Attack");
    static int EndRunState = Animator.StringToHash("Base Layer.EndRun");
    static int JumpState = Animator.StringToHash("Base Layer.Jump");
    static int LandingState = Animator.StringToHash("Base Layer.Landing");

    void Awake()
    {
        PlayerObject = GameObject.FindWithTag("Player");
        animator = this.gameObject.GetComponent<Animator>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        //プレイヤー方向を向く
        if (!CanAttack)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(PlayerObject.transform.position - transform.transform.position), 0.07f);
            this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);
            //頭をプレイヤー方向へ向かせる
            HeadRotation();
        }
        //死亡フラグが立っていたら
		if (this.GetComponent<EnemyStatusManager>().getIsDead())
        {
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
		//移動
        forward = this.transform.TransformDirection(Vector3.forward).normalized;
        back = this.transform.TransformDirection(Vector3.back).normalized;
        right = this.transform.TransformDirection(Vector3.right).normalized;
        left = this.transform.TransformDirection(Vector3.left).normalized;
        up = this.transform.TransformDirection(Vector3.up).normalized;
        Move();
        Attack();
        Avoid();
        AnimationController();
    }

    /// <summary>
    /// 移動
    /// </summary>
    void Move()
    {
        //回避中,攻撃中でなければ移動
        if (!AvoidFlag && !AttackFlag && !CanAttack)
        {
            float dis = Vector3.Distance(this.transform.position, PlayerObject.transform.position);
            //一定距離近づいていたら回避行動
            if (dis < AvoidDistance && isGround)
            {
                AvoidFlag = true;
            }
            //遠かったら走る
            if (dis > RunDistance)
            {
                RunFlag = true;
                MoveSpeed = RunSpeed;
            }
            //近かったら歩く
            else
            {
                RunFlag = false;
                MoveSpeed = WalkSpeed;
            }
            GetComponent<Rigidbody>().AddForce(forward * MoveSpeed, ForceMode.VelocityChange);
            Step();
        }
    }

    /// <summary>
    /// 攻撃行動
    /// </summary>
    void Attack()
    {
        //ランダム時間をセットしていなかったら
        if (!isSetRand)
        {
            RandTime = Random.Range(300, 600);
            AttackFlag = false;
            isSetRand = true;
        }
        //時間経過
        SecondTime += Method.GameTime();
        //時間になったら攻撃開始
        if (SecondTime >= RandTime)
        {
            isSetRand = false;
            AttackFlag = true;
        }
        //攻撃フラグが立っていたら
        if (AttackFlag)
        {
            float dis = Vector3.Distance(this.transform.position, PlayerObject.transform.position);
            //敵に向かって突っ込む
            GetComponent<Rigidbody>().AddForce(forward * RunSpeed, ForceMode.VelocityChange);
            //攻撃可能距離まで来たら
            if (dis <= AttackDistance)
            {
                CanAttack = true;
                
            }
            //攻撃中だったら
            if (CanAttack)
            {   
                float height = 8.0f;
                //下向きに力を加える
                GetComponent<Rigidbody>().AddForce(Vector3.down * 10, ForceMode.VelocityChange);
                //地面との距離を測る
                RaycastHit hit;
                if (Physics.Raycast(this.transform.position, Vector3.down, out hit, Mathf.Infinity))
                {
                    //Debug.Log(hit.distance);
                    //落下中で地面との接触距離になったら
                    if (hit.distance <= height && this.GetComponent<Rigidbody>().velocity.y < 0)
                    {
                        Debug.Log("end");
                        CanAttack = false;
                        AttackFlag = false;
                        isGround = true;
                        SecondTime = 0;
                    }
                    else
                    {
                        isGround = false;
                    }
                }
                //何も当たらなかったら(多分埋まっていたら)
                else
                {
                    GetComponent<Rigidbody>().AddForce(up * JumpPower, ForceMode.VelocityChange);
                }
            }
        }
    }

    /// <summary>
    /// 回避行動
    /// </summary>
    void Avoid()
    {
        //Debug.Log(this.rigidbody.velocity.y);
        float height = 8.0f;
        //回避フラグが立っていたら
        if (AvoidFlag)
        {
            Debug.Log("avoid");
            //下向きに力を加える
            GetComponent<Rigidbody>().AddForce(Vector3.down * 10, ForceMode.VelocityChange);
            //Debug.Log("avoid");
            //地面との距離を測る
            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, Vector3.down, out hit, Mathf.Infinity))
            {
                //Debug.Log(hit.distance);
                //落下中で地面との接触距離になったら
                if (hit.distance <= height && this.GetComponent<Rigidbody>().velocity.y < 0)
                {
                    AvoidFlag = false;
                    isGround = true;
                    isJump = false;
                }
                else
                {
                    isGround = false;
                }
            }
            //何も当たらなかったら(多分埋まっていたら)
            else
            {
                Debug.Log("none");
                GetComponent<Rigidbody>().AddForce(up * JumpPower, ForceMode.VelocityChange);
            }
        }
    }

    /// <summary>
    /// 壁引っかかり対処のステップ行動
    /// </summary>
    void Step()
    {
        float mag = this.GetComponent<Rigidbody>().velocity.magnitude;
        if (mag < 7)
        {
            AvoidFlag = true;
            Avoid();
        }
    }

    /// <summary>
    /// プレイヤーの位置に合わせて頭を回転
    /// </summary>
    void HeadRotation()
    {
        Vector3 toPlayerVec = PlayerObject.transform.position - this.transform.position;
        float angle = Vector3.Angle(forward, toPlayerVec) - 90;
        //自身の右方向ベクトルとプレイヤーとの距離
        float thisRightDis = Vector3.Distance(this.transform.position + right, PlayerObject.transform.position);
        //自身の左方向ベクトルとプレイヤーとの距離
        float thisLeftDis = Vector3.Distance(this.transform.position + left, PlayerObject.transform.position);
        //プレイヤーオブジェクトが左側にいたら
        if (thisLeftDis < thisRightDis)
        {
            angle = 180 - angle;
        }
        float cos = Mathf.Sin((angle + 90) * Mathf.Deg2Rad);
        animator.SetFloat("Horizontal", cos);
    }

    /// <summary>
    /// アニメーション管理
    /// </summary>
    void AnimationController()
    {
        // 参照用のステート変数にBase Layer (0)の現在のステートを設定する
        currentBaseState = this.animator.GetCurrentAnimatorStateInfo(0);
        animator.SetBool("AvoidFlag", AvoidFlag);
        animator.SetBool("isGround", isGround);
        animator.SetBool("RunFlag", RunFlag);
        animator.SetBool("AttackFlag", AttackFlag);
        animator.SetBool("CanAttack", CanAttack);
    }

    /// <summary>
    /// ジャンプ攻撃イベント
    /// </summary>
    void JumpAttackEvent()
    {
        Vector3 forwardVec = (forward * 5) + up/2;
        GetComponent<Rigidbody>().AddForce(forwardVec * JumpPower, ForceMode.VelocityChange);
    }

    /// <summary>
    /// 回避ジャンプイベント
    /// </summary>
    void AvoidanceJumpEvent()
    {
        isJump = true;
        //三方向ランダムに避ける
        Vector3 backVec = (back * 3) + up;
        Vector3 rightVec = (right * 3) + up;
        Vector3 leftVec = (left * 3) + up;
        int rand = Random.Range(0,4);
        switch (rand)
        {
            case 0:
                GetComponent<Rigidbody>().AddForce(backVec * JumpPower, ForceMode.VelocityChange);
                break;
            case 1:
                GetComponent<Rigidbody>().AddForce(rightVec * JumpPower, ForceMode.VelocityChange);
                break;
            case 2:
                GetComponent<Rigidbody>().AddForce(leftVec * JumpPower, ForceMode.VelocityChange);
                break;
        }
    }
}
