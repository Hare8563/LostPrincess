using UnityEngine;
using System.Collections;

/// <summary>
/// カメラの回転原点
/// </summary>
public class CameraController : MonoBehaviour {
	/// <summary>
	/// 追うオブジェクト
	/// </summary>
    [SerializeField]
	private GameObject target;
    /// <summary>
    /// ターゲットオブジェクトとの距離
    /// </summary>
    private Vector3 distance;
    /// <summary>
    /// マウスのスクリーン座標
    /// </summary>
    private Vector2 mousePosition;
    /// <summary>
    /// マウスのX座標移動量
    /// </summary>
    private float mouseX;
    /// <summary>
    /// マウスのY座標移動量
    /// </summary>
    private float mouseY;
    /// <summary>
    /// カメラの回転
    /// </summary>
    private Vector2 rotation;
    /// <summary>
    /// X軸最大可動域
    /// </summary>
    [SerializeField]
    [Range(0,180)]
    private float maxRage_X;
    /// <summary>
    /// X軸最小可動域
    /// </summary>
    [SerializeField]
    [Range(-180, 0)]
    private float minRage_X;
    /// <summary>
    /// 回転速度
    /// </summary>
    [SerializeField]
    [Range(0, 10)]
    private float speed;
    /// <summary>
    /// 移動可能かどうか
    /// </summary>
    private bool canMove = true;

    void Awake()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
    }
	
	void Start () 
    {
        distance = this.transform.position - target.transform.position;
        Screen.lockCursor = true;
        Screen.showCursor = false;
	}

    void Update()
    {
        this.transform.position = target.transform.position + distance;
        if (canMove)
        {
            //Debug.Log(transform.TransformDirection(Vector3.forward));
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            rotation.x = this.transform.localEulerAngles.x;
            rotation.y = this.transform.localEulerAngles.y;
            rotation.x += -mouseY * speed;   //上下回転
            rotation.y += mouseX * speed;  //左右回転
            if (rotation.x > 180) rotation.x -= 360;
            //Debug.Log(rotation.x);
            if (rotation.x > maxRage_X)
            {
                rotation.x = maxRage_X;
            }
            if (rotation.x < minRage_X)
            {
                rotation.x = minRage_X;
            }
            this.transform.localEulerAngles = new Vector3(rotation.x, rotation.y, 0);
            ChangeCursor();
        }
    }

    /// <summary>
    /// カメラの方向情報を取得する
    /// </summary>
    /// <param name="direction">取得したい方向</param>
    /// <returns>カメラの方向</returns>
    public Vector3 getCameraDirection(Vector3 direction)
    {
        Vector3 cameraDirection = Camera.main.transform.TransformDirection(direction);
        cameraDirection = new Vector3(cameraDirection.x, 0, cameraDirection.z);
        return cameraDirection;
    }

    /// <summary>
    /// マウス表示/非表示切り替え
    /// </summary>
    void ChangeCursor()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Screen.lockCursor = !Screen.lockCursor;
            Screen.showCursor = !Screen.showCursor;
        }
    }

    /// <summary>
    /// 移動可能かどうかを設定する
    /// </summary>
    public void setCanMove(bool value)
    {
        canMove = value;
    }
}
