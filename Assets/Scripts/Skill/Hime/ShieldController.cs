using UnityEngine;
using System.Collections;

public class ShieldController : MonoBehaviour {

    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    private GameObject PlayerObject;
    /// <summary>
    /// 姫オブジェクト
    /// </summary>
    private GameObject HimeObeject;
    /// <summary>
    /// 見る方向
    /// </summary>
    private Vector3 LookPosition;
    /// <summary>
    /// 見るフラグ
    /// </summary>
    private bool LookFlag = false;
    /// <summary>
    /// アルファ値
    /// </summary>
    private float a = 0;
    /// <summary>
    /// 衝突する対象の名前
    /// </summary>
    private string toCollisionName;

    void Awake()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        HimeObeject = GameObject.FindGameObjectWithTag("Hime");
    }

	// Use this for initialization
	void Start () 
    {
        toCollisionName = "";
        foreach (Transform child in this.transform)
        {
            child.gameObject.gameObject.renderer.material.SetColor("_TintColor", new Color(1, 1, 1, 0));
        } 
	}
	
	// Update is called once per frame
	void Update () 
    {
        this.transform.position = HimeObeject.transform.position + new Vector3(0, 5, 0);
        //透明度を下げる
        if (a > 0)
        {
            a -= 0.03f;
        }
        foreach (Transform child in this.transform)
        {
            child.gameObject.gameObject.renderer.material.SetColor("_TintColor", new Color(a, a, a, a));
        }
        //Debug.Log(toCollisionName);
	}

    /// <summary>
    /// シールドが見る方向をセット
    /// </summary>
    /// <param name="lookPos"></param>
    public void setLookPosition(Vector3 lookPos)
    {
        a = 1;
        this.transform.LookAt(lookPos - this.transform.position);
        LookFlag = true;
    }

    /// <summary>
    /// シールド通過許可対象を設定する
    /// </summary>
    /// <param name="name">許可するオブジェクトの名前</param>
    public void setToShieldCollision(string name)
    {
        toCollisionName = name + "(Clone)";
    }

    /// <summary>
    /// 何かに当たったら
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerEnter(Collider collider)
    {
        if (collider.name != toCollisionName)
        {
            a = 1;
            this.transform.LookAt(collider.gameObject.transform.position);
            Destroy(collider.gameObject);
        }
    }
}
