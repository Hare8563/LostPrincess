using UnityEngine;
using System.Collections;

public class MagicCursorScript : MonoBehaviour
{
    /// <summary>
    /// 魔法生成オブジェクト
    /// </summary>
    private GameObject MagicOriginObject;
    /// <summary>
    /// 魔法生成スクリプト
    /// </summary>
    private MagicScript magicScript;
    /// <summary>
    /// マウスボタンの列挙体
    /// </summary>
    private enum MouseButtonEnum
    {
        LEFT_BUTTON = 0,
        RIGHT_BUTTON,
        CENTER_BUTTON,
    }
    /// <summary>
    /// マウスボタンの構造体
    /// </summary>
    private struct MouseButtonStruct
    {
        public bool right;
        public bool left;
        public bool center;
        public bool rightDown;
        public bool leftDown;
        public bool leftUp;
        public bool centerDown;
    }
    /// <summary>
    /// マウスボタン
    /// </summary>
    private MouseButtonStruct mouseButton;
    /// <summary>
    /// カーソルの大きさ
    /// </summary>
    private float scale = 1;
    /// <summary>
    /// カーソルの最大サイズ
    /// </summary>
    [SerializeField]
    private float MaxScale;
    /// <summary>
    /// 魔法発動フラグ
    /// </summary>
    private bool InvocationFlag = false;
    /// <summary>
    /// メテオオブジェクト
    /// </summary>
    private GameObject MeteoObject;
    /// <summary>
    /// メテオを発射するまでの時間
    /// </summary>
    private float InvoSecond = 0;
    /// <summary>
    /// メテオを発射した回数
    /// </summary>
    private int InvoCount = 0;
    /// <summary>
    /// カメラコントロールオブジェクト
    /// </summary>
    private CameraController cameraController;

    void Awake()
    {
        MagicOriginObject = GameObject.Find("MagicOrigin(Clone)");
        magicScript = MagicOriginObject.GetComponent<MagicScript>();
        MeteoObject = Resources.Load("Prefab/MeteoBall") as GameObject;
        cameraController = GameObject.Find("CameraControllPoint").GetComponent<CameraController>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MouseEvent();
        Scale();
        Move();
        MouseUpEevent();
        MagicInvocation();
    }

    /// <summary>
    /// マウスイベント
    /// </summary>
    void MouseEvent()
    {
        mouseButton.right = Input.GetMouseButton((int)MouseButtonEnum.RIGHT_BUTTON);
        mouseButton.left = Input.GetMouseButton((int)MouseButtonEnum.LEFT_BUTTON);
        mouseButton.center = Input.GetMouseButton((int)MouseButtonEnum.CENTER_BUTTON);
        mouseButton.rightDown = Input.GetMouseButtonDown((int)MouseButtonEnum.RIGHT_BUTTON);
        mouseButton.leftDown = Input.GetMouseButtonDown((int)MouseButtonEnum.LEFT_BUTTON);
        mouseButton.leftUp = Input.GetMouseButtonUp((int)MouseButtonEnum.LEFT_BUTTON);
        mouseButton.centerDown = Input.GetMouseButtonDown((int)MouseButtonEnum.CENTER_BUTTON);
    }

    /// <summary>
    /// スケールの変更
    /// </summary>
    void Scale()
    {
        //if (mouseButton.left && scale < MaxScale)
        //{
        //    scale += 0.03f;
        //}
        scale = MaxScale;
        this.transform.localScale = new Vector3(scale, 1, scale);
    }

    /// <summary>
    /// 移動
    /// </summary>
    void Move()
    {
        if (magicScript != null)
        {
            this.transform.position = magicScript.getRayHitPoint();
        }
    }

    /// <summary>
    /// マウスボタンが離された時のイベント
    /// </summary>
    void MouseUpEevent()
    {
        if (mouseButton.leftUp)
        {
            //Camera.main.rect = new Rect(0, 0, 1, 1);
            cameraController.setCanMove(true);
            //魔法発動
            InvocationFlag = true;
            Destroy(MagicOriginObject);
        }
        else if (!InvocationFlag)
        {
            //Camera.main.rect = new Rect(0, 0, 0, 0);
            cameraController.setCanMove(false);
        }
    }

    /// <summary>
    /// カーソル範囲内にメテオを落とす
    /// </summary>
    void MagicInvocation()
    {
        if (InvocationFlag)
        {
            InvoSecond += Method.GameTime();
            if ((int)InvoSecond % 20 == 0 &&
                InvoCount < scale)
            {
                float height = 100;
                float AngleRand_X = Random.Range(0, 360 * Mathf.PI / 180);
                float AngleRand_Z = Random.Range(0, 360 * Mathf.PI / 180);
                Vector3 pos = new Vector3(Mathf.Cos(AngleRand_X) * (scale * 1.5f), height, Mathf.Cos(AngleRand_Z) * (scale * 1.5f));
                Instantiate(MeteoObject, this.transform.position + pos, MeteoObject.transform.rotation);
                InvoCount++;
                InvoSecond = 0;
            }
            else if (InvoCount >= scale)
            {
                InvoCount = 0;
                Destroy(this.gameObject);
            }
        }
    }
}
