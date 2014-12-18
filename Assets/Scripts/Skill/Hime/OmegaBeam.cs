using UnityEngine;
using System.Collections;

public class OmegaBeam : MonoBehaviour {

    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    private GameObject PlayerObject;

    void Awake()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
    }

	// Use this for initialization
	void Start () 
    {
        Destroy(this.gameObject, 10);
	}
	
	// Update is called once per frame
	void Update () 
    {
        //プレイヤーの方向を向く
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(PlayerObject.transform.position - this.transform.transform.position), 0.035f);
        this.transform.rotation = new Quaternion(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z, this.transform.rotation.w);
	}

    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Debug.Log("Beam Hit");
        }
    }
}
