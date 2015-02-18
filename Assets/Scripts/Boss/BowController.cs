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
    /// <summary>
    /// ターゲットオブジェクト
    /// </summary>
    private GameObject TargetObject;
    /// <summary>
    /// 自動エイムするかどうか
    /// </summary>
    private bool isAim = false;
    /// <summary>
    /// 矢を停止させるフラグ
    /// </summary>
    private bool StopFlag = false;

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
        //まだ打ってなかったら
        if (!isSetRot)
        {
            //ターゲットが存在していたら
            if (Target != null)
            {
                isSetRot = true;
                Vector3 TargetCenter = new Vector3();
                TargetCenter = Method.FutureDeviation(Target, Speed, this.transform.position) + new Vector3(0, 4.0f, 0);// Target.transform.position + new Vector3(0, 4.0f, 0);
                this.transform.rotation = Quaternion.LookRotation(TargetCenter - this.transform.position);
                // ターゲットとの距離
                float Distance = Vector3.Distance(TargetCenter, this.transform.position);
            }
            else
            {
                isSetRot = true;
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward.normalized, out hit, Mathf.Infinity))//, 1 << LayerMask.NameToLayer("Stage")))
                {
				  if(hit.collider.tag == "Enemy")
                    this.transform.rotation = Quaternion.LookRotation(hit.point - this.transform.position);
                    //Debug.Log("hit");
                }
                else
                {
                    this.transform.rotation = Camera.main.transform.rotation;
                }
            }
        }
        if (!StopFlag)
        {
            transform.Translate(Vector3.forward * Speed);
        }
	}

    /// <summary>
    /// 目標物
    /// </summary>
    public void setTargetObject(GameObject target)
    {
        TargetObject = target;
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
        //Debug.Log(collider.name);
        //Debug.Log("Target -> " + Target.name);
        //Debug.Log("Magic -> " + collider.name);
        if (collider.tag == "Stage")
        {
            StopFlag = true;
            this.rigidbody.collider.enabled = false;
            Instantiate(HitEffect, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject, 5.0f);
        }
        else if (Target == null && 
                (collider.tag == "Boss" ||
                collider.tag == "Enemy" ||
                collider.tag == "Hime"))
        {
            collider.GetComponent<EnemyStatusManager>().Damage(EnemyDamage);
            Instantiate(HitEffect, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
        if (Target != null && Target.tag == collider.tag)
        {
            if (Target.tag == "Player")
            {
                Target.GetComponent<PlayerController>().Damage(PlayerDamage);
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

    /// <summary>
    /// 自動エイムを行うかの設定
    /// </summary>
    /// <param name="set"></param>
    public void setIsAutoAim(bool set)
    {
        isAim = set;
    }
}
