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
    /// ターゲットオブジェクト
    /// </summary>
    private GameObject TargetObject;

    void Awake()
    {
<<<<<<< HEAD
        TargetObject = GameObject.FindGameObjectWithTag("Player");
=======
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        FuturePos = PlayerObject.transform.position + PlayerObject.GetComponent<PlayerController>().getVectorDistance() * 50;
        toTargetDistance = Vector3.Distance(FuturePos, this.transform.position);
        HitDistanceTime = toTargetDistance / Speed;
        Debug.Log(HitDistanceTime);
>>>>>>> 33cf050060cbfdc1e5eea16656a7b41305108be1
    }

	// Use this for initialization
	void Start () {
		Destroy(this.gameObject, 2.0f);
		this.transform.LookAt(TargetObject.transform.position);
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
			TargetObject.GetComponent<PlayerController>().Damage(5);
			Destroy(this.gameObject);
		}
	}

    /// <summary>
    /// ターゲットを切り替える
    /// </summary>
    public void SetTarget(GameObject target)
    {
        TargetObject = target;
    }
}
