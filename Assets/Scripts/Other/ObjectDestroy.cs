using UnityEngine;
using System.Collections;

/// <summary>
/// ただ単にオブジェクトを破棄したいときに使う
/// </summary>
public class ObjectDestroy : MonoBehaviour {
    public float time = 0.0f;

	// Use this for initialization
	void Start () 
    {
        Destroy(this.gameObject, time);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
