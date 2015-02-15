using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AimCursorManager : MonoBehaviour {

    /// <summary>
    /// uGUIキャンバス
    /// </summary>
    private GameObject canvas;
    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    private GameObject PlayerObject;
    /// <summary>
    /// 照準可能か
    /// </summary>
    private bool canAim = false;
	/// <summary>
	/// 現在ロックオンしているオブジェクト
	/// </summary>
	private GameObject LockOnObject;

    public GameObject test;

    void Awake()
    {
        PlayerObject = GameObject.FindWithTag("Player");
    }

	// Use this for initialization
	void Start () 
    {
        canvas = GameObject.Find("Canvas");
	}
	
	// Update is called once per frame
    void Update()
    {
        //SearchObject();
        //MoveAimCursor();
    }

	/// <summary>
	/// ロックオンするオブジェクトを探す
	/// </summary>
	private void SearchObject()
	{
		float SearchSize = 100f;
		float new_dis = 9999;
		float old_dis = 9999;
        Vector3 StoW = new Vector3();
        //延長線上に障害物があれば照準を非表示
        RaycastHit hit;
        //カメラの座標
        Vector3 origin = Camera.main.transform.position;
        //カメラの向いているベクトル
        Vector3 to = Camera.main.transform.TransformDirection(Vector3.forward);
        //カメラからの距離
        float toCameraDis = 1000f;
        //カメラの視線上に障害物があったら
        if (Physics.Raycast(origin, to, out hit, toCameraDis))
        {
            StoW = hit.point;
        }
		//指定範囲内のオブジェクトコライダを検索
		Collider[] targets = Physics.OverlapSphere(StoW, SearchSize);
		//画面中心に一番近い敵オブジェクトを検索
		foreach(Collider obj in targets)
		{
            if (obj.tag == "Enemy" ||
                obj.tag == "Boss" ||
                obj.tag == "Hime")
            {
                new_dis = Vector3.Distance(StoW, obj.gameObject.transform.position);
                if (new_dis < old_dis)
                {
                    LockOnObject = obj.gameObject;
                }
                old_dis = Vector3.Distance(StoW, obj.gameObject.transform.position);
            }
		}
        new_dis = 9999;
        old_dis = 9999;

        //test.transform.position = StoW;
        //test.transform.localScale = new Vector3(SearchSize, SearchSize, SearchSize);
	}

	/// <summary>
	///照準カーソルの処理
	/// </summary>
	private void MoveAimCursor()
	{
        if (LockOnObject != null)
        {
            //延長線上に障害物があれば照準を非表示
            RaycastHit hit;
            //カメラの座標
            Vector3 origin = Camera.main.transform.position;
            //カメラから敵へのベクトル
            Vector3 to = (LockOnObject.transform.position + new Vector3(0, 4.0f, 0)) - origin;
            //カメラと敵座標との距離
            float dis = Vector3.Distance(origin, LockOnObject.transform.position + new Vector3(0, 4.0f, 0));
            //カメラと敵との間に障害物があったら
            if (Physics.Raycast(origin, to, out hit, dis, 1 << LayerMask.NameToLayer("Stage")))
            {
                canAim = false;
            }
            else
            {
                canAim = true;
            }
            //Debug.DrawLine(origin, EnemyObject.transform.position + new Vector3(0, 4.0f, 0));
            //Debug.Log(canAim);
            //敵のいる場所にカーソル移動
            foreach (Transform child in canvas.transform)
            {
                if (child.name == "AimCursor")
                {
                    //ビューポート内の敵の位置座標
                    Vector3 WtoV = Camera.main.WorldToViewportPoint(LockOnObject.transform.position + new Vector3(0, 15.0f, 0));
                    //画面内だったら表示
                    if (0.0f < WtoV.x && WtoV.x < 1.0f &&
                        0.0f < WtoV.y && WtoV.y < 1.0f &&
                        0.0f < WtoV.z && canAim)
                    {
                        child.GetComponent<Image>().enabled = true;
                    }
                    else
                    {
                        child.GetComponent<Image>().enabled = false;
                        //ターゲットを外す
                        LockOnObject = null;
                    }
                    //Debug.Log(child.GetComponent<Image>().enabled);
                    child.GetComponent<Image>().rectTransform.transform.position = new Vector3(WtoV.x * Screen.width, WtoV.y * Screen.height, 0);
                }
            }
        }
        else
        {
            foreach (Transform child in canvas.transform)
            {
                if (child.name == "AimCursor")
                {
                    child.GetComponent<Image>().enabled = false;
                }
            }
        }
	}

	/// <summary>
	/// ロックオンしているオブジェクト
	/// </summary>
	/// <returns>The lock on object.</returns>
	public GameObject getLockOnObject()
	{
		return LockOnObject;
	}
}
