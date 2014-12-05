using UnityEngine;
using System.Collections;

public class BigMeteo : MonoBehaviour {

	/// <summary>
	/// 移動スピード
	/// </summary>
	[SerializeField]
	[Range(0,5)]
	private float Speed = 0;

	// Use this for initialization
	void Start () {
		Destroy(this.gameObject, 5.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate()
	{
		this.transform.Translate(Vector3.forward * Speed);
	}
}
