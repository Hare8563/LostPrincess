using UnityEngine;
using System.Collections;

public class PhotonLazer : MonoBehaviour
{
    /// <summary>
    /// ラインレンダラ（レーザー関連に使用）
    /// </summary>
    private LineRenderer lineRenderer;
    /// <summary>
    /// ラインレンダラの終着点
    /// </summary>
    private Vector3 lineRendererEndPoint;
    /// <summary>
    /// レーザー着弾点エフェクト
    /// </summary>
    private GameObject LazerHitEffect;
    /// <summary>
    /// インスタンス生成後のレーザー着弾点エフェクト
    /// </summary>
    private GameObject hitEffect;
    /// <summary>
    /// 着弾点エフェクトを生成したかどうか
    /// </summary>
    private bool isHitEffect;
    /// <summary>
    /// 角度と向かうべき角度
    /// </summary>
    float xr, zr, yr, gxr, gyr, gzr;
    /// <summary>
    /// 死ぬまでの時間
    /// </summary>
    float DestroyTime = 0;

    void Awake()
    {
        LazerHitEffect = Resources.Load("Prefab/LazerHitEffect") as GameObject;
    }

    // Use this for initialization
    void Start()
    {
        float rote = 30;    //初期角度
        float Range = 0.3f; //角度変化の速度
        //Destroy(this.gameObject, 3.0f);
        xr = Random.Range(-rote, rote) * Mathf.PI / 180;
        yr = Random.Range(-rote, rote) * Mathf.PI / 180;
        zr = Random.Range(-rote, rote) * Mathf.PI / 180;
        gxr = Random.Range(-Range, Range) * Mathf.PI / 180;
        gyr = Random.Range(-Range, Range) * Mathf.PI / 180;
        gzr = Random.Range(-Range, Range) * Mathf.PI / 180;
        this.transform.rotation = new Quaternion(xr, yr, zr, 1);
        isHitEffect = false;
    }

    // Update is called once per frame
    void Update()
    {
        //指定方向のオブジェクトに当たったらエフェクト生成
        Vector3 down = this.transform.TransformDirection(Vector3.down);
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, down, out hit, Mathf.Infinity))
        //{
        //貫通するレイが当たった全てのオブジェクト情報を取得し、衝突箇所にエフェクト生成
        RaycastHit[] hits = Physics.RaycastAll(transform.position, down);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.tag != "Hime")
            {
                //Debug.Log(hit.collider.name);
                lineRendererEndPoint = hits[i].point;
                //Debug.Log(hitEffect);
                if (!isHitEffect)
                {
                    isHitEffect = true;
                    hitEffect = (GameObject)Instantiate(LazerHitEffect, hits[i].point, LazerHitEffect.transform.rotation);
                }
                if (hitEffect != null) hitEffect.transform.position = hits[i].point;
            }
        }
        //ラインレンダラ表示
        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.SetVertexCount(2); //点の数指定
        lineRenderer.SetPosition(0, this.transform.position);
        lineRenderer.SetPosition(1, lineRendererEndPoint);

        //角度更新
        xr += gxr * Method.GameTime();
        yr += gyr * Method.GameTime();
        zr += gzr * Method.GameTime();
        this.transform.rotation = new Quaternion(xr, yr, zr, 1);
        DestroyTime += Method.GameTime();

        //180フレーム経過したら削除
        if (DestroyTime > 180)
        {
            Destroy(hitEffect);
            Destroy(this.gameObject);
        }
    }
}