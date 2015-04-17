using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using CSV;

public class buttonController : MonoBehaviour {
    /// <summary>
    /// 既にボタンが押されたか
    /// </summary>
    private bool isDownButton = false;

	// Use this for initialization
	void Start () 
    {
        Screen.lockCursor = false;
        UnityEngine.Cursor.visible = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ButtonClick(){
        if (!isDownButton)
        {
            isDownButton = true;
            PlayerPrefsEx prefs = new PlayerPrefsEx();
            CsvReader reader = new CsvReader("CSV/LvTable");
            prefs.SetString("NAME", "");
            prefs.SetInt("HP", reader.getParamValue(1, CsvParam.HP));
            prefs.SetInt("MP", reader.getParamValue(1, CsvParam.MP));
            prefs.SetInt("MPMAX", reader.getParamValue(1, CsvParam.MP));
            prefs.SetInt("LV", 1);
            prefs.SetInt("EXP", 0);
            prefs.SetInt("Sword", reader.getParamValue(1, CsvParam.SWORD_ATK));
            prefs.SetInt("Magic", reader.getParamValue(1, CsvParam.MAGIC_ATK));
            prefs.SetInt("Bow", reader.getParamValue(1, CsvParam.BOW_ATK));
            prefs.Save(System.Environment.CurrentDirectory + "/saveData.xml");
            LoadingController.NextScene("Prologue");
        }
		//Application.LoadLevel (@"stage");
	}

	public void ExitButtonClick(){
		Application.Quit ();
	}
}
