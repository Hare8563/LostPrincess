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
    private float mouseWheel;
    /// <summary>
    /// マウスホイールの合計移動量
    /// </summary>
    private float mouseWheelValue;
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
        //CameraXShit();
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
        //プレイヤーの頭上から自身へのベクトル
        Vector3 toVec = this.transform.position - this.transform.parent.gameObject.transform.position;
        //プレイヤーの頭上と自身との距離
        float dis = Vector3.Distance(this.transform.position, this.transform.parent.transform.position);

        //カメラとプレイヤーとの間に障害物が存在したら
        if (Physics.Raycast(this.transform.parent.transform.position, toVec, out hit, dis, 1 << LayerMask.NameToLayer("Stage")))
        {
            //ポリゴン埋まりを防ぐために少しプレイヤー方向へカメラを近づける
            float backDis = 2.0f;
            Camera.main.transform.position = hit.point - toVec.normalized * backDis;
        }
        else
        {
            //マウスホイールによってプレイヤーとの距離を変更
            mouseWheel = Input.GetAxis("Mouse ScrollWheel");
            if (mouseWheel != 0)
            {
                mouseWheelValue += -mouseWheel;
                //距離制限
                float maxDis = 0.5f;
                if (mouseWheelValue < -maxDis) 
                {
                    mouseWheelValue = -maxDis;
                    mouseWheel = 0;
                }
                else if (mouseWheelValue > maxDis) 
                {
                    mouseWheelValue = maxDis;
                    mouseWheel = 0;
                }
                //カメラ移動
                Vector3 toOriginVec = this.transform.position - this.transform.parent.position;
                this.transform.position += toOriginVec * -mouseWheel;
                //カメラ位置反映
                CameraIcon.transform.position = new Vector3(CameraIcon.transform.position.x, CameraIconInit_Y - mouseWheelValue * 300f, CameraIcon.transform.position.z);
                //Camera.main.transform.LookAt(this.transform.parent.transform.position);
            }
            //カメラを自身の場所へ移動
            Camera.main.transform.position = this.transform.position;
        }
    }
}