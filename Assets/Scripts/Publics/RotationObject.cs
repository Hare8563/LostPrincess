using UnityEngine;
using System.Collections;

/// <summary>
/// ただ単にオブジェクトを回転させたいときに使う
/// </summary>
public class RotationObject : MonoBehaviour {

    public bool RoteX = false;
    public bool RoteY = false;
    public bool RoteZ = false;
    public float Speed = 0;
    private float rote = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        rote += Speed * Time.deltaTime;
        if (rote > 360 || rote < -360) rote = 0;
        if (RoteX)
        {
            this.transform.rotation = Quaternion.Euler(rote, this.transform.rotation.y, this.transform.rotation.z);
        }
        else if (RoteY)
        {
            this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, rote, this.transform.rotation.z);
        }
        else if (RoteZ)
        {
            this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y,rote);
        }
	}
}
