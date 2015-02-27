using UnityEngine;
using System.Collections;

public class BigMine : MonoBehaviour {

    /// <summary>
    /// 子オブジェクト
    /// </summary>
    private GameObject[] ChildObjects;
    /// <summary>
    /// 最大サイズ
    /// </summary>
    private float MaxSize = 7;
    /// <summary>
    /// 現在のサイズ
    /// </summary>
    private float nowSize = 0;
    /// <summary>
    /// 縮小するフラグ
    /// </summary>
    private bool SmallingFlag = false;
	/// <summary>
	/// プレイヤーがふれている時間
	/// </summary>
	private float TatchCount = 0;

	void Awake()
	{

	}

	// Use this for initialization
	void Start () {
	    foreach (Transform child in this.transform)
        {
            if (child.name == "Particle")
            {
                child.gameObject.particleSystem.startSize = 0;
            }
            else if (child.name == "Ball")
            {
                child.transform.localScale = Vector3.zero;
            }
            else if (child.name == "Point light")
            {
                child.light.intensity = 0;
            }
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!SmallingFlag)
        {
            nowSize += 0.01f * Method.GameTime();
            if (nowSize > MaxSize)
            {
                SmallingFlag = true;
            }
            foreach (Transform child in this.transform)
            {
                if (child.name == "Particle")
                {
                    child.gameObject.particleSystem.startSize = nowSize;
                }
                else if (child.name == "Ball")
                {
                    child.transform.localScale = new Vector3(nowSize, nowSize, nowSize);
                }
                else if (child.name == "Point light")
                {
                    child.light.intensity = nowSize * 8;
                }
            }
        }
        else
        {
            nowSize -= 0.1f * Method.GameTime();
            if (nowSize < 0)
            {
                Destroy(this.gameObject);
            }
            foreach (Transform child in this.transform)
            {
                if (child.name == "Particle")
                {
                    child.gameObject.particleSystem.startSize = nowSize;
                }
                else if (child.name == "Ball")
                {
                    child.transform.localScale = new Vector3(nowSize, nowSize, nowSize);
                }
                else if (child.name == "Point light")
                {
                    child.light.intensity = nowSize * 8;
                }
            }
        }
	}
}
