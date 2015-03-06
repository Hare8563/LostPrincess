﻿using UnityEngine;
using System.Collections;

public class Ending : MonoBehaviour {

    /// <summary>
    /// イベントクラス
    /// </summary>
    private GameObject EventManager;

    void Awake()
    {
        EventManager = GameObject.Find("EventManager");
    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetMouseButton(0))
        {
            LoadingController.NextScene("RastBoss");
        }
        //ホワイトイン
        EventManager.GetComponent<EventController>().FadeIn(0.2f);
	}
}
