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
    /// 子カメラオブジェクト
    /// </summary>
    private GameObject CameraObject;
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

    void Awake()
    {
        CameraObject = this.transform.FindChild("Camera").gameObject;
        CursorObject = Resources.Load("Prefab/MagicCursor") as GameObject;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        MouseEvent();
        CameraMove();
        CameraRay();
        if (CursorObjectInst == null)
        {
            CursorObjectInst = (GameObject)Instantiate(CursorObject, RayHitPoint, CursorObject.transform.rotation);
        }
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
        Vector3 moveVec = new Vector3(mouseX, 0, mouseY);
        CameraObject.rigidbody.AddForce(moveVec * 80);
    }

    /// <summary>
    /// カメラから地面に飛ばすレイ
    /// </summary>
    void CameraRay()
    {
        if (Physics.Raycast(CameraObject.transform.position, Vector3.down, out hit, Mathf.Infinity))
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
