using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AimScript : MonoBehaviour
{
    /// <summary>
    /// 射程距離
    /// </summary>
    [SerializeField]
    private float Dis;
    /// <summary>
    /// レイキャスト
    /// </summary>
    private RaycastHit hit;
    /// <summary>
    /// 照準カーソルオブジェクト
    /// </summary>
    private Image AimCursorImage;

    // Use this for initialization
    void Start()
    {
        AimCursorImage = GameObject.Find("AimCenter").GetComponent<Image>();
        //Debug.Log(AimCursorImage);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 origin = Camera.main.transform.position;
        Vector3 direction = Camera.main.transform.TransformDirection(Vector3.forward);
        //Vector3 ViewPoint;// = new Vector3(hit.point.x * Screen.width, hit.point.y * Screen.height, 0);
        if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Collider"))))
        {
            //Debug.Log(hit.collider.name);
            if (hit.collider.tag == "Enemy" ||
                hit.collider.tag == "Boss" ||
                hit.collider.tag == "Hime")
            {
                AimCursorImage.color = Color.red;
            }
            else
            {
                AimCursorImage.color = Color.white;
            }
            this.transform.LookAt(hit.point);
        }
        else
        {
            AimCursorImage.color = Color.white;
        }
    }
}
