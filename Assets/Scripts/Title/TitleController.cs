using UnityEngine;
using System.Collections;

public class TitleController : MonoBehaviour {

    /// <summary>
    /// 選択肢に用いる変数
    /// </summary>
    private int SelectValue = 0;
    /// <summary>
    /// 選択肢の数
    /// </summary>
    private int SelectNum = 2;
    /// <summary>
    /// 選択肢テキスト
    /// </summary>
    private GameObject[] SelectText;
    /// <summary>
    /// 3Dテキストの最大サイズ
    /// </summary>
    private int MaxFontSize = 400;
    /// <summary>
    /// 3Dテキストの最小サイズ
    /// </summary>
    private int MinFontSize = 300;

    void Awake()
    {
        SelectText = GameObject.FindGameObjectsWithTag("TitleSelect");
    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    //上
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Method.Selecting(ref SelectValue, SelectNum, "up");
        }
        //下
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Method.Selecting(ref SelectValue, SelectNum, "down");
        }

        TextMesh tm0 = (TextMesh)SelectText[0].GetComponent(typeof(TextMesh));
        TextMesh tm1 = (TextMesh)SelectText[1].GetComponent(typeof(TextMesh));
        //選択肢テキストの挙動
        switch (SelectValue)
        {
            case 0:
                tm0.fontSize = MaxFontSize;
                tm1.fontSize = MinFontSize;
                JumpScene("stage");
                break;

            case 1:
                tm0.fontSize = MinFontSize;
                tm1.fontSize = MaxFontSize;
                Application.Quit();
                break;
        }
	}

    /// <summary>
    /// シーン移動
    /// </summary>
    /// <param name="name"></param>
    void JumpScene(string name)
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            LoadingController.NextScene(name);
        }
    }
}
