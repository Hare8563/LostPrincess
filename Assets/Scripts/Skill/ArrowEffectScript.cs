using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArrowEffectScript : MonoBehaviour {

    /// <summary>
    /// チャージエフェクト配列
    /// </summary>
    private List<ParticleSystem> ChargeEffects = new List<ParticleSystem>();
    /// <summary>
    /// ショットエフェクト配列
    /// </summary>
    private List<ParticleSystem> ShotEffects = new List<ParticleSystem>();
    /// <summary>
    /// 指定するエフェクトの色
    /// </summary>
    private Color effectColor = Color.white;
    /// <summary>
    /// チャージエフェクトの色
    /// </summary>
    private Color ChargeColor = Color.white;
    /// <summary>
    /// ショットエフェクトの色
    /// </summary>
    private Color ShotColor = Color.white;
    /// <summary>
    /// チャージ段階ごとの色
    /// </summary>
    [SerializeField]
    private Color[] colors;

    void Awake()
    {
        //チャージエフェクト
        for (int i = 0; i < 3; i++)
        {
            ChargeEffects.Add(this.transform.Find("Charge_Convergence_" + i).gameObject.GetComponent<ParticleSystem>());
        }
        //発射エフェクト
        for (int i = 0; i < 4; i++)
        {
            ShotEffects.Add(this.transform.Find("Charge_Lv" + i).gameObject.GetComponent<ParticleSystem>());
        }
    }

	// Use this for initialization
    void Start()
    {
        //チャージエフェクト生成開始
        for (int i = 0; i < ChargeEffects.Count; i++)
        {
            ChargeEffects[i].enableEmission = true;
        }
        //ショットエフェクト生成停止
        for (int i = 0; i < ShotEffects.Count; i++)
        {
            ShotEffects[i].enableEmission = false;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        float speed = 0.02f;
        Method.SmoothChange(ref ChargeColor.r, effectColor.r, speed);
        Method.SmoothChange(ref ChargeColor.g, effectColor.g, speed);
        Method.SmoothChange(ref ChargeColor.b, effectColor.b, speed);
        Method.SmoothChange(ref ChargeColor.a, effectColor.a, speed);
        Method.SmoothChange(ref ShotColor.r, effectColor.r, speed);
        Method.SmoothChange(ref ShotColor.g, effectColor.g, speed);
        Method.SmoothChange(ref ShotColor.b, effectColor.b, speed);
        Method.SmoothChange(ref ShotColor.a, effectColor.a, speed);
        //チャージエフェクト色反映
        for (int i = 0; i < ChargeEffects.Count; i++)
        {
            ChargeEffects[i].startColor = ChargeColor;
        }
        //ショットエフェクト色反映
        for (int i = 0; i < ShotEffects.Count; i++)
        {
            ShotEffects[i].startColor = ShotColor;
        }
	}

    /// <summary>
    /// チャージエフェクトの生成を行うか設定
    /// </summary>
    /// <param name="value"></param>
    public void setChargeEffectEmit(bool value)
    {
        for (int i = 0; i < ChargeEffects.Count; i++)
        {
            ChargeEffects[i].enableEmission = value;
        }
    }

    /// <summary>
    /// ショットエフェクトの生成を行うか設定
    /// </summary>
    /// <param name="value"></param>
    public void setShotEffectEmit(bool value)
    {
        //ショットエフェクト生成停止
        for (int i = 0; i < ShotEffects.Count; i++)
        {
            ShotEffects[i].enableEmission = value;
        }
    }

    /// <summary>
    /// 色番号を指定
    /// </summary>
    /// <param name="num"></param>
    public void setColorNumber(int num)
    {
        if (num < 3)
        {
            ChargeEffects[0].startSize = (num + 1) * 1.5f;
            effectColor = colors[num];
        }
    }
}
