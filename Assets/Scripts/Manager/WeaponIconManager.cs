using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponIconManager : MonoBehaviour {

    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    private GameObject PlayerObject;
    /// <summary>
    /// uGUIキャンバス
    /// </summary>
    public GameObject canvas;
    /// <summary>
    /// 武器アイコンの構造体
    /// </summary>
    private struct Icon
    {
        public GameObject SWORD;
        public GameObject MAGIC;
        public GameObject BOW;
    }
    /// <summary>
    /// 武器アイコン
    /// </summary>
    private Icon icon;
    /// <summary>
    /// 武器アイコンオブジェクト
    /// </summary>
    private GameObject WeaponIconObject;

    void Awake()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        canvas = GameObject.Find("Canvas");
    }

	// Use this for initialization
	void Start () 
    {
        foreach (Transform child in canvas.transform)
        {
            if (child.name == "Sword")
            {
                icon.SWORD = child.gameObject;
            }
            else if (child.name == "Magic")
            {
                icon.MAGIC = child.gameObject;
            }
            else if (child.name == "Bow")
            {
                icon.BOW = child.gameObject;
            }
        }
        //Debug.Log(icon.SWORD);
	}
	
	// Update is called once per frame
	void Update () 
    {
        int weapon = PlayerObject.GetComponent<PlayerController>().getNowWeapon();
        switch (weapon)
        {
            case 0:
                Create(icon.SWORD);
                break;
            case 1:
                Create(icon.MAGIC);
                break;
            case 2:
                Create(icon.BOW);
                break;
        }
	}

    void Create(GameObject icon)
    {
        float scaleSpeed = 1.005f;
        float colorSpeed = 0.02f;
        if (WeaponIconObject == null)
        {
            WeaponIconObject = Instantiate(icon, icon.transform.position, icon.transform.rotation) as GameObject;
            WeaponIconObject.transform.parent = canvas.transform;
            WeaponIconObject.transform.localScale = new Vector3(1, 1, 1);
        }
        //サイズ変更
        WeaponIconObject.transform.localScale *= scaleSpeed;
        //アルファ値変更
        float a = WeaponIconObject.GetComponent<Image>().color.a;
        a -= colorSpeed;
        WeaponIconObject.GetComponent<Image>().color = new Color(1, 1, 1, a);
        //アルファ値が0になったら削除
        if (a <= 0.0f)
        {
            a = 1;
            Destroy(WeaponIconObject);
        }
        //Debug.Log(WeaponIconObject.GetComponent<Image>().color.a);
    }
}
