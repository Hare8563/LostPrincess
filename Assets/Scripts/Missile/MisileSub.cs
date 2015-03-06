using UnityEngine;
using System.Collections;

public class MisileSub : MonoBehaviour {
    /// <summary>
    /// 1秒間に進む距離
    /// </summary>
    public float _speed = 100.0f;
    /// <summary>
    /// 1秒間に回転する角度
    /// </summary>
    public float _rotSpeed = 100.0f;
    /// <summary>
    /// 初期角度
    /// </summary>
    private int InitVec;
    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    private GameObject PlayerObject;

    /// <summary>
    /// 爆発プレハブ
    /// </summary>
    private GameObject ExprosionPrehub;

	// Use this for initialization
	void Start () 
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
		Destroy(this.gameObject, 5f);
    }
	
	// Update is called once per frame
	void Update () 
    {
        // ターゲットまでの角度を取得
        Vector3 vecTarget = PlayerObject.transform.position + new Vector3(0, 4.0f, 0) - this.transform.position; // ターゲットへのベクトル
        Vector3 vecForward = transform.TransformDirection(Vector3.forward);   // 弾の正面ベクトル
        float angleDiff = Vector3.Angle(vecForward, vecTarget);            // ターゲットまでの角度
        float angleAdd = (_rotSpeed * Time.deltaTime);                    // 回転角
        Quaternion rotTarget = Quaternion.LookRotation(vecTarget);
        // ターゲットが回転角の外なら、指定角度だけターゲットに向ける
        float t = (angleAdd / angleDiff);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotTarget, t);
        // 前進
        transform.position += transform.TransformDirection(Vector3.forward) * _speed * Time.deltaTime;
    }

	/// <summary>
	/// 何かに当たったら
	/// </summary>
	/// <param name="collision"></param>
	void OnTriggerEnter(Collider collider)
	{
		//Debug.Log(collision.collider.name);
		if (collider.tag == "Player")
		{
			//Debug.Log("bullet damage");
			collider.gameObject.GetComponent<PlayerController>().Damage(1);
		}
		Destroy(this.gameObject);
	}
}
