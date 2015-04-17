using UnityEngine;
using System.Collections;

public class StaffRoll : MonoBehaviour {

	// Use this for initialization
	void Start () {
        iTween.MoveTo(gameObject, iTween.Hash(
            "path", iTweenPath.GetPath("Path_01"),
            "time", 70f,
            "oncomplete", "toTitle",
            "easetype", iTween.EaseType.linear));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
