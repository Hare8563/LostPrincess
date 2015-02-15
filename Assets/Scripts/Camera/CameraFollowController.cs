using UnityEngine;
using System.Collections;

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

    // Use this for initialization
    void Start()
    {
        CameraIcon = GameObject.Find("ZoomCameraIcon");
        CameraIconInit_Y = CameraIcon.transform.position.y;
    }

    // Update is called once per frame
    void Update()
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
			float backDis = 5.0f;
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
            //カメラ移動
            Vector3 toOriginVec = Camera.main.transform.position - this.transform.parent.position;
            Camera.main.transform.position = this.transform.position + toOriginVec * mouceWheelValue;
            //カメラ位置反映
            CameraIcon.transform.position = new Vector3(CameraIcon.transform.position.x, CameraIconInit_Y - mouceWheelValue * 300f, CameraIcon.transform.position.z);
        }
        //Camera.main.transform.LookAt(this.transform.parent.transform.position);
    }
}
