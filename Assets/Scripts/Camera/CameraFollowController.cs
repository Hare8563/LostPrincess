using UnityEngine;
using System.Collections;

public class CameraFollowController : MonoBehaviour
{
    /// <summary>
    /// レイヤーマスク
    /// </summary>
    public LayerMask layerMask;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        //プレイヤーの頭上からカメラへのベクトル
        Vector3 toVec = this.transform.position - this.transform.parent.gameObject.transform.position;
		//プレイヤーの頭上とカメラとの距離
        float dis = Vector3.Distance(this.transform.position, this.transform.parent.transform.position);
        //Debug.DrawRay(this.transform.parent.transform.position);
        //カメラとプレイヤーとの間に障害物が存在したら
        if (Physics.Raycast(this.transform.parent.transform.position, toVec, out hit, dis, layerMask))
        {
            //ポリゴン埋まりを防ぐために少しプレイヤー方向へカメラを戻す
			float backDis = 0.3f;
			Camera.main.transform.position = hit.point - toVec.normalized * backDis;
        }
        else
        {
            Camera.main.transform.position = this.transform.position;
        }
    }
}
