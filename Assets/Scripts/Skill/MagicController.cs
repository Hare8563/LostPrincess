using UnityEngine;
using System.Collections;

public class MagicController : MonoBehaviour {
    /// <summary>
    /// 1秒間に回転する角度
    /// </summary>
    [SerializeField]
    [Range(0, 360)]
    private float _rotSpeed = 5.0f;
    /// <summary>
    /// 速さ
    /// </summary>
    [SerializeField]
    [Range(0, 100)]
    private float Speed = 0;
    /// <summary>
    /// 目標物
    /// </summary>
    private GameObject Target;
    /// <summary>
    /// 削除までの時間
    /// </summary>
    [SerializeField]
    [Range(0,10)]
    private float DestroyTime = 0;
    /// <summary>
    /// ヒットエフェクト
    /// </summary>
    private GameObject HitEffect;
	/// <summary>
	/// ライトオブジェクト
	/// </summary>
	private Light LightObject;
	/// <summary>
	/// 光の光量
	/// </summary>
	[SerializeField]
	[Range(0,10)]
	private float LightIntensity;
    /// <summary>
    /// ターゲットオブジェクト
    /// </summary>
    private GameObject TargetObject;
    /// <summary>
    /// スリップファイアオブジェクト
    /// </summary>
    private GameObject SlipFireObject;
    /// <summary>
    /// スリップファイアのインスタンス
    /// </summary>
    private GameObject SlipFireInst;

    void Awake()
    {
        HitEffect = Resources.Load("Prefab/HitEffect") as GameObject;
		LightObject = this.transform.FindChild("Point light").GetComponent<Light>();
        SlipFireObject = Resources.Load("Prefab/SlipFire") as GameObject;
    }

	void Start () 
    {
        Destroy(this.gameObject, DestroyTime);
        if (Target == null && TargetObject != null)
        {
            Target = TargetObject;
        }
		LightObject.intensity = LightIntensity;
        //Debug.Log("Emmit");
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        //ターゲットが存在していたら
        if (TargetObject != null)
        {
            // ターゲットとの距離
            Vector3 TargetCenter = Target.transform.position + new Vector3(0, 3.0f, 0);
            float Distance = Vector3.Distance(TargetCenter, this.transform.position);
            Vector3 InitvecTarget = TargetCenter - this.transform.position; // ターゲットへのベクトル
            Vector3 InitvecForward = transform.TransformDirection(Vector3.forward);   // 弾の正面ベクトル
            float InitangleDiff = Vector3.Angle(InitvecForward, InitvecTarget);            // ターゲットまでの角度
            float InitangleAdd = (_rotSpeed * Time.deltaTime);                    // 回転角
            Quaternion InitrotTarget = Quaternion.LookRotation(InitvecTarget);

            // ターゲットまでの角度を取得
            Vector3 vecTarget = TargetCenter - this.transform.position; // ターゲットへのベクトル
            Vector3 vecForward = transform.TransformDirection(Vector3.forward);   // 弾の正面ベクトル
            float angleDiff = Vector3.Angle(vecForward, vecTarget);            // ターゲットまでの角度
            float angleAdd = (_rotSpeed * Time.deltaTime);                    // 回転角
            Quaternion rotTarget = Quaternion.LookRotation(vecTarget);
            // ターゲットが回転角の外なら、指定角度だけターゲットに向ける
            float t = (angleAdd / angleDiff);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotTarget, t);
            // 前進
            transform.position += transform.TransformDirection(Vector3.forward) * Speed;

            //this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(GameObject.FindGameObjectWithTag("Player").transform.position - this.transform.position), 0.01f);
            //this.transform.position += transform.forward * 0.2f;

            ////接触
            //if (Distance < 4.0f)
            //{
            //    if (Target.tag == "Player")
            //    {
            //        Target.GetComponent<PlayerController>().Damage(5);
            //    }
            //    else if (Target.tag == "Boss")
            //    {
            //        Target.GetComponent<BossController>().Damage(5);
            //    }
            //    Destroy(this.gameObject);
            //}
        }
        else
        {
            transform.Translate(Vector3.forward * Speed);
        }

		//ライト光量を明滅
		LightObject.intensity = LightIntensity + Random.Range(-0.5f,0.6f);
        //Debug.Log(EnemyDamage);
	}

    /// <summary>
    /// 目標物
    /// </summary>
    public void setTargetObject(GameObject tagret)
    {
        TargetObject = tagret;
    }

    /// <summary>
    /// 敵に与えるダメージ
    /// </summary>
    public static int EnemyDamage { set; get; }

    /// <summary>
    /// プレイヤーに与えるダメージ
    /// </summary>
    public static int PlayerDamage { set; get; }

    /// <summary>
    /// 何かに当たったら
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Stage")
        {
            Instantiate(HitEffect, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
        //Debug.Log("Target -> " + Target.name);
        //Debug.Log("Magic -> " + collider.name);
        if (Target != null && Target.tag == collider.tag)
        {
            if (Target.tag == "Player")
            {
                SlipFireInst = (GameObject)Instantiate(SlipFireObject, Target.transform.position, SlipFireObject.transform.rotation);
                SlipFireInst.transform.parent = Target.transform;
                Target.GetComponent<PlayerController>().Damage(PlayerDamage);
            }
            else if (Target.tag == "Boss" ||
                Target.tag == "Enemy" ||
                Target.tag == "Hime")
            {
                Target.GetComponent<EnemyStatusManager>().Damage(EnemyDamage);
            }
            Instantiate(HitEffect, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
        else
        {
            if (collider.tag == "Enemy")
            {
                collider.GetComponent<EnemyStatusManager>().Damage(EnemyDamage);
                Instantiate(HitEffect, this.transform.position, this.transform.rotation);
                Destroy(this.gameObject);
            }
        }
    }
}
