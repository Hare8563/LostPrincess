using UnityEngine;
using System.Collections;

public class GUITextureResize : MonoBehaviour {
    /// <summary>
    /// 画面サイズ
    /// </summary>
    private Vector2 size;
    /// <summary>
    /// 自身GUIテクスチャ
    /// </summary>
    private GUITexture texture;
    /// <summary>
    /// テクスチャのサイズ
    /// </summary>
    private Vector2 textureSize;
    /// <summary>
    /// テクスチャの位置
    /// </summary>
    public Rect texturePos;
    /// <summary>
    /// テクスチャの初期サイズ
    /// </summary>
    private Vector2 textureInitSize;
    /// <summary>
    /// テクスチャの初期位置
    /// </summary>
    public Rect textureInitPos;
    /// <summary>
    /// プレイ中ずっとリサイズするか
    /// </summary>
    public bool debagFlag;
    /// <summary>
    /// 画面縦幅を基準にリサイズ(初期設定)
    /// </summary>
    public bool UpperdownResize = false;
    /// <summary>
    /// 画面横幅を基準にリサイズ
    /// </summary>
    public bool RightleftResize = false;
    /// <summary>
    /// 画面縦幅だけに合わせてリサイズ
    /// </summary>
    public bool UpperdownFillResize = false;
    /// <summary>
    /// 画面横幅だけに合わせてリサイズ
    /// </summary>
    public bool RightleftFillResize = false;
    /// <summary>
    /// 画面全体に広がるようにリサイズ
    /// </summary>
    public bool FullWindowResize = false;
    /// <summary>
    /// ウィンドウを分割する数。これでリサイズ基準を決める(初期値:4)
    /// </summary>
    public float SpritNum = 4;
    /// <summary>
    /// 分割した画面のサイズ
    /// </summary>
    private float spritWindowSize;
    /// <summary>
    /// 回転するGUIの角度
    /// </summary>
    private float GUIAngle;
    /// <summary>
    /// GUITextureの中心
    /// </summary>
    private Vector2 GUICenter;

	void Start () {
        GUIAngle = 0;
        size = new Vector2(Screen.width, Screen.height);
        texture = this.guiTexture;
        textureSize = textureInitSize = new Vector2(texture.guiTexture.pixelInset.width, texture.guiTexture.pixelInset.height);
        texturePos = textureInitPos = texture.pixelInset;
        //全てにチェックがされていなかったら、縦基準リサイズを適用
        if (!UpperdownResize && 
            !RightleftResize &&
            !UpperdownFillResize &&
            !RightleftFillResize &&
            !FullWindowResize)
        {
            UpperdownResize = true;
        }
        //縦基準リサイズ
        if (UpperdownResize)
        {
            spritWindowSize = size.y / SpritNum;
            //分割した画面の縦幅とテクスチャの縦幅との差
            float dis = spritWindowSize - textureSize.y;
            //分割した画面の縦幅にテクスチャの縦幅をあわせる
            float x = textureSize.x + dis;
            float y = textureSize.y + dis;
            this.texture.pixelInset = new Rect(- (x / 2), - (y / 2), x, y);
        }
        //横基準リサイズ
        else if (RightleftResize)
        {
            spritWindowSize = size.x / SpritNum;
            //分割した画面の横幅とテクスチャの横幅との差
            float dis = spritWindowSize - textureSize.x;
            //分割した画面の横幅にテクスチャの横幅をあわせる
            float x = textureSize.x + dis;
            float y = textureSize.y + dis;
            this.texture.pixelInset = new Rect(-(x / 2), -(y / 2), x, y);
        }
        //縦合わせリサイズ
        else if (UpperdownFillResize)
        {
            texture.pixelInset = new Rect(texturePos.x, -(Screen.height / 2), textureSize.x, Screen.height);
        }
        //横合わせリサイズ
        else if (RightleftFillResize)
        {
            texture.pixelInset = new Rect(-(Screen.width / 2), texturePos.y, Screen.width, textureSize.y);
        }
        //フルウィンドウリサイズ
        else if (FullWindowResize)
        {
            texture.pixelInset = new Rect(-((float)Screen.width / 2), -((float)Screen.height / 2), Screen.width, Screen.height);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (debagFlag)
        {
            size = new Vector2(Screen.width, Screen.height);
            texture = this.guiTexture;
            textureSize = new Vector2(texture.guiTexture.pixelInset.width, texture.guiTexture.pixelInset.height);
            //縦基準リサイズ
            if (UpperdownResize)
            {
                spritWindowSize = size.y / SpritNum;
                //分割した画面の横幅とテクスチャの横幅との差
                float dis = spritWindowSize - textureSize.y;
                //差を加算
                texture.pixelInset = new Rect(texturePos.x, texturePos.y, textureSize.x + dis, textureSize.y + dis);
            }
            //横基準リサイズ
            else if (RightleftResize)
            {
                spritWindowSize = size.x / SpritNum;
                //分割した画面の横幅とテクスチャの横幅との差
                float dis = spritWindowSize - textureSize.x;
                //差を加算
                texture.pixelInset = new Rect(texturePos.x, texturePos.y, textureSize.x + dis, textureSize.y + dis);
            }
            //縦合わせリサイズ
            else if (RightleftFillResize)
            {
                texture.pixelInset = new Rect(texturePos.x, -(Screen.height / 2), textureSize.x, Screen.height);
            }
            //横合わせリサイズ
            else if (RightleftFillResize)
            {
                texture.pixelInset = new Rect(-(Screen.width / 2), texturePos.y, Screen.width, textureSize.y);
            }

            //フルウィンドウリサイズ
            else if (FullWindowResize)
            {
                texture.pixelInset = new Rect(-((float)Screen.width / 2), -((float)Screen.height / 2), Screen.width, Screen.height);
            }
        }
    }
}
