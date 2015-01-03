using UnityEngine;
using System.Collections;

public class BigMeteo : MonoBehaviour {

	/// <summary>
	/// 移動スピード
	/// </summary>
    [SerializeField]
    [Range(0, 5)]
    private float Speed = 0;
    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    private GameObject PlayerObject;
    /// <summary>
    /// 敵オブジェクト
    /// </summary>
    private GameObject EnemyObject;
    /// <summary>
    /// 反射させられたか
    /// </summary>
    private bool isReflect = false;

    void Awake()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        EnemyObject = GameObject.FindGameObjectWithTag("Hime");
    }

	// Use this for initialization
	void Start () {
		Destroy(this.gameObject, 5.0f);
		this.transform.LookAt(PlayerObject.transform.position);
        isReflect = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        
	}

	void FixedUpdate()
	{
		this.transform.Translate(Vector3.forward * Speed * Method.GameTime());
	}

	void OnTriggerEnter(Collider collider)
	{
		//Debug.Log (collider.tag);
        if (collider.tag == "Weapon_Sword")
        {
            isReflect = true;
            this.transform.LookAt(EnemyObject.transform.position);
        }
        else if (collider.tag == "Player" && !isReflect)
        {
			PlayerObject.GetComponent<PlayerController>().Damage(5);
			Destroy(this.gameObject);
        }
        else if (collider.tag == "Hime" && isReflect)
        {
            EnemyObject.GetComponent<RastBossController>().Damage(5);
            Destroy(this.gameObject);
        }
	}
}
