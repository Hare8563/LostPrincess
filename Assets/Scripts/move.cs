using UnityEngine;
using System.Collections;

public class move : MonoBehaviour {

	public float speed = 5.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.LeftArrow)){
			this.transform.Translate(Vector3.left);
		}
	}
}