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

    void Awake()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        HimeObeject = GameObject.FindGameObjectWithTag("Hime");
    }

	// Use this for initialization
	void Start () 
    {
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
	}

    /// <summary>
    /// シールドが見る方向をセット
    /// </summary>
    /// <param name="lookPos"></param>
    public void SetLookPosition(Vector3 lookPos)
    {
        a = 1;
        this.transform.LookAt(lookPos - this.transform.position);
        LookFlag = true;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag != "Hime" && collider.tag != "Player")
        {
            a = 1;
            this.transform.LookAt(collider.gameObject.transform.position);
            Destroy(collider.gameObject);
        }
    }
}
