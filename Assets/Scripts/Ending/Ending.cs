using UnityEngine;
using System.Collections;

public class Ending : MonoBehaviour {

    /// <summary>
    /// イベントクラス
    /// </summary>
    private GameObject EventManager;
    /// <summary>
    /// 次の遷移先
    /// </summary>
    [SerializeField]
    private string NextSccene;

    void Awake()
    {
        EventManager = GameObject.Find("EventManager");
    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetMouseButton(0))
        {
            LoadingController.NextScene(NextSccene);
        }
        //ホワイトイン
        EventManager.GetComponent<EventController>().FadeIn(0.2f);
	}
}
