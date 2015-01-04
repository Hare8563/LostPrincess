using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	/// <summary>
	/// 追うオブジェクト
	/// </summary>
    [SerializeField]
	private GameObject target;
    /// <summary>
    /// ターゲットオブジェクトとの距離
    /// </summary>
    [SerializeField]
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
    [Range(0,89)]
    private float maxRage_X;
    /// <summary>
    /// 回転速度
    /// </summary>
    [SerializeField]
    [Range(0, 10)]
    private float speed;

    void Awake()
    {

    }
	
	void Start () 
    {
        distance = this.transform.position - target.transform.position;
	}
	
	void Update () 
    {
        //Debug.Log(transform.TransformDirection(Vector3.forward));
		this.transform.position = target.transform.position + distance;
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        rotation.x = this.transform.localEulerAngles.x;
        rotation.y = this.transform.localEulerAngles.y;
        rotation.x += -mouseY * speed;   //上下回転
        rotation.y += mouseX * speed;  //左右回転
        this.transform.localEulerAngles = new Vector3(rotation.x, rotation.y, 0);
	}

    /// <summary>
    /// カメラの方向情報を取得する
    /// </summary>
    /// <param name="direction">取得したい方向</param>
    /// <returns>カメラの方向</returns>
    public Vector3 getCameraDirection(Vector3 direction)
    {
        Vector3 cameraDirection = transform.TransformDirection(direction);
        cameraDirection = new Vector3(cameraDirection.x, 0, cameraDirection.z);
        return cameraDirection;
    }
}
