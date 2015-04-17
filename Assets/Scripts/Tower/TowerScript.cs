using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerScript : MonoBehaviour
{

    /// <summary>
    /// 自身のステータス
    /// </summary>
    private EnemyStatusManager thisStatus;
    /// <summary>
    /// 全子オブジェクト
    /// </summary>
    private Transform[] ChildList;
    /// <summary>
    /// 子オブジェクトに与える力
    /// </summary>
    private Vector3 forceVec;
    /// <summary>
    /// 死亡フラグ
    /// </summary>
    private bool DeadFlag = false;
    /// <summary>
    /// 死んだ瞬間に消したいオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject[] Destroys;
    /// <summary>
    /// バラバラになったオブジェクト
    /// </summary>
    private List<GameObject> BrokenObjects = new List<GameObject>();
    /// <summary>
    /// 子オブジェクトの個数
    /// </summary>
    private int ChildCount = 0;
    /// <summary>
    /// 削除した個数
    /// </summary>
    private int BrokenCount = 0;
    /// <summary>
    /// 崩壊エフェクト
    /// </summary>
    private GameObject BlokenDUst;
    /// <summary>
    /// 崩壊効果音
    /// </summary>
    [SerializeField]
    private AudioClip BlokenSound;
    /// <summary>
    /// HPゲージオブジェクト
    /// </summary>
    private EnemyCanvasHPScript HPGaugeObject;

    void Awake()
    {
        thisStatus = this.GetComponent<EnemyStatusManager>();
        ChildList = this.transform.GetComponentsInChildren<Transform>();
        BlokenDUst = Resources.Load("Prefab/BlokenDust") as GameObject;
    }

    // Use this for initialization
    void Start()
    {
        ChildCount = this.gameObject.transform.childCount;
        HPGaugeObject = this.GetComponent<EnemyCanvasCreateScript>().Add(this.thisStatus.getStatus().HP, "守りの塔");
        //Debug.Log(ChildCount);
    }

    // Update is called once per frame
    void Update()
    {
        HPGaugeObject.setNowHp(this.thisStatus.getStatus().HP);
        if (this.thisStatus.getStatus().HP <= 0)
        {
            if (!DeadFlag)
            {
                float range = 15;
                DeadFlag = true;
                //エフェクト生成
                Instantiate(BlokenDUst, this.transform.position, this.transform.rotation);
                //効果音再生
                GetComponent<AudioSource>().PlayOneShot(BlokenSound);
                //子オブジェクトをバラバラに
                foreach (Transform i in ChildList)
                {
                    i.transform.parent = null;
                    BrokenObjects.Add(i.gameObject);
                    forceVec = new Vector3(Random.Range(-range, range), Random.Range(-range, 0), Random.Range(-range, range));
                    if (i.gameObject.GetComponent<Rigidbody>() == null) i.gameObject.AddComponent<Rigidbody>();
                    i.gameObject.GetComponent<Rigidbody>().AddForce(forceVec, ForceMode.Impulse);
                    i.gameObject.GetComponent<Rigidbody>().AddTorque(forceVec / 5, ForceMode.Impulse);
                    i.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.down * 5, ForceMode.Impulse);
                }
                //死んだ瞬間に消したいオブジェクトを削除
                foreach (GameObject i in Destroys)
                {
                    Destroy(i);
                }
                //タワーのコライダをOFF
                this.GetComponent<Rigidbody>().GetComponent<Collider>().enabled = false;
            }
            //バラバラになったオブジェクトが落ちたら削除
            foreach (GameObject i in BrokenObjects)
            {
                if (i != null && i.transform.position.y < -20)
                {
                    BrokenCount++;
                    Destroy(i.gameObject);
                }
            }
            //全子オブジェクトが削除されたら自身を削除
            if (BrokenCount >= ChildCount)
            {
                Destroy(this.gameObject);
            }
        }
    }
}