using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour {

    /// <summary>
    /// 取得するアイテムの列挙体
    /// </summary>
    private enum Items
    {
        HP,
        AMMO,
    }
    /// <summary>
    /// 取得するアイテム
    /// </summary>
    private Items items;
    /// <summary>
    /// 取得するアイテムのランダム値
    /// </summary>
    private int ItemRand;
    /// <summary>
    /// アイテムの色
    /// </summary>
    [SerializeField]
    private Color[] ItemColor;
    /// <summary>
    /// アイテムの名前
    /// </summary>
    private string ItemName;
    /// <summary>
    /// 回復HPの量
    /// </summary>
    [SerializeField]
    private int RejectHp;
    /// <summary>
    /// 回復矢の量
    /// </summary>
    [SerializeField]
    private int RejectAmmo;
    /// <summary>
    /// 移動スピード
    /// </summary>
    [SerializeField]
    private float Speed;
    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    private GameObject PlayerObject;
    /// <summary>
    /// アイテム取得エフェクト
    /// </summary>
    private GameObject GetEffect;
    /// <summary>
    /// アイテム取得エフェクトのインスタンス
    /// </summary>
    private GameObject GetEffectInst;

    void Awake()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        GetEffect = Resources.Load("Prefab/GetItem") as GameObject;
    }

	// Use this for initialization
	void Start () 
    {
        ItemRand = (int)Random.Range(0, 2);
        this.GetComponent<ParticleSystem>().startColor = ItemColor[ItemRand];
	}
	
	// Update is called once per frame
	void Update () 
    {
        this.transform.LookAt(PlayerObject.transform.position + new Vector3(0, 2f, 0));
        this.transform.Translate(Vector3.forward * Speed);
        if (GetEffectInst != null)
        {
            GetEffectInst.transform.localPosition = Vector3.zero;
        }
	}

    /// <summary>
    /// 何かに当たったら
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            switch (ItemRand)
            {
                case 0:
                    other.GetComponent<PlayerController>().getStatus().HP += RejectHp;
                    break;
                case 1:
                    other.GetComponent<PlayerController>().getStatus().AMMO += RejectAmmo;
                    break;
            }
            GetEffectInst = (GameObject)Instantiate(GetEffect, PlayerObject.transform.position, GetEffect.transform.rotation);
            GetEffectInst.GetComponent<ParticleSystem>().startColor = ItemColor[ItemRand];
            GetEffectInst.transform.parent = PlayerObject.transform;
            this.GetComponent<ParticleSystem>().enableEmission = false;
            Destroy(this.gameObject, 3.0f);
        }
    }
}
