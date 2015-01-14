using UnityEngine;
using System.Collections;

public class TornadoEffect : MonoBehaviour {

	/// <summary>
	/// プレイヤーオブジェクト
	/// </summary>
	private GameObject PlayerObject;
	/// <summary>
	/// ダメージを受けるタイミング
	/// </summary>
	private int DamageTiming = 0;

	void Awake()
	{
		PlayerObject = GameObject.FindGameObjectWithTag ("Player");
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float dis = Vector3.Distance (PlayerObject.transform.position, this.transform.position);
		if (dis < 40) {
			DamageTiming++;
			if(DamageTiming % 30 == 0)
			{
				DamageTiming = 0;
				PlayerObject.GetComponent<PlayerController>().Damage(5);
			}
		}
	}
}
