using UnityEngine;
using System.Collections;

/// <summary>
/// カメラコントロール
/// </summary>
public class CameraFollowController : MonoBehaviour
{
    /// <summary>
    /// マウスホイールの移動量
    /// </summary>
    private float mouceWheel;
    /// <summary>
    /// マウスホイールの合計移動量
    /// </summary>
    private float mouceWheelValue;
    /// <summary>
    /// カメラアイコンUIオブジェクト
    /// </summary>
    private GameObject CameraIcon;
    /// <summary>
    /// カメラアイコンの初期Y座標
    /// </summary>
    private float CameraIconInit_Y;
    /// <summary>
    /// マウスのX座標移動量
    /// </summary>
    private float mouseX;
    /// <summary>
    /// 回転軸からのズレの大きさ
    /// </summary>
    private float ShiftVal = 5;
    /// <summary>
    /// カメラのローカルX座標
    /// </summary>
    private float CameraPosX;
    /// <summary>
    /// カメラが回転した方向の列挙体
    /// </summary>
    private enum CameraMoveVec
    {
        RIGHT,
        LEFT,
    }
    /// <summary>
    /// カメラが回転した方向
    /// </summary>
    private CameraMoveVec cameraMoveVec;

    // Use this for initialization
    void Start()
    {
        CameraIcon = GameObject.Find("ZoomCameraIcon");
        CameraIconInit_Y = CameraIcon.transform.position.y;
        CameraPosX = this.transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        CameraXShit();
        CameraRotation();
    }

    /// <summary>
    /// カメラのローカルX軸移動
    /// </summary>
    void CameraXShit()
    {
        //マウスが動いたと感知するまでの速さ
        float mouseSpeed = 2.0f;
        //移動速度
        float moveSpeed = 0.1f;
        mouseX = Input.GetAxis("Mouse X");
        //右回転
        if (mouseX > mouseSpeed)
        {
            cameraMoveVec = CameraMoveVec.RIGHT;
        }
        //左回転
        else if (mouseX < -mouseSpeed)
        {
            cameraMoveVec = CameraMoveVec.LEFT;
        }
        switch (cameraMoveVec)
        {
            case CameraMoveVec.RIGHT:
                Method.SmoothChangeEx(ref CameraPosX, ShiftVal, moveSpeed);
                break;
            case CameraMoveVec.LEFT:
                Method.SmoothChangeEx(ref CameraPosX, -ShiftVal, moveSpeed);
                break;
        }
        this.transform.localPosition = new Vector3(CameraPosX, 0.5f, -10);
    }

    /// <summary>
    /// カメラの回転と壁避け
    /// </summary>
    void CameraRotation()
    {
        RaycastHit hit;
        //プレイヤーの頭上からカメラへのベクトル
        Vector3 toVec = this.transform.position - this.transform.parent.gameObject.transform.position;
        //プレイヤーの頭上とカメラとの距離
        float dis = Vector3.Distance(this.transform.position, this.transform.parent.transform.position);
        //カメラとプレイヤーとの間に障害物が存在したら
        if (Physics.Raycast(this.transform.parent.transform.position, toVec, out hit, dis, 1 << LayerMask.NameToLayer("Stage")))
        {
            //ポリゴン埋まりを防ぐために少しプレイヤー方向へカメラを戻す
            float backDis = 0.5f;
            Camera.main.transform.position = hit.point - toVec.normalized * backDis;
        }
        else
        {
            //カメラを自身の場所へ移動
            //Camera.main.transform.position = this.transform.position;
            //マウスホイールによってプレイヤーとの距離を変更
            mouceWheel = Input.GetAxis("Mouse ScrollWheel");
            mouceWheelValue += -mouceWheel;
            //距離制限
            if (mouceWheelValue < -0.5f) { mouceWheelValue = -0.5f; }
            else if (mouceWheelValue > 0.5f) { mouceWheelValue = 0.5f; }
            Vector3 toOriginVec = Camera.main.transform.position - this.transform.parent.position;
            Camera.main.transform.position = this.transform.position + toOriginVec * mouceWheelValue;
            //Debug.Log(mouceWheelValue * 100f);
            CameraIcon.transform.position = new Vector3(CameraIcon.transform.position.x, CameraIconInit_Y - mouceWheelValue * 300f, CameraIcon.transform.position.z);
        }
        //Camera.main.transform.LookAt(this.transform.parent.transform.position);
    }
}
