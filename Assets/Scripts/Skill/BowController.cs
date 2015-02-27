using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BowController : MonoBehaviour {
    /// <summary>
    /// 速さ
    /// </summary>
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
    /// 矢を停止させるフラグ
    /// </summary>
    private bool StopFlag = false;
    /// <summary>
    /// 矢エフェクトクラス
    /// </summary>
    private ArrowEffectScript arrowEffect = new ArrowEffectScript();
    /// <summary>
    /// チャージした時間
    /// </summary>
    private float ChargeTime = 0;
    /// <summary>
    /// チャージした段階
    /// </summary>
    private int ChargeIndex = 0;
    /// <summary>
    /// チャージ中かどうか
    /// </summary>
    private bool isCharge = true;

    void Awake()
    {
        HitEffect = Resources.Load("Prefab/HitEffect") as GameObject;
        arrowEffect = this.GetComponent<ArrowEffectScript>();
    }

	// Use this for initialization
	void Start () 
    {
        isSetRot = false;
        //Destroy(this.gameObject, DestroyTime);
        if (Target == null && TargetObject != null)
        {
            Target = TargetObject;
        }
        arrowEffect.setChargeEffectEmit(true);
        arrowEffect.setShotEffectEmit(false);
	}
	
	// Update is called once per frame
	void Update () 
    {
        //まだ打ってなかったら
        if (!isSetRot)
        {
            //ターゲットが存在していたら
            if (Target != null)
            {
                isSetRot = true;
                Vector3 TargetCenter = new Vector3();
                TargetCenter = Method.FutureDeviation(Target, Speed + 3, this.transform.position) + new Vector3(0, 3.0f, 0);// Target.transform.position + new Vector3(0, 4.0f, 0);
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
                    if (hit.collider.tag == "Enemy")
                        this.transform.rotation = Quaternion.LookRotation(hit.point - this.transform.position);
                    //Debug.Log("hit");
                }
                else
                {
                    this.transform.rotation = Camera.main.transform.rotation;
                }
            }
        }
	}

	void FixedUpdate()
    {
        //移動可能だったら
        if (!StopFlag)
        {
            transform.Translate(Vector3.forward * Speed);
            arrowEffect.setChargeEffectEmit(false);
            arrowEffect.setShotEffectEmit(true);
            Destroy(this.gameObject, DestroyTime);
        }
        //移動不可だったら
        else
        {
            //チャージ中だったら
            if (isCharge)
            {
                ChargeTime += Method.GameTime();
                if ((int)ChargeTime % 30 == 0 && ChargeIndex < 2)
                {
                    ChargeTime = 0;
                    ChargeIndex++;
                    arrowEffect.setColorNumber(ChargeIndex);
                }
                arrowEffect.setChargeEffectEmit(true);
                arrowEffect.setShotEffectEmit(false);
            }
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
        if (collider.tag == "Stage")
        {
            StopFlag = true;
            this.GetComponent<ArrowEffectScript>().setChargeEffectEmit(false);
            this.GetComponent<ArrowEffectScript>().setShotEffectEmit(false);
            this.rigidbody.collider.enabled = false;
            Instantiate(HitEffect, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject, 5.0f);
        }
        else if (Target == null && 
                (collider.tag == "Boss" ||
                collider.tag == "Enemy" ||
                collider.tag == "Hime") &&
                !StopFlag)
        {
            StopFlag = true;
            //this.transform.parent = collider.gameObject.transform;
            collider.GetComponent<EnemyStatusManager>().Damage(EnemyDamage * (((ChargeIndex + 1) / 3) + 1));
            Instantiate(HitEffect, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
        if (Target != null && 
            Target.tag == collider.tag)
        {
            if (Target.tag == "Player")
            {
                StopFlag = true;
                //this.transform.parent = collider.gameObject.transform;
                Target.GetComponent<PlayerController>().Damage(PlayerDamage);
            }
            
            Instantiate(HitEffect, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
        else
        {
            if (collider.tag == "Enemy")
            {
                StopFlag = true;
                //this.transform.parent = collider.gameObject.transform;
                collider.GetComponent<EnemyStatusManager>().Damage(EnemyDamage);
                Instantiate(HitEffect, this.transform.position, this.transform.rotation);
                Destroy(this.gameObject);
            }
        }
    }

    /// <summary>
    /// 移動停止するかどうか指定
    /// </summary>
    /// <param name="value"></param>
    public void setMoveStop(bool value)
    {
        StopFlag = value;
    }

    /// <summary>
    /// チャージ中かどうか設定
    /// </summary>
    public void setIsCharge(bool value)
    {
        isCharge = value;
    }
}
