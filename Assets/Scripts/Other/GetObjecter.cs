using UnityEngine;
using System.Collections;

/// <summary>
/// 基本的なオブジェクトを返すクラス
/// </summary>
public class GetObjecter : MonoBehaviour {
    public bool _getPlayer;
    public bool _getEnemy;
    public bool _getBullet;
    public bool _getPlayerAnimator;
    public bool _getEnemyAnimator;
    public bool _getBarrier;

    void Awake()
    {
        GetPlayerFlag = _getPlayer;
        GetEnemyFlag = _getEnemy;
        GetBulletFlag = _getBullet;
        GetPlayerAnimatorFlag = _getPlayerAnimator;
        GetEnemyAnimatorFlag = _getEnemyAnimator;
        GetBarrierFlag = _getBarrier;
        GetMainCameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        GetSceneChangeFlaggerObject = (GameObject)Resources.Load("Prefab/SceneChangeFlagger");
        if (_getPlayer) 
        { 
            GetPlayerObject = GameObject.FindGameObjectWithTag("Player");
        }
        if (_getEnemy)
        {
            GetEnemyObject = GameObject.FindGameObjectWithTag("Enemy");
        }
        if (_getBullet) { GetBulletObject = (GameObject)Resources.Load("Prefab/Bullet"); }
        if (_getPlayerAnimator) { GetPlayerAnimatorObject = GetPlayerObject.GetComponent<Animator>(); }
        if (_getEnemyAnimator) { GetEnemyAnimatorObject = GetEnemyObject.GetComponent<Animator>(); }
        if (_getBarrier) { GetBarrierObject = GameObject.Find("Barrier"); }
    }

    #region Objects
    /// <summary>
    /// プレイヤーオブジェクトのプロパティ
    /// （読み込み専用）
    /// </summary>
    /// <returns></returns>
    public static GameObject GetPlayerObject { private set; get; }
    /// <summary>
    /// プレイヤーウェポンRオブジェクトのプロパティ
    /// （読み込み専用）
    /// </summary>
    /// <returns></returns>
    public static GameObject GetPlayerWeponRObject { private set; get; }
    /// <summary>
    /// プレイヤーウェポンLオブジェクトのプロパティ
    /// （読み込み専用）
    /// </summary>
    /// <returns></returns>
    public static GameObject GetPlayerWeponLObject { private set; get; }

    /// <summary>
    /// エネミーオブジェクトのプロパティ
    /// （読み込み専用）
    /// </summary>
    /// <returns></returns>
    public static GameObject GetEnemyObject { private set; get; }

    /// <summary>
    /// メインカメラオブジェクトのプロパティ
    /// （読み込み専用）
    /// </summary>
    /// <returns></returns>
    public static GameObject GetMainCameraObject { private set; get; }

    /// <summary>
    /// 弾プレハブオブジェクトのプロパティ
    /// （読み込み専用）
    /// </summary>
    /// <returns></returns>
    public static GameObject GetBulletObject { private set; get; }

    /// <summary>
    /// プレイヤーアニメーターのプロパティ
    /// （読み込み専用）
    /// </summary>
    /// <returns></returns>
    public static Animator GetPlayerAnimatorObject { private set; get; }

    /// <summary>
    /// エネミーアニメーターのプロパティ
    /// （読み込み専用）
    /// </summary>
    /// <returns></returns>
    public static Animator GetEnemyAnimatorObject { private set; get; }

    /// <summary>
    /// エネミーターゲット用オブジェクトのプロパティ
    /// （読み込み専用）
    /// </summary>
    /// <returns></returns>
    public static GameObject GetEnemyTargetObject { private set; get; }

    /// <summary>
    /// バリアオブジェクトのプロパティ
    /// （読み込み専用）
    /// </summary>
    /// <returns></returns>
    public static GameObject GetBarrierObject { private set; get; }

    /// <summary>
    /// シーンチェンジ用コライダオブジェクトのプロパティ
    /// （読み込み専用）
    /// </summary>
    /// <returns></returns>
    public static GameObject GetSceneChangeFlaggerObject { private set; get; }
    #endregion

    #region Flags
    /// <summary>
    /// プレイヤーを取得しているかのプロパティ
    /// </summary>
    /// <returns></returns>
    public static bool GetPlayerFlag { private set; get; }

    /// <summary>
    /// 敵を取得しているかのプロパティ
    /// </summary>
    /// <returns></returns>
    public static bool GetEnemyFlag { private set; get; }

    /// <summary>
    /// 弾を取得しているかのプロパティ
    /// </summary>
    /// <returns></returns>
    public static bool GetBulletFlag { private set; get; }

    /// <summary>
    /// プレイヤーのアニメーターを取得しているかのプロパティ
    /// </summary>
    /// <returns></returns>
    public static bool GetPlayerAnimatorFlag { private set; get; }

    /// <summary>
    /// 敵のアニメーターを取得しているかのプロパティ
    /// </summary>
    /// <returns></returns>
    public static bool GetEnemyAnimatorFlag { private set; get; }

    /// <summary>
    /// バリアオブジェクトを取得しているかのプロパティ
    /// </summary>
    /// <returns></returns>
    public static bool GetBarrierFlag { private set; get; }
    #endregion
}
