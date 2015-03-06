using UnityEngine;
using System.Collections;

public class GameOverManager : MonoBehaviour {
    /// <summary>
    /// ゲームオーバーロゴ
    /// </summary>
    [SerializeField]
    private GameObject Rogo;
    /// <summary>
    /// 赤背景
    /// </summary>
    [SerializeField]
    private GameObject BackPlane;
    /// <summary>
    /// 背景アルファ値
    /// </summary>
    private float backAlpha;
    /// <summary>
    /// ロゴアルファ値
    /// </summary>
    private float rogoAlpha;
    /// <summary>
    /// 背景アルファ値の変更速度
    /// </summary>
    private float aBackVelocity = 0;
    /// <summary>
    /// ロゴアルファ値の変更速度
    /// </summary>
    private float aRogoVelocity = 0;
    /// <summary>
    /// 背景マテリアル
    /// </summary>
    private Material backMaterial;
    /// <summary>
    /// ロゴマテリアル
    /// </summary>
    private Material rogoMaterial;
    /// <summary>
    /// 遷移にかける時間
    /// </summary>
    [SerializeField]
    private float time;
    /// <summary>
    /// アルファ値の最大値
    /// </summary>
    [SerializeField]
    [Range(0, 1.0f)]
    private float maxAlpha;

	// Use this for initialization
	void Start () 
    {
        backAlpha = 0;
        rogoAlpha = 0;
        backMaterial = BackPlane.renderer.material;
        rogoMaterial = Rogo.renderer.material;
        backMaterial.color = new Color(backMaterial.color.r, backMaterial.color.g, backMaterial.color.b, backAlpha);
        rogoMaterial.color = new Color(rogoMaterial.color.r, rogoMaterial.color.g, rogoMaterial.color.b, rogoAlpha);
	}
	
	// Update is called once per frame
	void Update () 
    {
        //1秒かけてアルファ値変更
        backAlpha = Mathf.SmoothDamp(backAlpha, maxAlpha, ref aBackVelocity, time);
        rogoAlpha = Mathf.SmoothDamp(rogoAlpha, 1.0f, ref aRogoVelocity, time);
        backMaterial.color = new Color(backMaterial.color.r, backMaterial.color.g, backMaterial.color.b, backAlpha);
        rogoMaterial.color = new Color(rogoMaterial.color.r, rogoMaterial.color.g, rogoMaterial.color.b, rogoAlpha);
        if (rogoAlpha > 0.9f)
        {
            LoadingController.NextScene(Application.loadedLevelName);
        }
	}
}
