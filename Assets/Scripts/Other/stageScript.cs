using UnityEngine;
using System.Collections;

public class stageScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

		public void OnTriggerEnter(Collider collider){
				if(collider.gameObject.CompareTag(@"Player")){
						DontDestroyOnLoad (collider);
						collider.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
						Application.LoadLevel(@"Scene2");
				}

		}
}
