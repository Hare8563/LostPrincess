using UnityEngine;
using System.Collections;
using StatusClass;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// ボス戦かどうか
    /// </summary>
    [SerializeField]
    private bool isBossBattle = false;
    /// <summary>
    /// ターゲットオブジェクト
    /// </summary>
    private GameObject TargetObject;
    /// <summary>
    /// 道中スピード
    /// </summary>
    [SerializeField]
    [Range(0, 100)]
    private float RoadSpeed = 0;
    /// <summary>
    /// 通常移動スピード
    /// </summary>
    [SerializeField]
    [Range(0, 100)]
    private float NormalSpeed = 0;
    /// <summary>
    /// 攻撃移動スピード
    /// </summary>
    [SerializeField]
    [Range(0, 100)]
    private float AttackSpeed = 0;
    /// <summary>
    /// 攻撃中かどうか
    /// </summary>
    private bool isAttack = false;
    /// <summary>
    /// 魔法ボール
    /// </summary>
    private GameObject MagicBallObject;
    /// <summary>
    /// 矢
    /// </summary>
    private GameObject ArrowObject;
    /// <summary>
    /// 魔法/弓発射位置
    /// </summary>
    [SerializeField]
    private GameObject ShotPoint;
    /// <summary>
    /// アニメーションコントローラ
    /// </summary>
    private Animator animator;
    /// <summary>
    /// 魔法を撃っているかどうか
    /// </summary>
    private bool isShotMagic = false;
    /// <summary>
    /// 魔法を一発打ったか(短時間に連続して出さないようにする)
    /// </summary>
    private bool isOneShotMagic = false;
    /// <summary>
    /// 矢を撃っているかどうか
    /// </summary>
    private bool isShotArrow = false;
    /// <summary>
    /// 矢を一発打ったか(短時間に連続して出さないようにする)
    /// </summary>
    private bool isOneShotArrow = false;
    /// <summary>
    /// 剣を振っているかどうか
    /// </summary>
    private bool isAttackSword = false;
    /// <summary>
    /// 移動しているかどうか
    /// </summary>
    private bool isMove = false;
    /// <summary>
    /// base layerで使われる、アニメーターの現在の状態の参照
    /// </summary>
    private AnimatorStateInfo currentBaseState;
    //各アニメーションへのステート
    static int idleState = Animator.StringToHash("Base Layer.Idle");
    static int runState = Animator.StringToHash("Base Layer.Run");
    static int swordRunState = Animator.StringToHash("Base Layer.Sword_Run");
    static int swordState = Animator.StringToHash("Base Layer.Sword");
    static int magic_01State = Animator.StringToHash("Base Layer.Magic_01");
    static int magic_02State = Animator.StringToHash("Base Layer.Magic_02");
    static int arrow_01State = Animator.StringToHash("Base Layer.Arrow_01");
    static int arrow_02State = Animator.StringToHash("Base Layer.Arrow_02");
    /// <summary>
    /// Statusクラス
    /// </summary>
    public Status status;
    /// <summary>
    /// 死亡フラグ
    /// </summary>
    private bool deadFlag = false;
    private GameObject prefab;
    void Awake()
    {
        TargetObject = GameObject.FindGameObjectWithTag("Boss");
        MagicBallObject = Resources.Load("Prefab/MagicBall") as GameObject;
        ArrowObject = Resources.Load("Prefab/Arrow") as GameObject;
        animator = this.gameObject.GetComponent<Animator>();
    }

    // Use this for initialization
    void Start()
    {
        status = new Status(1, 0, 10, 10);
    }

    void Update()
    {
        //アニメーション管理
        AnimationController();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //移動
        if (!isAttack)
        {
            Move();
        }
        //攻撃
        Attack();
    }

    /// <summary>
    /// 移動
    /// </summary>
    void Move()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");
        isMove = false;
        //ボス戦だったら
        if (isBossBattle)
        {
            if (inputH != 0.0f || inputV != 0.0f)
            {
                isMove = true;
            }
            //敵方向を向く
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(TargetObject.transform.position - this.transform.transform.position), 0.07f);
            this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);

            //前
            if (Input.GetKey(KeyCode.W))
            {
                isMove = true;
                rigidbody.AddForce(transform.TransformDirection(Vector3.forward).normalized * NormalSpeed, ForceMode.VelocityChange);
            }
            //後ろ
            else if (Input.GetKey(KeyCode.S))
            {
                isMove = true;
                rigidbody.AddForce(transform.TransformDirection(Vector3.back).normalized * NormalSpeed, ForceMode.VelocityChange);
            }
            //左
            if (Input.GetKey(KeyCode.A))
            {
                isMove = true;
                rigidbody.AddForce(transform.TransformDirection(Vector3.left).normalized * NormalSpeed, ForceMode.VelocityChange);
            }
            //右
            else if (Input.GetKey(KeyCode.D))
            {
                isMove = true;
                rigidbody.AddForce(transform.TransformDirection(Vector3.right).normalized * NormalSpeed, ForceMode.VelocityChange);
            }
        }
        //道中だったら
        else
        {
            if (inputH != 0.0f || inputV != 0.0f)
            {
                isMove = true;
            }
            //進行方向取得
            Vector3 inputVec = new Vector3(inputH, 0.0f, inputV).normalized;
            //キャラ回転
            if (!inputVec.Equals(Vector3.zero))
            {
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(inputVec), 0.1f);
                rigidbody.AddForce(inputVec * RoadSpeed * 10.0f, ForceMode.VelocityChange);
                //this.transform.Translate(Vector3.forward * RoadSpeed);
            }
        }
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    void Attack()
    {
        //剣
        if (Input.GetKeyDown(KeyCode.J))
        {
            isAttack = true;
            isAttackSword = true;
        }
        //魔法
        else if (Input.GetKeyDown(KeyCode.I))
        {
            isAttack = true;
            isShotMagic = true;
            MagicController.TargetObject = TargetObject;
        }
        //弓
        else if (Input.GetKeyDown(KeyCode.O))
        {
            isAttack = true;
            isShotArrow = true;
            BowController.TargetObject = TargetObject;
        }
    }

    /// <summary>
    /// アニメーション管理
    /// </summary>
    void AnimationController()
    {
        // 参照用のステート変数にBase Layer (0)の現在のステートを設定する
        currentBaseState = this.animator.GetCurrentAnimatorStateInfo(0);
        animator.SetBool("isMove", isMove);
        animator.SetBool("isAttack", isAttack);
        //animator.SetBool("isAttackSwordRun", isAttackSwordRun);
        animator.SetBool("isAttackSword", isAttackSword);
        animator.SetBool("isShotMagic", isShotMagic);
        animator.SetBool("isShotArrow", isShotArrow);
        animator.SetBool("DeadFlag", deadFlag);

        //Debug.Log(animator.GetBool("isAttackSword"));

        //立ちモーションの時
        if (currentBaseState.nameHash == idleState)
        {
            isAttack = false;
            isAttackSword = false;
            isShotMagic = false;
            isShotArrow = false;
        }
        //走りモーションの時
        else if (currentBaseState.nameHash == runState)
        {

        }
        //剣モーションの時
        else if (currentBaseState.nameHash == swordState)
        {

        }
        //魔法モーション(_01)の時
        else if (currentBaseState.nameHash == magic_01State)
        {
            isOneShotMagic = false;
        }
        //魔法モーション(_02)の時
        else if (currentBaseState.nameHash == magic_02State)
        {
            if (!isOneShotMagic)
            {
                isOneShotMagic = true;
                if (isBossBattle) Instantiate(MagicBallObject, ShotPoint.transform.position, Quaternion.LookRotation(TargetObject.transform.position - this.transform.position));
                else Instantiate(MagicBallObject, ShotPoint.transform.position, this.transform.rotation);
                MagicController.EnemyDamage = this.status.Magic_Power;
            }
        }
        //弓モーション(_01)の時
        else if (currentBaseState.nameHash == arrow_01State)
        {
            isOneShotArrow = false;
        }
        //弓モーション(_02)の時
        else if (currentBaseState.nameHash == arrow_02State)
        {
            if (!isOneShotArrow)
            {
                isOneShotArrow = true;
                if (isBossBattle) Instantiate(ArrowObject, ShotPoint.transform.position, Quaternion.LookRotation(TargetObject.transform.position - this.transform.position));
                else Instantiate(ArrowObject, ShotPoint.transform.position, this.transform.rotation);
                BowController.EnemyDamage = this.status.BOW_POW;
            }
        }
    }

    /// <summary>
    /// 何かに当たったら
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.collider.name);
        if (collision.collider.name == "toBossEventWall")
        {
            Application.LoadLevel("Boss");
        }
    }
    bool flag = false;
    float attackTime = 0.0f;

    /// <summary>
    /// 何かに当たり続けたら
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerStay(Collider collider)
    {
        currentBaseState = this.animator.GetCurrentAnimatorStateInfo(0);
        var currentTime = Time.time;
        if (collider.gameObject.CompareTag("Enemy") && Input.GetKeyDown(KeyCode.J))
        {

            attackTime = currentTime;
            flag = true;
        }
        if (currentTime - attackTime > 0.3f && flag == true)
        {
            Debug.Log("Called: " + collider.gameObject.name);
            var enemy = collider.gameObject.GetComponent<EnemyScript>();
            enemy.Damage(this.status.Sword_Power);
            flag = false;
            attackTime = 0.0f;
        }
    }

    /// <summary>
    /// 外部から経験値取得を呼び出す関数
    /// </summary>
    /// <param name="exp">経験値</param>
    public void GetExp(int exp)
    {
        this.status.EXP += exp;
        if (this.status.EXP >= this.status.ExpLimit)
            status.LevUp();
    }

    /// <summary>
    /// 外部からダメージを呼び出す関数
    /// </summary>
    /// <param name="val">ダメージ値</param>
    public void Damage(int val)
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        this.status.HP -= val;
        if (this.status.HP <= 0)
            this.deadFlag = true;
    }

    /// <summary>
    /// GUIを表示
    /// </summary>
    void OnGUI()
    {
        GUIStyle guistyle = new GUIStyle();
        guistyle.fontSize = 32;
        guistyle.normal.textColor = Color.red;
        GUI.Label(new Rect(0, 250, 200, 50), @"LEV: " + this.status.LEV.ToString(), guistyle);
        GUI.Label(new Rect(0, 300, 200, 50), @"HP: " + this.status.HP.ToString(), guistyle);
        GUI.Label(new Rect(0, 350, 200, 50), @"EXP: " + this.status.EXP.ToString(), guistyle);
    }
}