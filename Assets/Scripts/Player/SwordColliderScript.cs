using UnityEngine;
using System.Collections;

public class SwordColliderScript : MonoBehaviour {
    /// <summary>
    /// プレイヤーコントローラクラス
    /// </summary>
    private PlayerController playerController;
    /// <summary>
    /// 敵ステータスクラス
    /// </summary>
    private EnemyStatusManager enemyStatusManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        //プレイヤーに対して
        if (other.tag == "Player")
        {
            enemyStatusManager = this.transform.parent.gameObject.GetComponent<EnemyStatusManager>();
            other.GetComponent<PlayerController>().Damage(enemyStatusManager.getStatus().Sword_Power);
        }
        //敵に対して
        else if (other.tag == "Enemy" ||
                 other.tag == "Boss" ||
                 other.tag == "Hime")
        {
            playerController = this.transform.parent.gameObject.GetComponent<PlayerController>();
            other.GetComponent<EnemyStatusManager>().Damage(playerController.getStatus().Sword_Power);
            //Debug.Log(playerController.getStatus().Sword_Power);
        }
    }
}
