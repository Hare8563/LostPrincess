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
    /// <summary>
    /// ターゲットに対する回転
    /// </summary>
    private Vector3 toTargetVec;
    /// <summary>
    /// ターゲットオブジェクト
    /// </summary>
    private static GameObject TargetObject;
    /// <summary>
    /// 敵
    /// </summary>
    [SerializeField]
    private GameObject[] Enemys;
     
    void Awake()
    {
        TargetObject = GameObject.FindGameObjectWithTag("Player");
        ExplosionEffect = Resources.Load("Prefab/Explosion") as GameObject;
        float random = 20.0f;
        this.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-random, random), 0, Random.Range(-random, random)), ForceMode.Impulse);
    }

	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        float dis = Vector3.Distance(TargetObject.transform.position, this.transform.position);
        toTargetVec = TargetObject.transform.position - this.transform.position;
        this.GetComponent<Rigidbody>().AddForce(toTargetVec * 0.02f, ForceMode.Impulse);
        NowCount += Method.GameTime();
        if (NowCount >= ExploseFPSCount)
        {
            if (dis < 15)
            {
                TargetObject.GetComponent<PlayerController>().Damage(Random.Range(3, 7));
            }
            int rand = Random.Range(0,3);
            Instantiate(ExplosionEffect, this.transform.position, this.transform.rotation);
            Instantiate(Enemys[rand], this.transform.position, Enemys[rand].transform.rotation);
            Destroy(this.gameObject);
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            int rand = Random.Range(0, 3);
            Instantiate(ExplosionEffect, this.transform.position, this.transform.rotation);
            TargetObject.GetComponent<PlayerController>().Damage(Random.Range(3, 7));
            Instantiate(Enemys[rand], this.transform.position, Enemys[rand].transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
