using UnityEngine;
using System.Collections;

public class LoadingController : MonoBehaviour {
    /// <summary>
    /// シーン名
    /// </summary>
    private static string SceneName = "";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        Application.LoadLevel(SceneName);
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
