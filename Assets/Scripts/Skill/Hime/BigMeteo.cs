using UnityEngine;
using System.Collections;

public class BigMeteo : MonoBehaviour {

	/// <summary>
	/// 移動スピード
	/// </summary>
	[SerializeField]
	[Range(0,5)]
	private float Speed = 0;
    /// <summary>
    /// プレヤーオブジェクト
    /// </summary>
    private GameObject PlayerObject;

    /// <summary>
    /// ターゲットが次にいるであろう座標に弾が当たるまでの時間
    /// </summary>
    private float HitDistanceTime;
    /// <summary>
    /// ターゲットが次にいるであろう座標
    /// </summary>
    private Vector3 FuturePos;
    /// <summary>
    /// ターゲットとの距離
    /// </summary>
    private float toTargetDistance;

    void Awake()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        FuturePos = PlayerObject.transform.position + PlayerObject.GetComponent<PlayerController>().getVectorDistance() * 50;
        toTargetDistance = Vector3.Distance(FuturePos, this.transform.position);
        HitDistanceTime = toTargetDistance / Speed;
        Debug.Log(HitDistanceTime);
    }

	// Use this for initialization
	void Start () {
		Destroy(this.gameObject, 2.0f);
        //this.transform.LookAt(FuturePos);
		this.transform.LookAt(PlayerObject.transform.position);
	}
	
	// Update is called once per frame
	void Update () 
    {
        
	}

	void FixedUpdate()
	{
		this.transform.Translate(Vector3.forward * Speed * Method.GameTime());
	}

	void OnTriggerEnter(Collider collider)
	{
		//Debug.Log (collider.tag);
		if (collider.tag == "Player") {
			PlayerObject.GetComponent<PlayerController>().Damage(5);
			Destroy(this.gameObject);
		}
	}
}
