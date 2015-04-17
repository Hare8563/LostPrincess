using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RejectScript : MonoBehaviour {

    /// <summary>
    /// エフェクトを持つ子オブジェクトのリスト配列
    /// </summary>
    private List<ParticleSystem> childs = new List<ParticleSystem>();
    /// <summary>
    /// 回復中かどうか
    /// </summary>
    private bool isReject = false;
    /// <summary>
    /// 回復が終了したかどうか
    /// </summary>
    private bool isEnd = false;
    /// <summary>
    /// プレイヤーコントローラクラス
    /// </summary>
    private PlayerController playerController;
    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    private GameObject PlayerObject;
    /// <summary>
    /// 回復するタイミング
    /// </summary>
    private float RejectTimingCount = 0;

    void Awake()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        playerController = PlayerObject.GetComponent<PlayerController>();
    }

	// Use this for initialization
	void Start () {
        //エフェクトを持つ子オブジェクトを取得
        foreach (Transform child in this.transform)
        {
            childs.Add(child.gameObject.GetComponent<ParticleSystem>());
        }
        //最初はエフェクト生成を行わない
        foreach (ParticleSystem particleSystem in childs)
        {
            particleSystem.enableEmission = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        //キーが押されていたらエフェクト生成
        if (Input.GetKey(KeyCode.Space) && !isEnd)
        {
            foreach (ParticleSystem particleSystem in childs)
            {
                particleSystem.enableEmission = true;
                this.transform.position = PlayerObject.transform.position + new Vector3(0, 0.1f, 0);
                isReject = true;
                RejectMP();
            }
        }
        //押されていなかったら停止
        else
        {
            int endCount = 0;
            isEnd = true;
            foreach (ParticleSystem particleSystem in childs)
            {
                particleSystem.enableEmission = false;
                isReject = false;
                //生成終了したエフェクトの数をカウント
                if(particleSystem.IsAlive())
                {
                    endCount++;
                }
                //全エフェクト生成が終了したら自身を削除
                if (endCount == childs.Count)
                {
                    Destroy(this.gameObject, 2.0f);
                }
            }
        }
	}

    /// <summary>
    /// プレイヤーのMPを回復
    /// </summary>
    private void RejectMP()
    {
        //playerController.getStatus().MP++;
        RejectTimingCount++;//Method.GameTime();
        if ((int)RejectTimingCount % 10 == 0)
        {
            RejectTimingCount = 0;
            playerController.getStatus().MP++;
        }
    }
}
