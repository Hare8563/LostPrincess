using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyCanvasHPScript : MonoBehaviour {

    /// <summary>
    /// 最大HP
    /// </summary>
    private float MaxHp = 0;
    /// <summary>
    /// 現在のHP
    /// </summary>
    private float NowHp = 0;
    /// <summary>
    /// 幅
    /// </summary>
    private float width;
    /// <summary>
    /// サブゲージの幅
    /// </summary>
    private float subWidth = 100;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("Max = " + MaxHp);
        //Debug.Log("Now = " + NowHp);
        width = (NowHp / MaxHp) * 100;
        foreach (Transform i in this.transform)
        {
            if (i.gameObject.name == "EnemyHP_Gauge")
            {
                i.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(width, 100);
            }
            else if (i.gameObject.name == "EnemyHP_Gauge_Sub")
            {
                Method.SmoothChange(ref subWidth, width, 0.7f);
                i.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(subWidth - 0.5f, 100);
            }
        }
	}

    /// <summary>
    /// HPの最大値
    /// </summary>
    public void setMaxHp(float maxHp)
    {
        MaxHp = maxHp;
    }

    /// <summary>
    /// 現在のHPを設定
    /// </summary>
    /// <param name="nowHp"></param>
    public void setNowHp(float nowHp)
    {
        NowHp = nowHp;
    }
}
