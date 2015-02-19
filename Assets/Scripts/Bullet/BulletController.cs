using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {
    /// <summary>
    /// 弾の速さ
    /// </summary>
    private float Speed;
    /// <summary>
    /// ショットエフェクト
    /// </summary>
    private GameObject ShotEffect;

    void Awake()
    {
        ShotEffect = Resources.Load("Prefab/ShotEffect") as GameObject;
    }

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, 2.0f);
        Instantiate(ShotEffect, this.transform.position, this.transform.rotation);
	}
	
	// Update is called once per frame
	void Update () 
    {
        BillBoad();
        this.transform.Translate(Vector3.forward * Speed * Method.GameTime());
	}

    /// <summary>
    /// ビルボード
    /// </summary>
    void BillBoad()
    {
        Vector3 init = this.transform.localEulerAngles;
        this.transform.LookAt(Camera.main.transform.position);
        Vector3 look = this.transform.localEulerAngles;
        this.transform.localEulerAngles = new Vector3(init.x,init.y,look.z);
        
    }

    /// <summary>
    /// 弾の速さを設定
    /// </summary>
    public void setBulletSpeed(float speed)
    {
        Speed = speed;
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
