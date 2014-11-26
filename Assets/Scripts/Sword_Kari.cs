using UnityEngine;
using System.Collections;

public class Sword_Kari : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        float Distance = 3.0f;
        float x = this.transform.rotation.x;
        //一定距離近づいたら攻撃
        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, this.transform.position) <= Distance)
        {
            Method.SmoothChange(ref x, -45*Mathf.PI/180, 0.5f);
            this.transform.rotation = new Quaternion(x, this.transform.rotation.y, this.transform.rotation.z, this.transform.rotation.w);
        }
	}
}
