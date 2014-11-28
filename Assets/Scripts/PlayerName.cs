using UnityEngine;
using System.Collections;

public class PlayerName : MonoBehaviour {

    /// <summary>
    /// テキストフィールド
    /// </summary>
    private string textToEdit;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

	// Use this for initialization
	void Start () {
        textToEdit = "";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// GUI表示
    /// </summary>
    void OnGUI()
    {
        // テキストフィールドを表示する
        //textToField = GUI.TextField(new Rect(10, 10, 100, 100), textToEdit);
    }
}
