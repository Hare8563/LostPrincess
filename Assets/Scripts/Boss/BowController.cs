using UnityEngine;
using System.Collections;

public class BowController : MonoBehaviour {

    [SerializeField]
    [Range(0, 100)]
    private float Speed = 0;
    /// <summary>
    /// 角度を取得したかどうか
    /// </summary>
    private bool isSetRot = false;
    /// <summary>
    /// 目標物
    /// </summary>
    private GameObject Target;
    /// <summary>
    /// 削除までの時間
    /// </summary>
    [SerializeField]
    [Range(0, 10)]
    private float DestroyTime = 0;
    /// <summary>
    /// ヒットエフェクト
    /// </summary>
    GameObject HitEffect;

    void Awake()
    {
        HitEffect = Resources.Load("Prefab/HitEffect") as GameObject;
    }

	// Use this for initialization
	void Start () 
    {
        isSetRot = false;
        Destroy(this.gameObject, DestroyTime);
        if (Target == null && TargetObject != null)
        {
            Target = TargetObject;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        
        
	}

    void FixedUpdate()
    {
        if (!isSetRot)
        {
            //ターゲットが存在していたら
            if (Target != null)
            {
                isSetRot = true;
                Vector3 TargetCenter = Target.transform.position + new Vector3(0, 4.0f, 0);
                this.transform.rotation = Quaternion.LookRotation(TargetCenter - this.transform.position);
                // ターゲットとの距離
                float Distance = Vector3.Distance(TargetCenter, this.transform.position);
                //接触
                if (Distance < 4.0f)
                {
                    if (Target.tag == "Player")
                    {
                        Target.GetComponent<PlayerController>().Damage(5);
                    }
                    else if (Target.tag == "Boss")
                    {
                        Target.GetComponent<BossController>().Damage(5);
                    }
                    Destroy(this.gameObject);
                }
            }
        }
        transform.Translate(Vector3.forward * Speed);
    }

    /// <summary>
    /// 目標物
    /// </summary>
    public static GameObject TargetObject { set; private get; }

    /// <summary>
    /// 何かに当たったら
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("Target -> " + Target.name);
        //Debug.Log("Magic -> " + collider.name);
        if (Target != null && Target.tag == collider.tag)
        {
            if (Target.tag == "Player")
            {
                Target.GetComponent<PlayerController>().Damage(PlayerDamage);
            }
            else if (Target.tag == "Boss")
            {
                Target.GetComponent<BossController>().Damage(EnemyDamage);
                //Debug.Log(EnemyDamage);
            }
            else if (Target.tag == "Enemy")
            {
                Target.GetComponent<EnemyScript>().Damage(EnemyDamage);
            }
            Instantiate(HitEffect, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
        else
        {
            if (collider.tag == "Enemy")
            {
                collider.GetComponent<EnemyScript>().Damage(EnemyDamage);
                Instantiate(HitEffect, this.transform.position, this.transform.rotation);
                Destroy(this.gameObject);
            }
        }
    }

    /// <summary>
    /// 敵に与えるダメージ
    /// </summary>
    public static int EnemyDamage { set; get; }

    /// <summary>
    /// プレイヤーに与えるダメージ
    /// </summary>
    public static int PlayerDamage { set; get; }
}
