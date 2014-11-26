using UnityEngine;
using System.Collections;

public class CameraSlide : MonoBehaviour {
    /// <summary>
    /// 最終目的Z座標
    /// </summary>
    [SerializeField]
    [Range(0,1000)]
    private float MaxZ = 0;
    /// <summary>
    /// カメラの移動速度
    /// </summary>
    [SerializeField]
    [Range(0,5)]
    private float Speed = 0;
    /// <summary>
    /// 現在のZ座標
    /// </summary>
    private float nowZ = 0;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
        nowZ += Speed * Method.GameTime();
        if (nowZ >= MaxZ)
        {
            nowZ = 0;
            isEnd = true;
        }
        else
        {
            isEnd = false;
        }
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, nowZ);
	}

    /// <summary>
    /// 最後まで来たフラグ
    /// </summary>
    public static bool isEnd { set; get; }
}
