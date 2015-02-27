using UnityEngine;
using System.Collections;

public class EventController : MonoBehaviour
{
    /// <summary>
    /// 白GUIテクスチャ
    /// </summary>
    private GUITexture WhiteGuiTexture;
    /// <summary>
    /// アルファ値
    /// </summary>
    private float a = 0;
    /// <summary>
    /// アルファ値が変更されたかどうか
    /// </summary>
    private bool isAlfa;
    /// <summary>
    /// 画面GUITexture
    /// </summary>
    private GameObject[] GuiTextures;
    /// <summary>
    /// 画面GUItext
    /// </summary>
    private GameObject[] GuiTexts;

    void Awake()
    {
        WhiteGuiTexture = this.transform.FindChild("WhiteTex").guiTexture;
    }

    // Use this for initialization
    void Start()
    {
        WhiteGuiTexture.color = new Color(1, 1, 1, a);
        isAlfa = false;
    }

    // Update is called once per frame
    void Update()
    {
        WhiteGuiTexture.color = new Color(1, 1, 1, a);
        //Debug.Log(a);
    }

    /// <summary>
    /// ホワイトアウト
    /// </summary>
    /// <param name="scenename">ホワイトアウトした後に遷移するシーン</param>
    /// /// <param name="speed">ホワイトアウトするはやさ</param>
    public void FadeOut(string scenename, float speed)
    {
        //WhiteGuiTexture.color = color;
        //初期化
        if (!isAlfa)
        {
            isAlfa = true;
            a = 0f;
        }
        //アルファ値を加算
        a += (speed * Method.GameTime()) / 255;
        if (a >= 0.5f)
        {
            a = 0.5f;
            Application.LoadLevel(scenename);
        }
    }

    /// <summary>
    /// ホワイトイン
    /// </summary>
    /// <param name="speed">ホワイトインするはやさ</param>
    public void FadeIn(float speed)
    {
        //WhiteGuiTexture.color = color;
        //初期化
        if (!isAlfa)
        {
            isAlfa = true;
            a = 0.5f;
        }
        //アルファ値を減算
        if (a > 0.0f)
        {
            a -= (speed * Method.GameTime()) / 255;
            if (a < 0) a = 0;
        }
    }
}
