using UnityEngine;
using System.Collections;
using StatusClass;

[RequireComponent(typeof(EnemyStatusManager))]
public class EnemyScript : MonoBehaviour
{
    /// <summary>
    /// base layerで使われる、アニメーターの現在の状態の参照
    /// </summary>
    private AnimatorStateInfo currentBaseState;
    /// <summary>
    /// アニメーションコントローラ
    /// </summary>
    private Animator animator;
    int swordState = Animator.StringToHash("Base Layer.swordA");
    int magicState = Animator.StringToHash("Base Layer.Magic");
    /// <summary>
    /// ステータスクラス
    /// </summary>
    Status status;
    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    GameObject player;
    /// <summary>
    /// 二点間距離
    /// </summary>
    float twoPointDistance;
    /// <summary>
    /// ダメージを受けたときの効果音
    /// </summary>
    [SerializeField]
    private AudioClip expGetSe;

    /// <summary>
    /// 魔法効果音
    /// </summary>
    [SerializeField]
    private AudioClip MagicSe;

    ///<summary>
    /// 敵のタイプを設定するenum
    /// </summary>
    private enum EnemyType
    {
        SwordEnemy = 0,
        MagicEnemy = 1,
        BowEnemy = 2
    };

    /// <summary>
    /// 敵のタイプ指定
    /// </summary>
    [SerializeField]
    private EnemyType type;

    /// <summary>
    /// 魔法/弓発射位置
    /// </summary>
    [SerializeField]
    private GameObject ShotPoint;
    /// <summary>
    /// 矢のオブジェクト
    /// </summary>
    private GameObject ArrowObject;

    private GameObject ArrowInstance;
    /// <summary>
    /// ステータスマネージャークラス
    /// </summary>
    private EnemyStatusManager enemyStatusManager;

    /// <summary>
    /// 魔法オブジェクトインスタンス
    /// </summary>
    /// 
    private GameObject MagicBallObject;

    private GameObject magicInstance;
    /// <summary>
    /// 死亡フラグ
    /// </summary>
    private bool deadFlag = false;
    /// <summary>
    /// 初期HP
    /// </summary>
    private int InitHp;
    /// <summary>
    /// 敵対フラグ
    /// </summary>
    private bool HostilityFlag = false;
    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    private GameObject PlayerObject;
    /// <summary>
    /// 剣攻撃コライダを持つオブジェクト
    /// </summary>
    private GameObject SwordColliderObject;
    /// <summary>
    /// ナビメッシュ
    /// </summary>
    private NavMeshAgent agent;
    /// <summary>
    /// 1フレーム前の座標
    /// </summary>
    private Vector3 oldPos;
    /// <summary>
    /// 今フレームの座標
    /// </summary>
    private Vector3 newPos;
    /// <summary>
    /// 移動できるかどうか
    /// </summary>
    private bool canMove = true;
    /// <summary>
    /// 感知範囲を持つオブジェクト
    /// </summary>
    private SensingScript sensingObject;
    /// <summary>
    /// 敵のHP
    /// </summary>
    [SerializeField]
    private int HP;
    /// <summary>
    /// 敵の攻撃力
    /// </summary>
    [SerializeField]
    private int Power;
    /// <summary>
    /// 移動スピード
    /// </summary>
    [SerializeField]
    private float MoveSpeed;
    /// <summary>
    /// 攻撃頻度
    /// </summary>
    [SerializeField]
    private float AttackFreqSecond;
    /// <summary>
    /// 攻撃するまでのカウント
    /// </summary>
    private float FreqCount;
    /// <summary>
    /// 落とすアイテムのインスタンス
    /// </summary>
    private GameObject ItemInst;
    /// <summary>
    /// HPバーのオブジェクト
    /// </summary>
    private GameObject HPBarObject;
    /// <summary>
    /// HPバーのインスタンス
    /// </summary>
    private GameObject HPBarInst;
    /// <summary>
    /// HPバークラス
    /// </summary>
    private EnemyHPBarScript enemyHPBar;

    void Awake()
    {
        if (type == EnemyType.SwordEnemy)
        {
            SwordColliderObject = this.transform.FindChild("SwordCollider").gameObject;
            SwordColliderObject.collider.enabled = false;
        }
        else if (type == EnemyType.BowEnemy)
        {
            ArrowObject = Resources.Load("Prefab/Arrow") as GameObject;
        }
        else if (type == EnemyType.MagicEnemy)
        {
            MagicBallObject = Resources.Load("Prefab/MagicBall") as GameObject;
        }
        enemyStatusManager = this.gameObject.GetComponent<EnemyStatusManager>();
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        sensingObject = this.transform.FindChild("SensingObject").GetComponent<SensingScript>();
        animator = this.gameObject.GetComponent<Animator>();
        ItemInst = Resources.Load("Prefab/Item") as GameObject;
        HPBarObject = Resources.Load("Prefab/EnemyHPBar") as GameObject;

        //インスタンス生成
        HPBarInst = (GameObject)Instantiate(HPBarObject, this.transform.position, HPBarObject.transform.rotation);
        enemyHPBar = HPBarInst.GetComponent<EnemyHPBarScript>();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        player = GameObject.Find(@"HERO_MOTION07");
        oldPos = this.transform.position;
        this.gameObject.AddComponent<NavMeshAgent>();
        agent = this.GetComponent<NavMeshAgent>();
        agent.acceleration = 100;
        agent.speed = MoveSpeed;
        status = enemyStatusManager.getStatus();
        status.Sword_Power = Power;
        status.Magic_Power = Power;
        status.BOW_POW = Power;
        status.HP = HP;
        //Debug.Log(status.HP);
        InitHp = status.HP;
        FreqCount = AttackFreqSecond * 60;
        enemyHPBar.setMaxHp(InitHp);
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        newPos = this.transform.position - oldPos;
        //二点間の距離
        twoPointDistance = Vector3.Distance(this.transform.position, player.transform.position);
        //HPが減っていたら敵対
        if (InitHp > enemyStatusManager.getStatus().HP)
        {
            HostilityFlag = true;
        }
        switch (type)
        {
            case EnemyType.SwordEnemy:
                EnemySwordAction();
                break;
            case EnemyType.MagicEnemy:
                EnemyMagicAction();
                break;
            case EnemyType.BowEnemy:
                EnemyBowAction();
                break;
            default:
                break;
        }
        AnimationCheck();
        enemyHPBar.setNowHPStatus(this.status.HP, this.transform.position);
        oldPos = this.transform.position;
    }

    /// <summary>
    /// 剣によるアクション
    /// </summary>
    private void EnemySwordAction()
    {
        bool swordAttack = false;
        bool running = false;
        float MinDis = 10.0f;

        //Debug.Log(canMove);
        var distance = Vector3.Distance(this.transform.position, player.transform.position);
        //プレイヤーが近づいてきたら自分も近づく
        //Debug.Log(canMove);
        if ((sensingObject.getIsSensing() ||
            HostilityFlag) &&
            distance > MinDis &&
            canMove &&
            !enemyStatusManager.getIsDead())
        {
            this.transform.rotation = Quaternion.LookRotation(newPos);
            agent.SetDestination(PlayerObject.transform.position);
            running = true;
        }
        else
        {
            agent.Stop();
        }
        //目の前に来たら攻撃
        if (twoPointDistance <= MinDis)
        {
            agent.Stop();
            FreqCount += Method.GameTime();
            if (FreqCount >= AttackFreqSecond * 60)
            {
                FreqCount = 0;
                running = false;
                swordAttack = true;
            }
        }
        
        //HPが0になったら経験値を取得
        if (enemyStatusManager.getIsDead())
        {
            //audio.PlayOneShot(expGetSe);
            canMove = false;
            this.collider.enabled = false;
            deadFlag = true;
        }
        GetComponent<Animator>().SetBool(@"IsAttack", swordAttack);
        GetComponent<Animator>().SetBool(@"IsRunning", running);
        GetComponent<Animator>().SetBool(@"deadFlag", deadFlag);
    }
    /// <summary>
    /// 魔法による攻撃アクション
    /// </summary>
    private void EnemyMagicAction()
    {
        bool magicAttack = false;
        bool walking = false;

        var distance = this.transform.position - player.transform.position;

        //Debug.Log (Mathf.Sqrt(val));
        //目の前に来たら攻撃
        if ((sensingObject.getIsSensing() || HostilityFlag) && !enemyStatusManager.getIsDead())
        {
            FreqCount += Method.GameTime();
            if (FreqCount >= AttackFreqSecond * 60)
            {
                FreqCount = 0;
                //延長線上に障害物がなければ攻撃
                RaycastHit hit;
                Vector3 origin = this.transform.position + new Vector3(0, 2.0f, 0);
                Vector3 toVec = (PlayerObject.transform.position + new Vector3(0, 2.0f, 0)) - origin;
                float dis = Vector3.Distance(origin, PlayerObject.transform.position);
                if (!Physics.Raycast(origin, toVec, out hit, dis, 1 << LayerMask.NameToLayer("Stage")))
                {
                    walking = false;
                    magicAttack = true;
                }
            }
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(player.transform.position - this.transform.position), 1.0f);
            this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);
        }
        //HPが0になったら
        if (enemyStatusManager.getIsDead())
        {
            //audio.PlayOneShot(expGetSe);
            this.collider.enabled = false;
            deadFlag = true;
        }

        GetComponent<Animator>().SetBool(@"MagicAttack", magicAttack);
        GetComponent<Animator>().SetBool(@"walking", walking);
        GetComponent<Animator>().SetBool(@"deadFlag", deadFlag);
    }

    /// <summary>
    /// 弓キャラの攻撃アクション
    /// </summary>
    private void EnemyBowAction()
    {
        bool bowAttack = false;
        var distance = this.transform.position - player.transform.position;

        //目の前に来たら攻撃
        if ((sensingObject.getIsSensing() || HostilityFlag) && !enemyStatusManager.getIsDead())
        {
            FreqCount += Method.GameTime();
            if (FreqCount >= AttackFreqSecond * 60)
            {
                FreqCount = 0;
                //延長線上に障害物がなければ攻撃
                RaycastHit hit;
                Vector3 origin = this.transform.position + new Vector3(0, 2.0f, 0);
                Vector3 toVec = (PlayerObject.transform.position + new Vector3(0, 2.0f, 0)) - origin;
                float dis = Vector3.Distance(origin, PlayerObject.transform.position);
                if (!Physics.Raycast(origin, toVec, out hit, dis, 1 << LayerMask.NameToLayer("Stage")))
                {
                    bowAttack = true;
                }
            }
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(player.transform.position - this.transform.position), 1.0f);
            this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);
        }
        //HPが0になったら
        if (enemyStatusManager.getIsDead())
        {
            //audio.PlayOneShot(expGetSe);
            this.collider.enabled = false;
            deadFlag = true;
        }
        GetComponent<Animator>().SetBool(@"attack", bowAttack);
        GetComponent<Animator>().SetBool(@"deadFlag", deadFlag);
    }

    /// <summary>
    /// 剣攻撃開始イベント
    /// </summary>
    public void AttackStartEvent()
    {
        SwordColliderObject.collider.enabled = true;
    }

    /// <summary>
    /// 剣攻撃終了イベント
    /// </summary>
    public void AttackEndEvent()
    {
        SwordColliderObject.collider.enabled = false;
    }

    /// <summary>
    /// 魔法攻撃イベント
    /// </summary>
    public void magicAttackEvent()
    {
        magicInstance = Instantiate(MagicBallObject, ShotPoint.transform.position, Quaternion.LookRotation(player.transform.position - this.transform.position)) as GameObject;
        magicInstance.GetComponent<MagicController>().setTargetObject(player);
        magicInstance.layer = LayerMask.NameToLayer("Attack_Enemy");
        MagicController.PlayerDamage = this.status.Magic_Power;
        audio.PlayOneShot(MagicSe);
    }

    /// <summary>
    /// 弓攻撃イベント
    /// </summary>
    public void bowAttackEvent()
    {
        ArrowInstance = Instantiate(ArrowObject, ShotPoint.transform.position, Quaternion.LookRotation(player.transform.position - this.transform.position)) as GameObject;
        ArrowInstance.GetComponent<BowController>().setTargetObject(player);
        ArrowInstance.GetComponent<BowController>().setIsCharge(false);
        ArrowInstance.layer = LayerMask.NameToLayer("Attack_Enemy");
        BowController.PlayerDamage = this.status.BOW_POW;
    }

    /// <summary>
    /// 死亡イベント
    /// </summary>
    public void DeadEvent()
    {
        //player.GetComponent<PlayerController> ().GetExp (3);
        Instantiate(ItemInst, this.transform.position, ItemInst.transform.rotation);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// アニメーション管理
    /// </summary>
    void AnimationCheck()
    {
        // 参照用のステート変数にBase Layer (0)の現在のステートを設定する
        currentBaseState = this.animator.GetCurrentAnimatorStateInfo(0);
        if (currentBaseState.nameHash == swordState ||
            currentBaseState.nameHash == magicState)
        {
            canMove = false;
        }
        else if (!enemyStatusManager.getIsDead())
        {
            canMove = true;
        }
    }
}