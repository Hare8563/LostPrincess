using UnityEngine;
using System.Collections;

public class buttonController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ButtonClick(){
		Application.LoadLevel (@"stage");
	}

	public void ExitButtonClick(){
		Application.Quit ();
	}
}
