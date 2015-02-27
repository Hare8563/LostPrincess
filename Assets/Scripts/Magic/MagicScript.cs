using UnityEngine;
using System.Collections;

public class MagicScript : MonoBehaviour
{
    /// <summary>
    /// マウスのX座標移動量
    /// </summary>
    private float mouseX;
    /// <summary>
    /// マウスのY座標移動量
    /// </summary>
    private float mouseY;
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
        public bool centerDown;
    }
    /// <summary>
    /// マウスボタン
    /// </summary>
    private MouseButtonStruct mouseButton;
    /// <summary>
    /// 下にレイを飛ばすオブジェクト
    /// </summary>
    private GameObject DownRayObject;
    /// <summary>
    /// レイキャスト
    /// </summary>
    private RaycastHit hit;
    /// <summary>
    /// レイが当たった座標
    /// </summary>
    private Vector3 RayHitPoint;
    /// <summary>
    /// カーソルオブジェクト
    /// </summary>
    private GameObject CursorObject;
    /// <summary>
    /// カーソルインスタンスオブジェクト
    /// </summary>
    private GameObject CursorObjectInst;
    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    private GameObject PlayerObject;

    void Awake()
    {
        DownRayObject = this.transform.FindChild("DownRay").gameObject;
        CursorObject = Resources.Load("Prefab/MagicCursor") as GameObject;
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
    }

	// Use this for initialization
	void Start () {
        Instantiate(CursorObject, RayHitPoint, CursorObject.transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
        MouseEvent();
        CameraRay();
	}

    void FixedUpdate()
    {
        CameraMove();
    }

    /// <summary>
    /// マウスイベント
    /// </summary>
    void MouseEvent()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        mouseButton.right = Input.GetMouseButton((int)MouseButtonEnum.RIGHT_BUTTON);
        mouseButton.left = Input.GetMouseButton((int)MouseButtonEnum.LEFT_BUTTON);
        mouseButton.center = Input.GetMouseButton((int)MouseButtonEnum.CENTER_BUTTON);
        mouseButton.rightDown = Input.GetMouseButtonDown((int)MouseButtonEnum.RIGHT_BUTTON);
        mouseButton.leftDown = Input.GetMouseButtonDown((int)MouseButtonEnum.LEFT_BUTTON);
        mouseButton.centerDown = Input.GetMouseButtonDown((int)MouseButtonEnum.CENTER_BUTTON);
    }

    /// <summary>
    /// カメラの移動
    /// </summary>
    void CameraMove()
    {
        float speed = 80;
        if (mouseX != 0)
        {
            DownRayObject.rigidbody.AddForce(DownRayObject.transform.TransformDirection(Vector3.right) * mouseX * speed);
        }
        if (mouseY != 0)
        {
            DownRayObject.rigidbody.AddForce(DownRayObject.transform.TransformDirection(Vector3.up) * mouseY * speed);
        }
    }

    /// <summary>
    /// カメラから地面に飛ばすレイ
    /// </summary>
    void CameraRay()
    {
        if (Physics.Raycast(DownRayObject.transform.position, Vector3.down, out hit, Mathf.Infinity, 1<<LayerMask.NameToLayer("Stage")))
        {
            RayHitPoint = hit.point + new Vector3(0, 0.1f, 0);
        }
    }

    /// <summary>
    /// レイが衝突した座標を取得
    /// </summary>
    /// <returns></returns>
    public Vector3 getRayHitPoint()
    {
        return RayHitPoint;
    }
}
