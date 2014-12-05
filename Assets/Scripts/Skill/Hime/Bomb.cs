using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

    /// <summary>
    /// 爆発エフェクト
    /// </summary>
    private GameObject ExplosionEffect;
    /// <summary>
    /// 爆発するまでの時間
    /// </summary>
    [SerializeField]
    private float ExploseFPSCount = 0;
    /// <summary>
    /// 現在の時間
    /// </summary>
    private float NowCount = 0;
     
    void Awake()
    {
        ExplosionEffect = Resources.Load("Prefab/Explosion") as GameObject;
        float random = 70.0f;
        this.rigidbody.AddForce(new Vector3(Random.Range(-random, random), 0, Random.Range(-random, random)), ForceMode.Impulse);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        NowCount += Method.GameTime();
        if (NowCount >= ExploseFPSCount)
        {
            Instantiate(ExplosionEffect, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }

	}
}
