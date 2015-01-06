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
        Vector3 toVec = this.transform.position - this.transform.parent.gameObject.transform.position;
        float dis = Vector3.Distance(this.transform.position, this.transform.parent.transform.position);
        //Debug.DrawRay(this.transform.parent.transform.position);
        //カメラとプレイヤーとの間に障害物が存在したら
        if (Physics.Raycast(this.transform.parent.transform.position, toVec, out hit, dis, layerMask))
        {
            Camera.main.transform.position = hit.point;
        }
        else
        {
            Camera.main.transform.position = this.transform.position;
        }
    }
}
