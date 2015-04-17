using UnityEngine;
using System.Collections;

public class LoadingController : MonoBehaviour {
    /// <summary>
    /// シーン名
    /// </summary>
    private static string SceneName = "";
    /// <summary>
    /// ゲーム最初のロード画面か
    /// </summary>
    private static bool isInit = true;

	// Use this for initialization
	void Start () 
    {
        if (SceneName == "")
        {
            Application.LoadLevel("Title");
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (SceneName != "")
        {
            Application.LoadLevel(SceneName);
        }
        else
        {
            Application.LoadLevel("Title");
        }
	}

    /// <summary>
    /// 次に読み込むシーンを設定
    /// </summary>
    /// <param name="scenename">シーン名</param>
    public static void NextScene(string name)
    {
        SceneName = name;
        Application.LoadLevel("Load");
    }
}
