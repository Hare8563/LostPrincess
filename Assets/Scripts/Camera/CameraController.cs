using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	[SerializeField]
	private GameObject target;
	[SerializeField]
	private Vector3 distance = new Vector3(0, 2, -4);
	[SerializeField]
	private Vector3 lookPoint = new Vector3(0, 0, 0);
	
	void Start () {
	}
	
	void Update () {
		this.transform.position = target.transform.position + distance;
		
		Vector3 lookVector = target.transform.position + lookPoint - this.transform.position;
		this.transform.rotation = Quaternion.LookRotation(lookVector);
	}
}
