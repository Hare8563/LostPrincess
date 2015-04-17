using UnityEngine;
using System.Collections;

public class HimeMeramorSceneScript : MonoBehaviour
{
    /// <summary>
    /// 姫,悪姫オブジェクト
    /// </summary>
    [SerializeField]
    private GameObject[] HimeObject;
    /// <summary>
    /// 各姫の初期スケール
    /// </summary>
    private float[] InitScale = new float[2];
    /// <summary>
    /// 各姫の現在のスケール
    /// </summary>
    private float[] NowScale = new float[2];
    /// <summary>
    /// 各姫のスケール
    /// </summary>
    private Vector3[] Scale = new Vector3[2];
    /// <summary>
    /// 全塔オブジェクト
    /// </summary>
    [SerializeField]
    private GameObject Towers;
    /// <summary>
    /// 姫の高さ
    /// </summary>
    private float height_hime = 0;
    /// <summary>
    /// 塔の高さ
    /// </summary>
    private float height_tower = 0;
    /// <summary>
    /// 回転速度
    /// </summary>
    private float RotSpeed = 0;
    /// <summary>
    /// 削除オブジェクト
    /// </summary>
    [SerializeField]
    private GameObject DestroyObject;
    /// <summary>
    /// 次のシーンへ遷移するフラグ
    /// </summary>
    private bool NextSceneFlag = false;
    /// <summary>
    /// 次のシーンへ遷移するまでの時間
    /// </summary>
    private float NextSceneCount = 0;

    // Use this for initialization
    void Start()
    {
        Towers.transform.position = new Vector3(0, -70, 0);
        InitScale[0] = HimeObject[0].transform.localScale.x;
        InitScale[1] = HimeObject[1].transform.localScale.x;
        HimeObject[1].transform.localScale = Vector3.zero;
        NowScale[0] = HimeObject[0].transform.localScale.x;
        NowScale[1] = HimeObject[1].transform.localScale.x;
        foreach (GameObject obj in HimeObject)
        {
            obj.AddComponent<RotationObject>().setSpeed(0);
            obj.GetComponent<RotationObject>().setAxis("y");
        }
        //姫に近づくパス
        iTween.MoveTo(gameObject, iTween.Hash(
            "path", iTweenPath.GetPath("Path_01"),
            "time", 3f,
            "onupdate", "HimeUp",
            "oncomplete", "ToPath_02",
            "easetype", iTween.EaseType.linear));
        height_tower = -70;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(HimeObject[0].transform.position + new Vector3(0,3,0));
        if (NextSceneFlag)
        {
            NextSceneCount += Method.GameTime();
            if (NextSceneCount > 180)
            {
                LoadingController.NextScene("RastBoss");
            }
        }
    }

    /// <summary>
    /// 姫上昇
    /// </summary>
    void HimeUp()
    {
        Method.SmoothChange(ref height_hime, 30, 0.1f);
        foreach (GameObject obj in HimeObject)
        {
            obj.transform.position = new Vector3(0, height_hime, 0);
        }
    }

    /// <summary>
    /// ２番めのパス（周りを回る）
    /// </summary>
    void ToPath_02()
    {
        iTween.MoveTo(gameObject, iTween.Hash(
            "path", iTweenPath.GetPath("Path_02"),
            "time", 12f,
            "onupdate", "HimeRotate",
            "oncomplete", "ToPath_03",
            "easetype", iTween.EaseType.linear));
    }

    /// <summary>
    /// 姫回転
    /// </summary>
    void HimeRotate()
    {
        //float range = 1;
        if (RotSpeed < 3000) RotSpeed += 6;
        foreach (GameObject obj in HimeObject)
        {
            obj.GetComponent<RotationObject>().setSpeed(RotSpeed);
        }
        //姫変形
        float sizeSpeed = 0.1f;
        //善姫を小さく
        Method.SmoothChange(ref NowScale[0], 0, sizeSpeed);
        //悪姫大きく
        Method.SmoothChange(ref NowScale[1], InitScale[1], sizeSpeed);
        HimeObject[0].transform.localScale = new Vector3(NowScale[0], NowScale[0], NowScale[0]);
        HimeObject[1].transform.localScale = new Vector3(NowScale[1], NowScale[1], NowScale[1]);
        //塔上昇
        Method.SmoothChange(ref height_tower, 0, 0.1f);
        Towers.transform.position = new Vector3(0, height_tower, 0);
    }

    /// <summary>
    /// ３番目のパス（姫の目の前へ）
    /// </summary>
    void ToPath_03()
    {
        iTween.MoveTo(gameObject, iTween.Hash(
            "path", iTweenPath.GetPath("Path_03"),
            "time", 5f,
            "oncomplete", "HimeRotateStop",
            "easetype", iTween.EaseType.linear));
    }

    /// <summary>
    /// 姫回転停止
    /// </summary>
    void HimeRotateStop()
    {
        foreach (GameObject obj in HimeObject)
        {
            obj.GetComponent<RotationObject>().setSpeed(0);
            Destroy(obj.GetComponent<RotationObject>());
            obj.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        NextSceneFlag = true;
        Destroy(DestroyObject);
    }
}
