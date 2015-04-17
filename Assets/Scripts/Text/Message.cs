using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Message : MonoBehaviour {
    /// <summary>
    /// メッセージ送りのカウント
    /// </summary>
    private float MessageCount = 0;
    /// <summary>
    /// 古い文章
    /// </summary>
    private string oldText;
    /// <summary>
    /// メッセージが一文字流れるごとに流すSE
    /// </summary>
    public AudioClip MessageSe;
    /// <summary>
    /// キャンバスオブジェクト
    /// </summary>
    private GameObject canvas;
    /// <summary>
    /// テキスト
    /// </summary>
    private Text message;
    /// <summary>
    /// 文字送りの速さ
    /// </summary>
    [SerializeField]
    [Range(0.1f,1)]
    private float SendSpeed;

    void Awake()
    {
        //messageBox = GameObject.Find("MessageBox");
        canvas = GameObject.Find("Canvas");
        message = canvas.transform.Find("Text").gameObject.GetComponent<Text>();
    }

    // Use this for initialization
	void Start () {
        Time.timeScale = 1;
        message.text = "";
        getIsReadingFlag = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        SendMessage();
        //Debug.Log(setgetMessage);
    }

    /// <summary>
    /// メッセージ送り
    /// </summary>
    void SendMessage()
    {
        //メッセージ内容が空でなければ
        if (setgetMessage != null && setgetMessage != "")
        {
            //表示していたものと異なる文が来たら、
            //メッセージ送りカウンタを初期化
            if (oldText != setgetMessage)
            {
                MessageCount = 0;
                //Debug.Log("Init");
            }
            getIsReadingFlag = true;
            //messageBox.guiTexture.enabled = true;
            //メッセージ送りのカウントが文字列よりも小さかったら
            if (MessageCount < setgetMessage.Length)
            {
                //メッセージを一文字づつ表示
                MessageCount += SendSpeed * Method.GameTime();
                if (MessageSe != null) GetComponent<AudioSource>().PlayOneShot(MessageSe, 0.1f);
                //カウンタが文字列以上にならないよう調整
                if (MessageCount > setgetMessage.Length)
                {
                    MessageCount = setgetMessage.Length;
                }
                //this.transform.guiText.text = setgetMessage.Substring(0, (int)MessageCount);
                //Debug.Log(setgetMessage + " , " + this.transform.guiText.text);
            }
            //Debug.Log(oldText + " , " +setgetMessage);
            //Debug.Log(setgetMessage);
            message.text = setgetMessage.Substring(0, (int)MessageCount);
            oldText = setgetMessage;
        }
        //メッセージが空だったら初期化
        else
        {
            MessageCount = 0;
            getIsReadingFlag = false;
            //messageBox.guiTexture.enabled = false;
            message.text = "";
        }

        //メッセージを読み終えたら
        if (!getIsReadingFlag)
        {
            MessageCount = 0;
            getIsReadingFlag = false;
            //messageBox.guiTexture.enabled = false;
            //setgetMessage = "";
            message.text = "";
        }
        //Debug.Log(setgetMessage + " , " + this.transform.guiText.text);
        //Debug.Log(this.transform.guiText.text);
        //Debug.Log(messageBox.guiTexture.enabled);
    }

    /// <summary>
    /// メッセージテキストを指定するプロパティ
    /// </summary>
    public static string setgetMessage { set; get; }

    /// <summary>
    /// メッセージを読んでいるフラグを取得するプロパティ
    /// </summary>
    public static bool getIsReadingFlag { private set; get; }
}
