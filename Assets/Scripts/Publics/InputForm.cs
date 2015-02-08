using UnityEngine;
using System.Collections;
using AssemblyCSharp;
public class InputForm : MonoBehaviour {
	public static string InputFormName=@"";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI(){

		GUIStyle gui = new GUIStyle ();
		//gui.fontSize = 20;
		gui.normal.textColor = Color.red;
		int formWidth = 200;
		int formHeight = 400;
		int width = Screen.width;
		int height = Screen.height;
		//全体のボックス
		GUI.Box (new Rect ((width-formWidth)/2, (height-formHeight)/2, formWidth, formHeight), "名前を入力してください");
		Rect rect1 = new Rect((formWidth - 150)/2 + (width-formWidth)/2,
						  	 (formHeight - 200)/2+(height-formHeight)/2 , 150, 50);
		//名前入力フォーム
		InputFormName=GUI.TextField(rect1, InputFormName);
		Rect rect2 = new Rect((formWidth - 70)/2 + (width-formWidth)/2,
							  (formHeight - 200)/2+(height-formHeight)/2+100 , 70, 20);
		if (GUI.Button (rect2, "決定") || Event.current.keyCode == KeyCode.Return) {
			//クリックしたときの関数実行
			PlayerPrefsEx prefs = new PlayerPrefsEx ();
			prefs.SetString ("NAME", InputFormName);
			prefs.SetInt ("HP", 100);
			prefs.SetInt ("MP", 100);
			prefs.SetInt ("LV", 1);
			prefs.SetInt ("EXP", 0);
			prefs.SetInt ("Sword", 10);
			prefs.SetInt ("Magic", 8);
			prefs.SetInt ("Bow", 5);
			prefs.Save (System.Environment.CurrentDirectory + "/saveData.xml");
            LoadingController.NextScene("stage");
		}

	}

}
