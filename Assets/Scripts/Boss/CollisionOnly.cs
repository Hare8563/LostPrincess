using UnityEngine;
using System.Collections;

public class CollisionOnly : MonoBehaviour {

	private float StayTime = 0;
	/// <summary>
	/// プレイヤーオブジェクト
	/// </summary>
	private GameObject PlayerObject;

	void Awake()
	{
		PlayerObject = GameObject.FindGameObjectWithTag("Player");
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider collider)
	{
		if(collider.tag == "Player")
		{
			StayTime++;
			if(StayTime % 30 == 0)
			{
				StayTime = 0;
				PlayerObject.GetComponent<PlayerController>().Damage(5);
			}
		}
	}
}
