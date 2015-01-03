using UnityEngine;
using System.Collections;

public class OmegaBeam : MonoBehaviour {

    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    private GameObject PlayerObject;
	/// <summary>
	/// ダメージを受けるタイミング
	/// </summary>
	private int DamageTiming = 0;
    /// <summary>
    /// スキルが終了したか
    /// </summary>
    private bool isEnd = false;
    /// <summary>
    /// 経過時間
    /// </summary>
    private float secondTime = 0;

    void Awake()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
    }

	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        //プレイヤーの方向を向く
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(PlayerObject.transform.position - this.transform.transform.position), 0.035f);
        //this.transform.rotation = new Quaternion(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z, this.transform.rotation.w);
        secondTime += Method.GameTime();
        if (secondTime > 600)
        {
            secondTime = 0;
            isEnd = true;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player")
        {
            //Debug.Log("Beam Hit");
			DamageTiming++;
			if(DamageTiming % 10 == 0)
			{
				DamageTiming = 0;
				PlayerObject.GetComponent<PlayerController>().Damage(5);
			}
        }
    }

    /// <summary>
    /// オメガビームスキルが終了したかどうかを返す
    /// </summary>
    /// <returns></returns>
    public bool getEndOmegaBeam()
    {
        return isEnd;
    }
}
