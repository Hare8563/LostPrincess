using UnityEngine;
using System.Collections;

public class SlipDamageScript : MonoBehaviour {
    /// <summary>
    /// ダメージを負うまでの時間
    /// </summary>
    private float DamageCount = 0;
    /// <summary>
    /// ダメージを負う間隔
    /// </summary>
    [SerializeField]
    private int DamageTimingFlame;
    /// <summary>
    /// ダメージ量
    /// </summary>
    [SerializeField]
    private int DamageValue;

    private PlayerController playerController;
    private EnemyStatusManager enemyStatus;

	// Use this for initialization
	void Start () 
    {
        if (this.transform.parent.tag == "Player")
        {
            playerController = this.transform.parent.GetComponent<PlayerController>();
        }
        Destroy(this.gameObject, this.GetComponent<ParticleSystem>().duration + 1);
	}
	
	// Update is called once per frame
	void Update () 
    {
        this.transform.localPosition = Vector3.zero;
        DamageCount += Method.GameTime();
        if ((int)DamageCount % DamageTimingFlame == 0)
        {
            DamageCount = 0;
            playerController.Damage(DamageValue);
        }
	}
}
