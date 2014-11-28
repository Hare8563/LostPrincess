using UnityEngine;
using System.Collections;

public class Meteo : MonoBehaviour {

	/// <summary>
	/// スピード
	/// </summary>
	[SerializeField]
	[Range(0,100)]
	private float Speed;

	// Use this for initialization
	void Start () {

        Destroy(this.gameObject, 5.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/// <summary>
	/// 何かに当たったら
	/// </summary>
	/// <param name="collider"></param>
	void OnTriggerEnter(Collider collider)
	{
		if(collider.tag == TargetTag)
		{
			if(TargetTag == "Player"){ collider.GetComponent<PlayerController>().Damage(5); }
			else if(TargetTag == "Boss"){ collider.GetComponent<BossController>().Damage(5); }
			else if(TargetTag == "Enemy"){ collider.GetComponent<EnemyScript>().Damage(5); }
			Destroy(this.gameObject);
		}
	}
	
	/// <summary>
	/// ターゲットのタグ
	/// </summary>
	/// <value>The target tag.</value>
	public string TargetTag{set; get;}
}
