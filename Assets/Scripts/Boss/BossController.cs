using UnityEngine;
using System.Collections;
using StatusClass;

[RequireComponent(typeof(EnemyStatusManager))]
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
    static int damageState = Animator.StringToHash("Base Layer.Damage");

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
    /// イベントコントローラー
    /// </summary>
    private GameObject eventController;
    /// <summary>
    /// 敵オブジェクト
    /// </summary>
    private GameObject[] EnemyObject = new GameObject[2];
    /// <summary>
    /// 追加敵を出現させる時のHP
    /// </summary>
    public float ActiveEnemyHP;
    /// <summary>
    /// 剣振り効果音
    /// </summary>
    public AudioClip SwordSe;
    /// <summary>
    /// 魔法効果音
    /// </summary>
    public AudioClip MagicSe;
    /// <summary>
    /// 弓効果音
    /// </summary>
    public AudioClip BowSe;

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
        int i = 0;
        foreach (GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.transform.parent == null && obj.name == "DarkMatter")
            {
                EnemyObject[i] = obj;
                EnemyObject[i].SetActive(false);
                i++;
            }
        }
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

        status = this.gameObject.GetComponent<EnemyStatusManager>().getStatus();
	}
	
	/// <summary>
	/// 更新
	/// </summary>
    void Update()
    {
        VectorToPlayer = TargetObject.transform.position - this.transform.position;
        forward = this.transform.TransformDirection(Vector3.forward).normalized;
        back = this.transform.TransformDirection(Vector3.back).normalized;
        left = this.transform.TransformDirection(Vector3.left).normalized;
        right = this.transform.TransformDirection(Vector3.right).normalized;
        ToPlayerDistance = Vector3.Distance(TargetObject.transform.position, this.transform.position);
        //プレイヤー方向を向く
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(TargetObject.transform.position - transform.transform.position), 0.07f);
        this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);
        //アニメーション切り替え
        AnimationController();

        //HPが指定数以下になったら
        Debug.Log(this.GetComponent<EnemyStatusManager>().getStatus().HP);
        if (this.GetComponent<EnemyStatusManager>().getStatus().HP <= ActiveEnemyHP)
        {
            for (int i = 0; i < EnemyObject.Length; i++)
            {
                EnemyObject[i].SetActive(true);
            }
        }
        //Debug.Log(this.rigidbody.velocity);
    }

    /// <summary>
    /// 固定更新
    /// </summary>
    void FixedUpdate()
    {
        //生きていたら
        if (!this.GetComponent<EnemyStatusManager>().getIsDead())
        {
            //移動フラグ初期化
            isMove = false;
            //移動終了時間を指定
            if (!isEndMoveTime && !isAttack)
            {
                endMoveTime = Random.Range(30f, 300f);
                isEndMoveTime = true;
                AttackRatio = Random.Range(0f, 1f);
                //Debug.Log("Reset Move Time");
            }

            //移動時間をカウント
            MoveTime += Time.deltaTime * 60f;

            //移動終了時間に達したら攻撃に入る
            if (MoveTime >= endMoveTime) 
            {
                //延長線上に障害物がなければ攻撃
                RaycastHit hit;
                Vector3 origin = this.transform.position + new Vector3(0, 4.0f, 0);
                Vector3 toVec = (TargetObject.transform.position + new Vector3(0, 4.0f, 0)) - origin;
                float dis = Vector3.Distance(origin, TargetObject.transform.position);
                if (Physics.Raycast(origin, toVec, out hit, dis, 1 << LayerMask.NameToLayer("Stage")))
                {
                    isAttack = false;
                }
                else
                {
                    isAttack = true;
                }
                //Debug.Log(isAttack);
            }

            //Debug.Log("isAttack = " + isAttack);
            //攻撃中だったら
            if (isAttack)
            {
                MoveTime = 0;
                isEndMoveTime = false;
                //AttackRatio = 0.9f;
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
            for (int i = 0; i < EnemyObject.Length; i++)
            {
                EnemyObject[i].SetActive(false);
            }
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
            float Distance = 20.0f;
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
        //Debug.Log(this.rigidbody.velocity.magnitude);
        //時間になったら、もしくは引っかかるなどして止まっていたら
        if (RandomRand <= RandCount || this.rigidbody.velocity.magnitude < 10f)
        {
            AnctionRand = Random.Range(0, 4);
            RandomRand = Random.Range(180, 300);
            RandCount = 0;
            //Debug.Log("Next Move");
        }

        //行動選択
        switch (AnctionRand)
        {
            case 0: SmallForwardRound(); break;
            case 1: RargeForwardRound(); break;
            case 2: SmallBackRound(); break;
            case 3: RargeBackRound(); break;
        }
        LeaveOrNear();
        //常に下方向に力をかける
        rigidbody.AddForce(Vector3.down * 3.5f, ForceMode.VelocityChange);
        //Debug.Log("MoveNow");
        //Debug.Log(NormalSpeed);
    }

    /// <summary>
    /// 目標に向かって左に回り込む
    /// </summary>
    void SmallForwardRound()
    {
        //rigidbody.velocity = this.transform.TransformDirection(Vector3.forward).normalized * NormalSpeed;
        //rigidbody.velocity = this.transform.TransformDirection(Vector3.left).normalized * NormalSpeed;
        rigidbody.AddForce((forward + left).normalized * NormalSpeed, ForceMode.VelocityChange);
    }

    /// <summary>
    /// 目標に向かって右に回り込む
    /// </summary>
    void RargeForwardRound()
    {
        //rigidbody.velocity = this.transform.TransformDirection(Vector3.forward).normalized * NormalSpeed;
        //rigidbody.velocity = this.transform.TransformDirection(Vector3.right).normalized * (NormalSpeed + 2);
        rigidbody.AddForce((forward + right).normalized * NormalSpeed, ForceMode.VelocityChange);
    }

    /// <summary>
    /// 目標から右に逃げる
    /// </summary>
    void SmallBackRound()
    {
        //rigidbody.velocity = this.transform.TransformDirection(Vector3.back).normalized * NormalSpeed ;
        //rigidbody.velocity = this.transform.TransformDirection(Vector3.right).normalized * NormalSpeed;
        rigidbody.AddForce((back + right).normalized * NormalSpeed, ForceMode.VelocityChange);
    }

    /// <summary>
    /// 目標から左に逃げる
    /// </summary>
    void RargeBackRound()
    {
        //rigidbody.velocity = this.transform.TransformDirection(Vector3.back).normalized * NormalSpeed;
        //rigidbody.velocity = this.transform.TransformDirection(Vector3.left).normalized * (NormalSpeed + 2);
        rigidbody.AddForce((back + left).normalized * NormalSpeed, ForceMode.VelocityChange);
    }

    /// <summary>
    /// 立ち止まる
    /// </summary>
    void Stopping()
    {
        //Debug.Log("StopNow");
        //isAttack = true;
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
        animator.SetBool("isDead", this.gameObject.GetComponent<EnemyStatusManager>().getIsDead());

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
            //audio.PlayOneShot(SwordSe);
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
                GameObject magic = Instantiate(MagicBallObject, ShotPoint.transform.position, Quaternion.LookRotation(TargetObject.transform.position - this.transform.position)) as GameObject;
                magic.GetComponent<MagicController>().setTargetObject(TargetObject);
                MagicController.PlayerDamage = this.status.Magic_Power;
                audio.PlayOneShot(MagicSe);
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
                GameObject arrow = Instantiate(ArrowObject, ShotPoint.transform.position, Quaternion.LookRotation(TargetObject.transform.position - this.transform.position)) as GameObject;
                arrow.GetComponent<BowController>().setTargetObject(TargetObject);
                arrow.GetComponent<BowController>().setIsAutoAim(true);
                BowController.PlayerDamage = this.status.BOW_POW;
                audio.PlayOneShot(BowSe);
            }
        }
        //ダメージモーションの時
        else if (currentBaseState.nameHash == damageState)
        {
            isDamage = false;
        }
    }
}