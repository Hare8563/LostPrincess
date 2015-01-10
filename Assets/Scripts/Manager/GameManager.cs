using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }
	}
}
