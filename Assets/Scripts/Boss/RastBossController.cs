using UnityEngine;
using System.Collections;
using HimeSkillClass;
using StatusClass;

public class RastBossController : MonoBehaviour
{
    /// <summary>
    /// ステータスクラス
    /// </summary>
    Status status;
    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    private GameObject PlayerObject;
    /// <summary>
    /// 攻撃開始点
    /// </summary>
    private GameObject[] AttackPoints;
    /// <summary>
    /// 現在のエフェクトサイズ
    /// </summary>
    private float nowEffectSize = 0;
    /// <summary>
    /// エフェクトの最小サイズ
    /// </summary>
    private const float EfectSize_Min = 0;
    /// <summary>
    /// エフェクトの最大サイズ
    /// </summary>
    private const float EffectSize_Max = 30;
    /// <summary>
    /// 現在のエフェクトライト光量
    /// </summary>
    private float nowLightIntensity = 0;
    /// <summary>
    /// エフェクトライトの最小光量
    /// </summary>
    private const float EfectLightIntensity_Min = 0;
    /// <summary>
    /// エフェクトライトの最大光量
    /// </summary>
    private const float EfectLightIntensity_Max = 3;
    /// <summary>
    /// エフェクトサイズと光量の変化スピード
    /// </summary>
    [SerializeField]
    [Range(0, 2.0f)]
    private float EffectChangeSpeed = 0.5f;
    /// <summary>
    /// HimeSkillクラス
    /// </summary>
    private HimeSkill himeSkill;
    /// <summary>
    /// スキル・ハイラッシュ時のエフェクト
    /// </summary>
    private GameObject DashEffect;
    /// <summary>
    /// スキル・ハイラッシュ時のエフェクトが有効か
    /// </summary>
    private bool isDashEffect = false;
    /// <summary>
    /// スキル・ビッグメテオ使用のタイミング
    /// </summary>
    private int BigMeteoTiming = 0;
    /// <summary>
    /// スキル・フォトンレーザーの個数
    /// </summary>
    private int PhotonLaserNum = 0;
    /// <summary>
    /// 攻撃のフラグ
    /// </summary>
    private bool AttackFlag = false;
    /// <summary>
    /// アニメーションコントローラ
    /// </summary>
    private Animator animator;
    /// <summary>
    /// base layerで使われる、アニメーターの現在の状態の参照
    /// </summary>
    private AnimatorStateInfo currentBaseState;
    //各アニメーションへのステート
    static int flyState = Animator.StringToHash("Base Layer.Fly");
    static int attack_01State = Animator.StringToHash("Base Layer.Attack_01");
    static int attack_02State = Animator.StringToHash("Base Layer.Attack_02");

    void Awake()
    {
        animator = this.gameObject.GetComponent<Animator>();
        AttackPoints = GameObject.FindGameObjectsWithTag("Hime_AttackPoint");
        DashEffect = this.transform.FindChild("DashEffect").gameObject;
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Use this for initialization
    void Start()
    {
        status = new Status(10, 0, 100, 100);
        //エフェクトサイズとライト光量を初期化
        nowEffectSize = EfectSize_Min;
        nowLightIntensity = EfectLightIntensity_Min;
        foreach (GameObject i in AttackPoints)
        {
            i.particleSystem.startSize = nowEffectSize;
            i.light.intensity = nowLightIntensity;
        }
        DashEffect.SetActive(isDashEffect);
    }

    // Update is called once per frame
    void Update()
    {
        AttackFlag = true;
        AnimationController();
        DashEffect.SetActive(isDashEffect);
    }

    /// <summary>
    /// ノーマルスキル・ハイラッシュ攻撃パターン
    /// </summary>
    void HighRash()
    {
        //スキル発動
        isDashEffect = true;
        himeSkill = new HimeSkill(this.transform.position, this.transform.rotation, this.gameObject);
        himeSkill.HighRash();
    }

    /// <summary>
    /// ノーマルスキル・ビッグメテオ攻撃パターン
    /// </summary>
    void BigMeteo()
    {
        isDashEffect = false;
        //移動
        //TODO:移動処理（必要なら）

        //攻撃エフェクトサイズ等更新
        Method.SmoothChange(ref nowEffectSize, EffectSize_Max, EffectChangeSpeed);
        Method.SmoothChange(ref nowLightIntensity, EfectLightIntensity_Max, EffectChangeSpeed);
        //値を反映
        AttackPoints[BigMeteoTiming].particleSystem.startSize = nowEffectSize;
        AttackPoints[BigMeteoTiming].light.intensity = nowLightIntensity;
        //エフェクトサイズが最大になったら
        if (AttackPoints[BigMeteoTiming].particleSystem.startSize > EffectSize_Max)
        {
            //エフェクト関連初期化
            AttackPoints[BigMeteoTiming].particleSystem.startSize = nowEffectSize = EfectSize_Min;
            AttackPoints[BigMeteoTiming].light.intensity = nowLightIntensity = EfectLightIntensity_Min;
            //スキル発動
            himeSkill = new HimeSkill(AttackPoints[BigMeteoTiming].transform.position, AttackPoints[BigMeteoTiming].transform.rotation);
            himeSkill.BigMeteo();
            //メテオ出現箇所を入れ替える
            BigMeteoTiming = BigMeteoTiming == 0 ? 1 : 0;
        }
        //プレイヤーの方向を向く
        //敵方向を向く
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(PlayerObject.transform.position - this.transform.transform.position), 0.07f);
        this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);
    }

    /// <summary>
    /// ノーマルスキル・フォトンレーザー攻撃パターン
    /// </summary>
    void PhotonLaser()
    {
        isDashEffect = false;
        //チャージ行動（？）
        //TODO:チャージ行動処理

        //両羽から照射
        if (PhotonLaserNum < 12)
        {
            foreach (GameObject i in AttackPoints)
            {
                //スキル発動
                himeSkill = new HimeSkill(i.transform.position, i.transform.rotation, i);
                himeSkill.PhotonLaser();
                PhotonLaserNum++;
            }
        }
    }

    /// <summary>
    /// バーサクスキル・ハイトルネード攻撃パターン
    /// </summary>
    void HighTornado()
    {
        isDashEffect = false;
        himeSkill = new HimeSkill(this.transform.position, this.transform.rotation, this.gameObject);
        himeSkill.HighTornado();
    }

    /// <summary>
    /// バーサクスキル・ビッグマイン攻撃パターン
    /// </summary>
    void BigMine()
    {
        isDashEffect = false;
        himeSkill = new HimeSkill(this.transform.position, this.transform.rotation, this.gameObject);
        himeSkill.BigMine();
    }

    /// <summary>
    /// バーサクスキル・オメガレーザー攻撃パターン
    /// </summary>
    void OmegaBeam()
    {
        isDashEffect = false;
        himeSkill = new HimeSkill(this.transform.position, this.transform.rotation, this.gameObject);
        himeSkill.OmegaLaser();
    }

    /// <summary>
    /// アニメーション管理
    /// </summary>
    void AnimationController()
    {
        // 参照用のステート変数にBase Layer (0)の現在のステートを設定する
        currentBaseState = this.animator.GetCurrentAnimatorStateInfo(0);
        animator.SetBool("AttackFlag", AttackFlag);

        //攻撃態勢だったら
        if (currentBaseState.nameHash == attack_02State)
        {
            //BigMeteo();
            //PhotonLaser();
            //HighRash();

            HighTornado();
        }
    }

    /// <summary>
    /// 外部参照ダメージ処理
    /// </summary>
    /// <param name="val"></param>
    public void Damage(int val)
    {
        //AudioSource audio = this.GetComponent<AudioSource>();
        //audio.Play();
        this.status.HP -= val;
    }
}