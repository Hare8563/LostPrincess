using UnityEngine;
using System.Collections;

public class buttonController : MonoBehaviour {
    /// <summary>
    /// 既にボタンが押されたか
    /// </summary>
    private bool isDownButton = false;

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
        if (!isDownButton)
        {
            isDownButton = true;
            Application.LoadLevelAdditive("InputFormScene");
        }
		//Application.LoadLevel (@"stage");
	}

	public void ExitButtonClick(){
		Application.Quit ();
	}
}
