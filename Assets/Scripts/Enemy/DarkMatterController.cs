using UnityEngine;
using System.Collections;
using StatusClass;

[RequireComponent(typeof(EnemyStatusManager))]
public class DarkMatterController : MonoBehaviour {

    /// <summary>
    /// 攻撃タイプの列挙体
    /// </summary>
    private enum Type
    {
        Gatling = 0,
        Misile,
    }
    /// <summary>
    /// 攻撃タイプ
    /// </summary>
    [SerializeField]
    private Type type;
    /// <summary>
    /// 速さ
    /// </summary>
    [SerializeField]
    [Range(0,10)]
    private float Speed;
    /// <summary>
    /// プレイヤーに近づける距離
    /// </summary>
    [SerializeField]
    private float MinDistance;
    /// <summary>
    /// 攻撃可能距離
    /// </summary>
    [SerializeField]
    private float AttackDistance;
    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    private GameObject PlayerObject;
    /// <summary>
    /// 弾オブジェクト
    /// </summary>
    private GameObject BulletObject;
    /// <summary>
    /// ミサイルオブジェクト
    /// </summary>
    private GameObject MisileObject;
    /// <summary>
    /// 弾を撃った時間
    /// </summary>
    private float ShotSecond = 0;
    /// <summary>
    /// 弾の速さ
    /// </summary>
    [SerializeField]
    private float BulletSpeed;
    /// <summary>
    /// 弾の散らばり具合
    /// </summary>
    [SerializeField]
    private float SpreadRange;
    /// <summary>
    /// 発射点の散らばり具合
    /// </summary>
    [SerializeField]
    private float ShotPointRange;
    /// <summary>
    /// 弾を撃つ間隔
    /// </summary>
    [SerializeField]
    private int ShotInterval = 1;
    /// <summary>
    /// ミサイル発射効果音
    /// </summary>
    public AudioClip MisileShotSe;
    /// <summary>
    /// ガトリング発射効果音
    /// </summary>
    public AudioClip GatlingShotSe;
    /// <summary>
    /// Statusクラス
    /// </summary>
    public Status status;
    /// <summary>
    /// ステータスマネージャークラス
    /// </summary>
    private EnemyStatusManager enemyStatusManager;
    /// <summary>
    /// HPゲージオブジェクト
    /// </summary>
    private EnemyCanvasHPScript HPGaugeObject;

    void Awake()
    {
        BulletObject = Resources.Load("Prefab/Bullet") as GameObject;
        MisileObject = Resources.Load("Prefab/MisileEmitter") as GameObject;
        enemyStatusManager = this.gameObject.GetComponent<EnemyStatusManager>();
    }

	// Use this for initialization
	void Start () {
        status = enemyStatusManager.getStatus();
        HPGaugeObject = this.GetComponent<EnemyCanvasCreateScript>().Add(this.status.HP, "分かつ輩");
	}
	
	// Update is called once per frame
	void Update () 
    {
        PlayerObject = GetObjecter.GetPlayerObject;
        Vector3 forward = this.transform.TransformDirection(Vector3.forward).normalized;
        Vector3 back = this.transform.TransformDirection(Vector3.back).normalized;
        Vector3 right = this.transform.TransformDirection(Vector3.right).normalized;
        Vector3 left = this.transform.TransformDirection(Vector3.left).normalized;
        Vector3 down = this.transform.TransformDirection(Vector3.down).normalized;

        //プレイヤーとの距離
        float dis = Vector3.Distance(this.transform.position, PlayerObject.transform.position + new Vector3(0, 4, 0));
        //もし離れていたら
        if (dis > MinDistance)
        {
            //Y軸回転のみでプレイヤーの方を向く
            this.transform.LookAt(PlayerObject.transform.position);
            this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);
            //前進
            this.rigidbody.AddForce(forward * Speed, ForceMode.VelocityChange);
        }
        //近づきすぎていたら
        else if (dis <= MinDistance)
        {
            //Y軸回転のみでプレイヤーの方を向く
            this.transform.LookAt(PlayerObject.transform.position);
            this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);
            //後退
            this.rigidbody.AddForce(back * Speed, ForceMode.VelocityChange);
        }

        //攻撃可能距離に来たら
        if (dis < AttackDistance)
        {
            switch (type)
            {
                case Type.Gatling:
                    Gatling();
                    break;
                case Type.Misile:
                    Misile();
                    break;
            }
        }
        HPGaugeObject.setNowHp(this.status.HP);
        if (this.status.HP <= 0)
        {
            Destroy(this.gameObject);
        }
	}

    /// <summary>
    /// ガトリング攻撃
    /// </summary>
    void Gatling()
    {
        ////延長線上に障害物がなければ攻撃
        //RaycastHit hit;
        //Vector3 origin = this.transform.position;
        //Vector3 toVec = origin - (PlayerObject.transform.position + new Vector3(0, 4, 0));
        ////Debug.DrawLine(origin, PlayerObject.transform.position + new Vector3(0, 4, 0));
        //if (!Physics.Raycast(origin, toVec, out hit, dis, 1 << LayerMask.NameToLayer("Stage")))
        //{
        Vector3 RandomVec = new Vector3(Random.Range(-SpreadRange, SpreadRange), Random.Range(-SpreadRange, SpreadRange), Random.Range(-SpreadRange, SpreadRange));
        Vector3 RandomPoint = new Vector3(Random.Range(-ShotPointRange, ShotPointRange), Random.Range(-ShotPointRange, ShotPointRange), Random.Range(-ShotPointRange, ShotPointRange));
        Quaternion toPlayer = Quaternion.LookRotation((PlayerObject.transform.position - this.transform.position) + RandomVec);
        ShotSecond += Method.GameTime();
        if ((int)ShotSecond % ShotInterval == 0)
        {
            audio.PlayOneShot(GatlingShotSe, 0.5f);
            GameObject bullet = Instantiate(BulletObject, this.transform.position + RandomPoint, toPlayer) as GameObject;
            bullet.GetComponent<BulletController>().setBulletSpeed(BulletSpeed);
        }
        //}
    }

    /// <summary>
    /// ミサイル攻撃
    /// </summary>
    void Misile()
    {
        ShotSecond += Method.GameTime();
        if ((int)ShotSecond % ShotInterval == 0)
        {
            GameObject misile = Instantiate(MisileObject, this.transform.position, this.transform.rotation) as GameObject;
            //真上に飛ばす
            misile.transform.localEulerAngles = new Vector3(270,0,0);
            audio.PlayOneShot(MisileShotSe);
        }
    }
}
