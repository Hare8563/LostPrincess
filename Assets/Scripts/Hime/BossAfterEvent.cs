using UnityEngine;
using System.Collections;

public class BossAfterEvent : MonoBehaviour {
    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    private GameObject PlayerObject;
    /// <summary>
    /// 移動スピード
    /// </summary>
    private float Speed = 0.2f;
    /// <summary>
    /// CageScriptクラス
    /// </summary>
    private CageScript cageScript;
    /// <summary>
    /// イベントコントローラー
    /// </summary>
    private GameObject eventController;
    /// <summary>
    /// エンディングへ向かうフラグ
    /// </summary>
    private bool toEndingFlag = false;

    void Awake()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        cageScript = GameObject.Find("cage01").GetComponent<CageScript>();
        eventController = GameObject.Find("EventManager");
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (cageScript.getIsCageOpen())
        {
            this.transform.LookAt(PlayerObject.transform.position);
            this.transform.rotation = new Quaternion(0, this.transform.rotation.y, this.transform.rotation.z, this.transform.rotation.w);
            this.transform.Translate(Vector3.forward * Speed);
        }
        if (toEndingFlag)
        {
            eventController.GetComponent<EventController>().FadeOut("Ending", 0.7f);
        }
        else
        {
            eventController.GetComponent<EventController>().FadeIn(0.7f);
        }
	}

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if (other.name == "toEndingEventCollider")
        {
            toEndingFlag = true;
        }
    }
}
