using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyCanvasCreateScript : MonoBehaviour {
    /// <summary>
    /// 最大HP
    /// </summary>
    private float MaxHp;
    /// <summary>
    /// ゲージの個数
    /// </summary>
    private static float GaugeCount = 0;
    /// <summary>
    /// インスタンス生成したオブジェクト
    /// </summary>
    private GameObject GaugeInstance;
    /// <summary>
    /// 敵HPゲージオブジェクト
    /// </summary>
    private GameObject EnemyStatusGaugeInstance;
    /// <summary>
    /// キャンバスオブジェクト
    /// </summary>
    private GameObject canvas;

    void Awake()
    {
        EnemyStatusGaugeInstance = Resources.Load("Prefab/EnemyStatus") as GameObject;
    }

	// Use this for initialization
	void Start () 
    {
        canvas = GameObject.Find("Canvas");
    }
	
	// Update is called once per frame
	void Update () 
    {
        
	}

    /// <summary>
    /// 名前設定
    /// </summary>
    /// <param name="name"></param>
    public void setName(string name)
    {
        foreach (Transform i in this.transform)
        {
            if (i.gameObject.name == "EnemyName")
            {
                i.GetComponent<Text>().text = name;
            }
        }
    }

    /// <summary>
    /// 新規ゲージ生成
    /// </summary>
    /// <param name="maxHp">最大HP</param>
    /// <param name="name">ゲージの名前</param>
    /// <returns>生成したオブジェクト</returns>
    public EnemyCanvasHPScript Add(float maxHp, string name)
    {
        MaxHp = maxHp;
        GaugeInstance = (GameObject)Instantiate(EnemyStatusGaugeInstance, Vector3.zero, this.transform.rotation);
        GaugeInstance.GetComponent<EnemyCanvasCreateScript>().setName(name);
        //Debug.Log(maxHp);
        GaugeInstance.GetComponent<EnemyCanvasHPScript>().setMaxHp(maxHp);
        GaugeInstance.GetComponent<EnemyCanvasHPScript>().setNowHp(maxHp);
        if (canvas == null)
        {
            canvas = GameObject.Find("Canvas");
        }
        GaugeInstance.transform.parent = canvas.transform;
        GaugeInstance.transform.localScale = new Vector3(-2, 2, 2);
        GaugeInstance.transform.localPosition = new Vector3(0, -30 * GaugeCount, 0);
        Destroy(GaugeInstance.GetComponent<EnemyCanvasCreateScript>());
        GaugeCount++;
        return GaugeInstance.GetComponent<EnemyCanvasHPScript>();
    }
}
