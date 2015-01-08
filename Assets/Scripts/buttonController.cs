using UnityEngine;
using System.Collections;

public class buttonController : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        Screen.lockCursor = false;
        Screen.showCursor = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ButtonClick(){
				Application.LoadLevelAdditive ("InputFormScene");
				//Application.LoadLevel (@"stage");
	}

	public void ExitButtonClick(){
		Application.Quit ();
	}
}
