using UnityEngine;
using System.Collections;

public class SensingScript : MonoBehaviour {

    /// <summary>
    /// 感知するオブジェクトのタグの列挙体
    /// </summary>
    public enum SensingTag
    {
        Player,
        Enemy,
        Boss,
        Hime,
    }
    /// <summary>
    /// 感知するオブジェクトのタグ
    /// </summary>
    [SerializeField]
    private SensingTag sensingTarget;
    /// <summary>
    /// 感知したかどうか
    /// </summary>
    private bool isSensing = false;
    /// <summary>
    /// 感知範囲
    /// </summary>
    [SerializeField]
    private float SensingRange = 0;

	// Use this for initialization
	void Start () {
        if (SensingRange < 1) this.GetComponent<SphereCollider>().radius = SensingRange;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// 感知したかどうかを取得
    /// </summary>
    /// <returns></returns>
    public bool getIsSensing()
    {
        return isSensing;
    }

    /// <summary>
    /// 何かに触れたら
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.tag);
        if (other.tag == sensingTarget.ToString())
        {
            isSensing = true;
        }
    }

    /// <summary>
    /// 何かが離れたら
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerExit(Collider other)
    {
        if (other.tag == sensingTarget.ToString())
        {
            isSensing = false;
        }
    }
}
