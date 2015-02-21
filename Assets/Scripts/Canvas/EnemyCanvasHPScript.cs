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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("Max = " + MaxHp);
        //Debug.Log("Now = " + NowHp);
        foreach (Transform i in this.transform)
        {
            if (i.gameObject.name == "EnemyHP_Gauge")
            {
                Debug.Log((NowHp / MaxHp) * 100);
                i.GetComponent<Image>().rectTransform.sizeDelta = new Vector2((NowHp / MaxHp) * 100, 100);
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

    public void setNowHp(float nowHp)
    {
        NowHp = nowHp;
    }
}
