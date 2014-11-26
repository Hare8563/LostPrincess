using UnityEngine;
using System.Collections;
using StatusClass;

public class BossController : MonoBehaviour {
    /// <summary>
    /// 敵として振る舞うか
    /// </summary>
    [SerializeField]
    private bool isEnemy = true;
    /// <summary>
    /// ターゲットオブジェクト
    /// </summary>
    private GameObject TargetObject;
    /// <summary>
    /// 攻撃パターン乱数保存の構造体
    /// </summary>
    private struct AttackPattern
    {
        public float Sword;
        public float Magic;
        public float Bow;
    };
    /// <summary>
    /// 攻撃パターン
    /// </summary>
    AttackPattern attackPattern = new AttackPattern();
    /// <summary>
    /// 移動している時間
    /// </summary>
    private float MoveTime = 0;
    /// <summary>
    /// 移動終了時間
    /// </summary>
    private float endMoveTime = 0;
    /// <summary>
    /// 移動中かどうか
    /// </summary>
    private bool isMove = false;
    /// <summary>
    /// 移動終了時間を指定したか
    /// </summary>
    private bool isEndMoveTime = false;
    /// <summary>
    /// 攻撃しているか
    /// </summary>
    private bool isAttack = false;
    /// <summary>
    /// 攻撃手段の割合
    /// </summary>
    private float AttackRatio = 0;
    /// <summary>
    /// ボス位置からプレイヤーへ向かうベクトル
    /// </summary>
    private Vector3 VectorToPlayer;
    /// <summary>
    /// 剣攻撃のため走っているか
    /// </summary>
    private bool isAttackSwordRun = false;
    /// <summary>
    /// 剣攻撃をしたか
    /// </summary>
    private bool isAttackSword = false;
    /// <summary>
    /// 魔法ボール
    /// </summary>
    private GameObject MagicBallObject;
    /// <summary>
    /// 魔法を打ったか
    /// </summary>
    private bool isShotMagic = false;
    /// <summary>
    /// 魔法を一発打ったか(短時間に連続して出さないようにする)
    /// </summary>
    private bool isOneShotMagic = false;
    /// <summary>
    /// 矢
    /// </summary>
    private GameObject ArrowObject;
    /// <summary>
    /// 弓を打ったか
    /// </summary>
    private bool isShotArrow = false;
    /// <summary>
    /// 弓を一発打ったか(短時間に連続して出さないようにする)
    /// </summary>
    private bool isOneShotArrow = false;
    /// <summary>
    /// プレイヤーとの距離
    /// </summary>
    private float ToPlayerDistance = 0;
    /// <summary>
    /// ランダムなカウンタ
    /// </summary>
    private float RandCount;
    /// <summary>
    /// 次の行動に移るまでのカウンタの目標ランダム値
    /// </summary>
    private int RandomRand;
    /// <summary>
    /// 行動選択のランダム値
    /// </summary>
    private int AnctionRand;
    /// <summary>
    /// 前進
    /// </summary>
    private Vector3 forward;
    /// <summary>
    /// 後退
    /// </summary>
    private Vector3 back;
    /// <summary>
    /// 左
    /// </summary>
    private Vector3 left;
    /// <summary>
    /// 右
    /// </summary>
    private Vector3 right;
    /// <summary>
    /// プレイヤーとの距離の最小値
    /// </summary>
    private float ToPlayerDistance_Min = 25;
    /// <summary>
    /// プレイヤーとの距離の最大値
    /// </summary>
    private float ToPlayerDistance_Max = 70;
    /// <summary>
    /// アニメーションコントローラ
    /// </summary>
    private Animator animator;
    /// <summary>
    /// 魔法/弓発射位置
    /// </summary>
    [SerializeField]
    private GameObject ShotPoint;
    /// <summary>
    /// 通常移動スピード
    /// </summary>
    [SerializeField]
    [Range(0,100)]
    private float NormalSpeed = 0;
    /// <summary>
    /// 攻撃移動スピード
    /// </summary>
    [SerializeField]
    [Range(0, 100)]
    private float AttackSpeed = 0;
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
    /// <summary>
    /// ダメージを負ったか
    /// </summary>
    private bool isDamage = false;
    /// <summary>
    /// 死んだか
    /// </summary>
    private bool isDead = false;
    /// <summary>
    /// イベントコントローラー
    /// </summary>
    private GameObject eventController;

    /// <summary>
    /// 読み込み
    /// </summary>
    void Awake()
    {
        if (isEnemy)
        {
            TargetObject = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            TargetObject = GameObject.FindGameObjectWithTag("Boss");
        }
        MagicBallObject = Resources.Load("Prefab/MagicBall") as GameObject;
        ArrowObject = Resources.Load("Prefab/Arrow") as GameObject;
        animator = this.gameObject.GetComponent<Animator>();
        eventController = GameObject.Find("EventManager");
    }

	/// <summary>
	/// 初期化
	/// </summary>
	void Start () 
    {
        attackPattern.Sword = 0.3f;
        attackPattern.Magic = 0.4f;
        attackPattern.Bow = 0.3f;

        RandCount = 0;
        AnctionRand = Random.Range(0, 3);
        RandomRand = Random.Range(180, 300);

        status = new Status(1, 0, 10, 10);
	}
	
	/// <summary>
	/// 更新
	/// </summary>
	void Update () 
    {
        VectorToPlayer = TargetObject.transform.position - this.transform.position;
        forward = Vector3.forward * Method.GameTime();
        back = Vector3.back * Method.GameTime();
        left = Vector3.left * Method.GameTime();
        right = Vector3.right * Method.GameTime();
        ToPlayerDistance = Vector3.Distance(TargetObject.transform.position, this.transform.position);
        //プレイヤー方向を向く
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(TargetObject.transform.position - transform.transform.position), 0.07f);
        this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);
        //アニメーション切り替え
        AnimationController();

        //Debug.Log(ToPlayerDistance);
	}

    /// <summary>
    /// 固定更新
    /// </summary>
    void FixedUpdate()
    {
        //生きていたら
        if (!this.deadFlag)
        {
            //移動フラグ初期化
            isMove = false;
            //移動終了時間を指定
            if (!isEndMoveTime && !isAttack)
            {
                endMoveTime = Random.Range(240f, 420f);
                isEndMoveTime = true;
                AttackRatio = Random.Range(0f, 1f);
                Debug.Log("Reset Move Time");
            }

            //移動時間をカウント
            MoveTime += Time.deltaTime * 60f;

            //移動終了時間に達したら攻撃に入る
            if (MoveTime >= endMoveTime) 
            {
                isAttack = true;
            }

            //Debug.Log("isAttack = " + isAttack);
            //攻撃中だったら
            if (isAttack)
            {
                MoveTime = 0;
                isEndMoveTime = false;
                CheckAttack(AttackRatio);
            }
            else
            {
                Move();
            }
        }
        //死んだら
        else
        {
            isDead = true;
            eventController.GetComponent<EventController>().WhiteOut("Ending", 0.5f);
            //Application.LoadLevel("Title");
        }
    }

    /// <summary>
    /// 攻撃手段の割合による攻撃手段の決定
    /// </summary>
    /// <param name="ratio">乱数で決定されたボスの攻撃手段</param>
    void CheckAttack(float ratio)
    {
        //剣
        if (0f <= ratio && ratio < attackPattern.Sword)
        {
            SwordAttack();
        }
        //魔法
        else if (attackPattern.Sword <= ratio && ratio < attackPattern.Sword + attackPattern.Magic)
        {
            MagicAttack();
        }
        //弓
        else if (attackPattern.Sword + attackPattern.Magic <= ratio && ratio < 1f)
        {
            BowAttack();
        }
        //Debug.Log("AttackNow");
    }

    /// <summary>
    /// 剣攻撃の挙動
    /// </summary>
    void SwordAttack()
    {
        //Debug.Log("SwordAttack");
        isAttackSwordRun = true;
        //プレイヤーに近づくフラグがたったら
        if (isAttackSwordRun)
        {
            float Distance = 15.0f;
            //一定距離近づいたら剣振りフラグを立てる
            if (ToPlayerDistance <= Distance)
            {
                isAttackSword = true;
                isAttackSwordRun = false;
            }
            //プレイヤーに向かって進む
            else
            {
                this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(TargetObject.transform.position - this.transform.position), 0.1f);
                this.transform.position += transform.forward * AttackSpeed;
            }
        }
        //Debug.Log("SwordNow");
    }

    /// <summary>
    /// 魔法攻撃の挙動
    /// </summary>
    void MagicAttack()
    {
        //Debug.Log("MagicAttack");
        if (!isShotMagic)
        {
            isShotMagic = true;
            //MagicBallObject.GetComponent<MagicController>().TargetObject = TargetObject;
            MagicController.TargetObject = TargetObject;
        }
    }

    /// <summary>
    /// 弓攻撃の挙動
    /// </summary>
    void BowAttack()
    {
        //Debug.Log("BowAttack");
        if (!isShotArrow)
        {
            isShotArrow = true;
            //ArrowObject.GetComponent<BowController>().TargetObject = TargetObject;
            BowController.TargetObject = TargetObject;
        }
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    void Move()
    {
        isMove = true;
        //一定時間ごとに行動を切り替える
        RandCount += Method.GameTime();
        if (RandomRand <= RandCount)
        {
            AnctionRand = Random.Range(0, 5);
            RandomRand = Random.Range(180, 300);
            RandCount = 0;
            Debug.Log("Next Move");
        }

        //行動選択
        switch (AnctionRand)
        {
            case 0: SmallForwardRound(); break;
            case 1: RargeForwardRound(); break;
            case 2: SmallBackRound(); break;
            case 3: RargeBackRound(); break;
            default: Stopping(); break;
        }
        LeaveOrNear();
        //Debug.Log("MoveNow");
    }

    /// <summary>
    /// 目標に向かって小さく左に回り込む
    /// </summary>
    void SmallForwardRound()
    {
        rigidbody.velocity = this.transform.TransformDirection(Vector3.forward).normalized * NormalSpeed;
        rigidbody.velocity = this.transform.TransformDirection(Vector3.left).normalized * NormalSpeed;
    }

    /// <summary>
    /// 目標に向かって大きく右に回り込む
    /// </summary>
    void RargeForwardRound()
    {
        rigidbody.velocity = this.transform.TransformDirection(Vector3.forward).normalized * NormalSpeed;
        rigidbody.velocity = this.transform.TransformDirection(Vector3.right).normalized * (NormalSpeed + 2);

    }

    /// <summary>
    /// 目標から小さく回り込みながら右に逃げる
    /// </summary>
    void SmallBackRound()
    {
        rigidbody.velocity = this.transform.TransformDirection(Vector3.back).normalized * NormalSpeed ;
        rigidbody.velocity = this.transform.TransformDirection(Vector3.right).normalized * NormalSpeed;

    }

    /// <summary>
    /// 目標から大きく回り込みながら左に逃げる
    /// </summary>
    void RargeBackRound()
    {
        rigidbody.velocity = this.transform.TransformDirection(Vector3.back).normalized * NormalSpeed;
        rigidbody.velocity = this.transform.TransformDirection(Vector3.left).normalized * (NormalSpeed + 2);
    }

    /// <summary>
    /// 立ち止まる
    /// </summary>
    void Stopping()
    {
        //Debug.Log("StopNow");
        isAttack = true;
        ////立ち止まるがプレイヤーが近づくと逃げる
        //if (ToPlayerDistance < ToPlayerDistance_Min)
        //{
        //    AnctionRand = Random.Range(2, 4);
        //}
    }

    /// <summary>
    /// 下がる
    /// </summary>
    void Avoid()
    {
        this.transform.Translate(back * NormalSpeed);
    }

    /// <summary>
    /// 目標と一定距離を保つ
    /// </summary>
    void LeaveOrNear()
    {
        ////近づき過ぎたら離れる
        //if (ToPlayerDistance_Min > ToPlayerDistance)
        //{
        //    AnctionRand = Random.Range(2, 3);
        //}
        ////遠過ぎたら近づく
        //else if (ToPlayerDistance > ToPlayerDistance_Max)
        //{
        //    AnctionRand = Random.Range(0, 1);
        //}
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
        animator.SetBool("isAttackSwordRun", isAttackSwordRun);
        animator.SetBool("isAttackSword", isAttackSword);
        animator.SetBool("isShotMagic", isShotMagic);
        animator.SetBool("isShotArrow", isShotArrow);
        animator.SetBool("isDamage", isDamage);
        animator.SetBool("isDead", isDead);

        //立ちモーションの時
        if (currentBaseState.nameHash == idleState)
        {
            //Debug.Log("Idle");
            isAttack = false;
            isDamage = false;
        }
        //走りモーションの時
        else if (currentBaseState.nameHash == runState)
        {
            
        }
        //剣モーションの時
        else if (currentBaseState.nameHash == swordState)
        {
            isAttackSword = false;
            isAttackSwordRun = false;
            isAttack = false;
        }
        //魔法モーション(_01)の時
        else if (currentBaseState.nameHash == magic_01State)
        {
            isOneShotMagic = false;
            isShotMagic = false;
            isAttack = false;
        }
        //魔法モーション(_02)の時
        else if (currentBaseState.nameHash == magic_02State)
        {
            if (!isOneShotMagic)//!isShotMagic)
            {
                isOneShotMagic = true;
                Instantiate(MagicBallObject, ShotPoint.transform.position, Quaternion.LookRotation(TargetObject.transform.position - this.transform.position));
            }
        }
        //弓モーション(_01)の時
        else if (currentBaseState.nameHash == arrow_01State)
        {
            isOneShotArrow = false;
            isShotArrow = false;
            isAttack = false;
        }
        //弓モーション(_02)の時
        else if (currentBaseState.nameHash == arrow_02State)
        {
            if (!isOneShotArrow)
            {
                isOneShotArrow = true;
                Instantiate(ArrowObject, ShotPoint.transform.position, Quaternion.LookRotation(TargetObject.transform.position - this.transform.position));
            }
        }
    }

    /// <summary>
    /// 外部からダメージを呼び出す関数
    /// </summary>
    /// <param name="val">ダメージ値</param>
    public void Damage(int val)
    {
        //AudioSource audio = GetComponent<AudioSource> ();
        //audio.Play ();
        isDamage = true;
        this.status.HP -= val;
        if (this.status.HP <= 0)
            this.deadFlag = true;
    }

    /// <summary>
    /// オブジェクトから離れた時に呼び出される
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerExit(Collider collider)
    {
        //Debug.Log(collider.name);
        if (collider.name == "LimitArea")
        {
            AnctionRand = Random.Range(0, 2);
        }
    }
}