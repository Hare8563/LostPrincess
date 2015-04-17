using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TalkController : MonoBehaviour
{
    /// <summary>
    /// 自動にするかどうか
    /// </summary>
    [SerializeField]
    private bool isAuto;
    /// <summary>
    /// 自動スパン
    /// </summary>
    [SerializeField]
    private int AutoSpan;
    /// <summary>
    /// 自動カウンタ
    /// </summary>
    private float AutoCount = 0;
    /// <summary>
    /// テキストデータのパス
    /// </summary>
    private string filePath = "Text/";
    /// <summary>
    /// 読み込んだテキストデータを格納するテキストアセット
    /// </summary>
    private TextAsset stageTextAsset;
    /// <summary>
    /// ステージの文字列データ
    /// </summary>
    private string stageData;
    /// <summary>
    /// ステージの文字列データの添え字
    /// </summary>
    private int stageDataIndex = 0;
    /// <summary>
    /// 抽出した文字を格納する変数
    /// </summary>
    private char pattern;
    /// <summary>
    /// メッセージ送り
    /// </summary>
    private int sendCount = 0;
    /// <summary>
    /// 任意指定文字で区切った文字列を格納する配列
    /// </summary>
    private string[] talkInfo;
    /// <summary>
    /// テキストのリスト
    /// </summary>
    private List<string> textList = new List<string>();
    /// <summary>
    /// 指定IDのテキストが何個あるかのカウンタ
    /// </summary>
    private int messageCount = 0;
    /// <summary>
    /// テキストの最初の要素番号を取得したか
    /// </summary>
    private bool isGetStartText = false;
    /// <summary>
    /// 表示し始めるテキストの配列要素位置
    /// </summary>
    private int startText = 0;
    /// <summary>
    /// イベント番号
    /// </summary>
    private static int EventNumber = 0;
    /// <summary>
    /// イベントを登録したかどうか
    /// </summary>
    private bool isEventSignup = false;
    /// <summary>
    /// 読み終わった後に遷移するシーン
    /// </summary>
    [SerializeField]
    private string NextScene;

    /// <summary>
    /// 初期化
    /// </summary>
    void Awake()
    {
        filePath += Application.loadedLevelName.ToString();
        ReadTextData();
        //EventManager = GameObject.Find("EventManager");
    }
	/// <summary>
	/// 初期化
	/// </summary>
	void Start () 
    {
        Time.timeScale = 1;
        Screen.lockCursor = true;
        UnityEngine.Cursor.visible = false;
        SendMessage();
	}
	
    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        //読む
        if (isAuto)
        {
            AutoCount += Method.GameTime();
            //Debug.Log((int)AutoCount);
            if (AutoCount >= AutoSpan * 60)
            {
                AutoCount = 0;
                SendMessage();
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            SendMessage();
        }
    }

    /// <summary>
    /// テキストデータの読み込み
    /// </summary>
    void ReadTextData()
    {
        // TextAssetとして、Resourcesフォルダからテキストデータをロードする
        stageTextAsset = Resources.Load(filePath) as TextAsset;
        // 文字列を代入
        stageData = stageTextAsset.text;
        // 空白を置換で削除
        //stageData = stageData.Replace(" ", "");

        //文字列を区切る(改行代わり)
        //これで一文を取得
        char[] kugiri = { '@' };
        talkInfo = stageData.Split(kugiri);

        //行の頭に#がついている、もしくは行が空であればその行の処理はスキップ
        for (int i = 0; i < talkInfo.Length; i++)
        {
            if (talkInfo[i].StartsWith("#") ||
                talkInfo[i].StartsWith("\t") ||
                talkInfo[i] == "")
            {
                //Debug.Log("SKIP!");
                continue;
            }
            //名前の前に何故か入る改行を削除
            talkInfo[i] = talkInfo[i].Replace("\n", "");
            talkInfo[i] = talkInfo[i].Replace("\r", "");
            //指定した箇所にのみ改行コードを挿入
            talkInfo[i] = talkInfo[i].Replace("/", "\n");
            //Debug.Log(talkInfo[i]);
            //テキスト内容をリストに格納
            textList.Add(talkInfo[i]);
        }
    }

    /// <summary>
    /// メッセージ送り
    /// </summary>
    private void SendMessage()
    {
        //指定された文と同一文のテキストの数を取得（行数を超えた出力を防ぐため）
        for (int i = 0; i < textList.Count; i++)
        {
            //テキストを取得した時の最初のテキストの文を取得
            if (!isGetStartText)
            {
                isGetStartText = true;
                startText = i;
            }
            messageCount++;
        }
        //指定されたIDの全テキストが全て表示し終えたら
        if (sendCount >= messageCount)
        {
            //初期化
            sendCount = -1;
            messageCount = 0;
            isGetStartText = false;
            isEventSignup = false;
            LoadingController.NextScene(NextScene);
        }
        else
        {
            //会話内容を送る
            Message.setgetMessage = textList[sendCount + startText];
            messageCount = 0;
        }
        sendCount++;
    }
}
