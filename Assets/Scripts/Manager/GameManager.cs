using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    void Awake()
    {
        Application.LoadLevelAdditive("Canvas");
    }

	// Use this for initialization
	void Start () 
    {
        Time.timeScale = 1.0f;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //ポーズ
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }
	}
}