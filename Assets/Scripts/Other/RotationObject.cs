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
    private Quaternion rotation = new Quaternion();
	// Use this for initialization
	void Start () {
        rotation = this.transform.rotation;
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

    /// <summary>
    /// 回転速度を指定
    /// </summary>
    /// <param name="speed"></param>
    public void setSpeed(float speed)
    {
        Speed = speed;
    }

    /// <summary>
    /// 回転軸を指定("x" , "y" , "z")
    /// </summary>
    /// <param name="axis"></param>
    public void setAxis(string axis)
    {
        switch (axis)
        {
            case "x":
                RoteX = true;
                RoteY = false;
                RoteZ = false;
                break;
            case "y":
                RoteX = false;
                RoteY = true;
                RoteZ = false;
                break;
            case "z":
                RoteX = false;
                RoteY = false;
                RoteZ = true;
                break;
        }
    }
}
