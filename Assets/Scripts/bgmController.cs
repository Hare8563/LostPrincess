using UnityEngine;
using System.Collections;

public class bgmController : MonoBehaviour {
	[SerializeField]
	private GameObject obj;
	// Use this for initialization
	void Start () {
		this.transform.position = obj.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
