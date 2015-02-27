using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour {

	/// <summary>
	/// 最大サイズ
	/// </summary>
	[SerializeField]
	[Range(0,50)]
	private float maxScale = 0;
	/// <summary>
	/// 現在のサイズ
	/// </summary>
	private float nowScale = 0;
	/// <summary>
	/// 拡大する時間
	/// </summary>
	[SerializeField]
	[Range(0,1)]
	private float ScaleSpeed = 0;

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, 3.0f);
	}
	
	// Update is called once per frame
	void Update () {
		//最初は小さく、徐々に大きくする
		if(nowScale < maxScale)
		{
			nowScale += ScaleSpeed;
		}
		this.transform.localScale = new Vector3(nowScale, nowScale, nowScale);
        this.transform.position = SetPosition;
	}

    /// <summary>
    /// 座標指定
    /// </summary>
    public Vector3 SetPosition { set; private get; }
}
