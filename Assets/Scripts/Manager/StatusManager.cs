using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class StatusManager : MonoBehaviour {
    /// <summary>
    /// uGUIキャンバス
    /// </summary>
    private GameObject canvas;
    /// <summary>
    /// セーブ/ロードを行うクラス
    /// </summary>
    private PlayerPrefsEx prefs;
    /// <summary>
    /// ステータスの構造体
    /// </summary>
    public struct StatusStruct
    {
        public string NAME;
        public int HP;
        public int MP;
        public int LV;
        public int EXP;
        public int SWORD;
        public int BOW;
        public int MAGIC;
    };
    /// <summary>
    /// プレイヤーステータスの構造体変数
    /// </summary>
    private StatusStruct PlayerStatus;
    /// <summary>
    /// 姫ステータスの構造体変数
    /// </summary>
    private StatusStruct HimeStatus;

    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    private GameObject PlayerObject;
    /// <summary>
    /// 姫オブジェクト
    /// </summary>
    private GameObject HimeObject;
    /// <summary>
    /// 姫戦かどうか
    /// </summary>
    public bool isHimeBattle = false;
    

    /// <summary>
    /// 読み込み
    /// </summary>
    void Awake()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        if (isHimeBattle) HimeObject = GameObject.FindGameObjectWithTag("Hime");
    }

	/// <summary>
	/// 初期化
	/// </summary>
	void Start () 
    {
        canvas = GameObject.Find("Canvas");
	}
	
	/// <summary>
	/// 更新
	/// </summary>
	void Update () 
    {
        //GUI管理
        StatusGUIController();
	}

    /// <summary>
    /// ロード
    /// </summary>
    void Load()
    {
        //Debug.Log(System.Environment.CurrentDirectory);
        prefs = new PlayerPrefsEx();
        prefs.Load(System.Environment.CurrentDirectory + "/saveData.xml");
        PlayerStatus.NAME = prefs.GetString("NAME");// Debug.Log("NAME = " + statusStruct.NAME);
        PlayerStatus.HP = prefs.GetInt("HP"); //Debug.Log("HP = " + statusStruct.HP);
        PlayerStatus.MP = prefs.GetInt("MP"); //Debug.Log("MP = " + statusStruct.MP);
        PlayerStatus.LV = prefs.GetInt("LV"); //Debug.Log("LV = " + statusStruct.LV);
        PlayerStatus.EXP = prefs.GetInt("EXP"); //Debug.Log("EXP = " + statusStruct.EXP);
        PlayerStatus.SWORD = prefs.GetInt("Sword"); //Debug.Log("SWORD = " + statusStruct.SWORD);
        PlayerStatus.BOW = prefs.GetInt("Bow"); //Debug.Log("BOW = " + statusStruct.BOW);
        PlayerStatus.MAGIC = prefs.GetInt("Magic"); //Debug.Log("MAGIC = " + statusStruct.MAGIC);
    }

    /// <summary>
    /// ステータスGUI管理
    /// </summary>
    void StatusGUIController()
    {
        PlayerStatusControll();
        if (isHimeBattle) HimeStatusControll();
    }

    /// <summary>
    /// プレイヤーステータスGUI管理
    /// </summary>
    private void PlayerStatusControll()
    {
        PlayerController playerController = PlayerObject.GetComponent<PlayerController>();
        foreach (Transform child in canvas.transform)
        {
            //Debug.Log(child.name);
            if (child.name == "NAME")
            {
                child.gameObject.GetComponent<Text>().text = playerController.getStatus().NAME;//statusStruct.NAME;
            }
            else if (child.name == "HP")
            {
                child.gameObject.GetComponent<Text>().text = playerController.getStatus().HP.ToString();//statusStruct.HP.ToString();
            }
            else if (child.name == "MP")
            {
                child.gameObject.GetComponent<Text>().text = playerController.getStatus().MP.ToString();//statusStruct.MP.ToString();
            }
            else if (child.name == "LV")
            {
                child.gameObject.GetComponent<Text>().text = playerController.getStatus().LEV.ToString();//statusStruct.LV.ToString();
            }
            else if (child.name == "AMMO")
            {
                child.gameObject.GetComponent<Text>().text = playerController.getStatus().AMMO.ToString();//statusStruct.LV.ToString();
            }
        }
    }

    /// <summary>
    /// 姫ステータス管理
    /// </summary>
    private void HimeStatusControll()
    {
        RastBossController himeController = HimeObject.GetComponent<RastBossController>();
        foreach (Transform child in canvas.transform)
        {
            //HP反映
            if (child.name == "HimeHP_Bar")
            {
                child.gameObject.GetComponent<Image>().rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, himeController.getNowHP());
            }
        }
    }

    /// <summary>
    /// ロードしたステータスを得る
    /// </summary>
    /// <returns></returns>
    public StatusStruct getLoadStatus()
    {
        Load();
        return PlayerStatus;
    }
}
