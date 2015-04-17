using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyHPBarScript : MonoBehaviour {
    /// <summary>
    /// キャンバス
    /// </summary>
    private GameObject canvas;
    /// <summary>
    /// HPバーの背景
    /// </summary>
    private Image HPBar_Back;
    /// <summary>
    /// HPバー
    /// </summary>
    private Image HPBar;
    /// <summary>
    /// サブHPバー
    /// </summary>
    private Image subHPBar;
    /// <summary>
    /// 初期HP
    /// </summary>
    private float InitHp;
    /// <summary>
    /// 現在のHP
    /// </summary>
    private float NowHp;
    /// <summary>
    /// 幅
    /// </summary>
    private float width;
    /// <summary>
    /// サブHPバーの幅
    /// </summary>
    private float subWidth = 100;
    /// <summary>
    /// カメラビューポート座標
    /// </summary>
    private Vector3 CameraViewPos;
    /// <summary>
    /// 画像のアルファ値
    /// </summary>
    private float a = 0;
    /// <summary>
    /// 1フレーム前のHP
    /// </summary>
    private float oldHp;
    /// <summary>
    /// 今フレームのHP
    /// </summary>
    private float newHp;
    /// <summary>
    /// HPが変わっていない時間
    /// </summary>
    private float SameCount = 0;
    /// <summary>
    /// 初期制限
    /// </summary>
    private bool startFlag = false;

    void Awake()
    {
        
    }

	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas");
        this.transform.parent = canvas.transform;
        this.transform.localPosition = Vector3.zero;
        HPBar_Back = this.transform.FindDeep("EnemyHP_Frame").GetComponent<Image>();
        HPBar = this.transform.FindDeep("EnemyHP").GetComponent<Image>();
        subHPBar = this.transform.FindDeep("EnemyHP_Back").GetComponent<Image>();
        newHp = 0;
        oldHp = 0;
        a = 0;
        HPBar_Back.color = new Color(HPBar_Back.color.r, HPBar_Back.color.g, HPBar_Back.color.b, a);
        HPBar.color = new Color(HPBar.color.r, HPBar.color.g, HPBar.color.b, a);
        subHPBar.color = new Color(subHPBar.color.r, subHPBar.color.g, subHPBar.color.b, a);
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        newHp = NowHp;
        setAlpha();
        setColor();
        setSize();
        oldHp = NowHp;
	}

    /// <summary>
    /// 画面内表示
    /// </summary>
    private void setEnable()
    {
        //ビューポート内の敵の位置座標
        Vector3 WtoV = Camera.main.WorldToViewportPoint(this.transform.position + new Vector3(0, 15.0f, 0));
        //画面内だったら表示
        if (0.0f < WtoV.x && WtoV.x < 1.0f &&
            0.0f < WtoV.y && WtoV.y < 1.0f &&
            0.0f < WtoV.z)
        {
            this.enabled = true;
        }
        else
        {
            this.enabled = false;
        }
    }

    /// <summary>
    /// アルファ値の変更
    /// </summary>
    private void setAlpha()
    {
        if (startFlag)
        {
            //HPが変わっていたら
            if (newHp != oldHp)
            {
                //アルファ値を最大に
                a = 1;
                //カウントを初期化
                SameCount = 0;
            }
            //前フレームと同じだったら
            else
            {
                //その時間をカウント
                SameCount += Method.GameTime();
                //一定時間経つとゆっくりアルファ値を変更
                if (SameCount > 180)
                {
                    SameCount = 180;
                    if (a > 0) a -= 0.01f;
                }
            }
        }
        else
        {
            startFlag = true;
        }
    }

    /// <summary>
    /// サイズの反映
    /// </summary>
    private void setSize()
    {
        width = (NowHp / InitHp) * 100;
        //Debug.Log(width);
        HPBar.rectTransform.sizeDelta = new Vector2(width, 5);
        Method.SmoothChange(ref subWidth, width, 0.7f);
        subHPBar.rectTransform.sizeDelta = new Vector2(subWidth, 5);
    }

    /// <summary>
    /// 色の反映
    /// </summary>
    private void setColor()
    {
        HPBar_Back.color = new Color(HPBar_Back.color.r, HPBar_Back.color.g, HPBar_Back.color.b, a - 0.5f);
        HPBar.color = new Color(HPBar.color.r, HPBar.color.g, HPBar.color.b, a);
        subHPBar.color = new Color(subHPBar.color.r, subHPBar.color.g, subHPBar.color.b, a);
    }

    /// <summary>
    /// 初期HPを設定
    /// </summary>
    public void setMaxHp(int maxHp)
    {
        InitHp = maxHp;
    }

    /// <summary>
    /// HPのステータスを設定
    /// </summary>
    /// <param name="nowHp"></param>
    public void setNowHPStatus(int nowHp, Vector3 pos)
    {
        NowHp = nowHp;
        CameraViewPos = Camera.main.WorldToViewportPoint(pos + new Vector3(0, 5.0f, 0));
        //Debug.Log(CameraViewPos);
        this.transform.position = new Vector3(CameraViewPos.x * Screen.width, CameraViewPos.y * Screen.height, 0);
        //Debug.Log("Init=" + InitHp + "\nNow=" + NowHp);
        //即更新
        this.Update();
    }
}
