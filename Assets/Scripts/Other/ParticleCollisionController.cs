using UnityEngine;
using System.Collections;

/// <summary>
/// パーティクルシステムにコライダを付与した際に使用する
/// </summary>
public class ParticleCollisionController : MonoBehaviour {

    /// <summary>
    /// 雨エフェクト
    /// </summary>
    private GameObject RainRippleEffect;

    void Awake()
    {
        //RainRippleEffect = Resources.Load("Prefab/RainRipple") as GameObject;
    }

	// Use this for initialization
	void Start () {
        //Debug.Log("Start");
	}

    /// <summary>
    /// パーティクルが衝突したら
    /// </summary>
    /// <param name="obj">衝突したオブジェクト</param>
    void OnParticleCollision(GameObject obj)
    {
        //Instantiate(RainRippleEffect, )
        Debug.Log("Collid");
    }
}
