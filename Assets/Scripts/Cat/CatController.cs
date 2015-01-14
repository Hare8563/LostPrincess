using UnityEngine;
using System.Collections;

public class CatController : MonoBehaviour {

    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    private GameObject PlayerObject;
    /// <summary>
    /// 速さ
    /// </summary>
    [SerializeField]
    [Range(0,10)]
    private float Speed;

    void Awake()
    {
        PlayerObject = GameObject.FindWithTag("Player");
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        //プレイヤー方向を向く
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(PlayerObject.transform.position - transform.transform.position), 0.07f);
        this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);
        if (this.GetComponent<EnemyStatusManager>().getIsDead())
        {
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        Vector3 forward = this.transform.TransformDirection(Vector3.forward).normalized;
        rigidbody.AddForce(forward * Speed, ForceMode.VelocityChange);
    }
}
