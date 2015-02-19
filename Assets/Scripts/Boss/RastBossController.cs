//#define skillDebug

using UnityEngine;
using System.Collections;
using HimeSkillClass;
using StatusClass;

public class RastBossController : MonoBehaviour
{
    #region 変数定義
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

    /// <summary>
    /// 移動するタイミング
    /// </summary>
    private float moveTiming = 0;
    /// <summary>
    /// その場に待機した時間
    /// </summary>
    private float nowStayTime = 0;
    /// <summary>
    /// 次に向かうべき座標
    /// </summary>
    private Vector3 nextPosition;
    /// <summary>
    /// 現在いる座標
    /// </summary>
    private Vector3 nowPosition;
    /// <summary>
    /// 次の攻撃までの時間
    /// </summary>
    private float nextAttackTime = 0;
    /// <summary>
    /// 次の攻撃に移るまでに経過した時間
    /// </summary>
    private float nowStayAttackTime = 0;
    /// <summary>
    /// ハイラッシュ使用時間
    /// </summary>
    private const float useHighRashTime = 1800;
    /// <summary>
    /// ビッグメテオ使用時間
    /// </summary>
    private const float useBigMeteoTime = 615;
    /// <summary>
    /// フォトンレーザー使用時間
    /// </summary>
    private const float usePhotonLaserTime = 300;
    /// <summary>
    /// ハイトルネード使用時間
    /// </summary>
    private const float useHighTornadoTime = 1200;
    /// <summary>
    /// ビッグマイン使用時間
    /// </summary>
    private const float useBigMineTime = 900;
    /// <summary>
    /// オメガビーム使用時間
    /// </summary>
    private const float useOmegaBeamTime = 600;
    /// <summary>
    /// 使用するノーマルスキルをランダムに決める
    /// </summary>
    private int randomUse_NormalSkill = 0;
    /// <summary>
    /// 既に使用したノーマルスキル
    /// </summary>
    private int oldUse_NormalSkill = 0;
    /// <summary>
    /// 使用するバーサクスキルをランダムに決める
    /// </summary>
    private int randomUse_BerserkSkill = 0;
    /// <summary>
    /// 既に使用したバーサクスキル
    /// </summary>
    private int oldUse_BerserkSkill = 0;
    /// <summary>
    /// バーサク状態か
    /// </summary>
    private bool isBerserk = false;
	/// <summary>
	/// ダウン状態か
	/// </summary>
	private bool isDown = false;
	/// <summary>
	/// ダウンしている時間
	/// </summary>
	private float DownTime = 0;
	/// <summary>
	/// 下に落ちているか
	/// </summary>
	private bool isGround = false;
	/// <summary>
	/// 死亡しているか
	/// </summary>
	private bool isDead = false;
    /// <summary>
    /// シールドオブジェクト
    /// </summary>
    private GameObject ShieldObject;
    /// <summary>
    /// シールドを展開するかどうか
    /// </summary>
    private bool isShield = false;
    /// <summary>
    /// シールドコントローラー
    /// </summary>
    private ShieldController shieldController;
    /// <summary>
    /// スキルを使用した時間
    /// </summary>
    private float usingSkillTime = 0;
    /// <summary>
    /// HPバーオブジェクト
    /// </summary>
    private GameObject HpBarObject;
    /// <summary>
    /// HPが半分以下になったか
    /// </summary>
    private bool isHarf = false;
    /// <summary>
    /// 初期HP
    /// </summary>
    private float initHp;
    /// <summary>
    /// 攻撃アイコンオブジェクト
    /// </summary>
    private GameObject AttackIconObject;
    /// <summary>
    /// ステータスマネージャークラス
    /// </summary>
    private EnemyStatusManager enemyStatusManager;

#if skillDebug
    //ノーマルスキル
    public bool useBigMeteo = false;
    public bool usePhotonLaser = false;
    public bool useHighRash = false;

    //バーサクスキル
    public bool useHighTornado = false;
    public bool useBigMine = false;
    public bool useOmegaBeam = false;
#endif
    #endregion

    /// <summary>
    /// 初期化
    /// </summary>
    void Awake()
    {
        animator = this.gameObject.GetComponent<Animator>();
        AttackPoints = GameObject.FindGameObjectsWithTag("Hime_AttackPoint");
        DashEffect = this.transform.FindChild("DashEffect").gameObject;
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        ShieldObject = GameObject.Find("Shield");
        AttackIconObject = GameObject.Find("HimeAttackIcon");
        enemyStatusManager = this.gameObject.GetComponent<EnemyStatusManager>();
        //Debug.Log(AttackIconObject);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
		//status = new Status(30, "CSV/RastBassTable");
        status = enemyStatusManager.getStatus();
        //エフェクトサイズとライト光量を初期化
        nowEffectSize = EfectSize_Min;
        nowLightIntensity = EfectLightIntensity_Min;
        foreach (GameObject i in AttackPoints)
        {
            i.particleSystem.startSize = nowEffectSize;
            i.light.intensity = nowLightIntensity;
        }
        DashEffect.SetActive(isDashEffect);
        nextPosition = nowPosition = this.transform.position;
        nextAttackTime = Random.Range(240f, 360f);
        ShieldObject.SetActive(isShield);
        initHp = this.GetComponent<EnemyStatusManager>().getStatus().HP;
        //Debug.Log(initHp);
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        //isBerserk = false;
        Move();
		Down();
        AnimationController();
        DashEffect.SetActive(isDashEffect);
        ShieldObject.SetActive(isShield);
        //死んでいたら
        if (enemyStatusManager.getIsDead())
        {
            LoadingController.NextScene("Title");
		}
        //Debug.Log(status.HP);
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    void Move()
    {
        AttackIconObject.GetComponent<AttackIconScript>().setAttackIcon("");
        //HPが半分以下だったら
        if (!isHarf && this.GetComponent<EnemyStatusManager>().getStatus().HP <= initHp / 2)
        {
            isHarf = true;
            if(!isBerserk)isDown = true;
        }
        if (!isDown)
        {
            //非攻撃状態
            if (!AttackFlag)
            {
                //移動可能範囲
                float canAreaMoveDistance = 70;
                float Speed = 0.05f;
                //ランダム時間毎に移動する
                nowStayTime += Method.GameTime();
                if (nowStayTime > moveTiming)
                {
                    nowStayTime = 0;
                    if (!isHarf) moveTiming = Random.Range(0f, 120f);
                    else moveTiming = Random.Range(0f, 60f);
                    float AngleRand_X = Random.Range(0, 360 * Mathf.PI / 180);
                    float AngleRand_Z = Random.Range(0, 360 * Mathf.PI / 180);
                    nextPosition.x = Mathf.Cos(AngleRand_X) * canAreaMoveDistance;
                    nextPosition.z = Mathf.Cos(AngleRand_Z) * canAreaMoveDistance;
                }
                //現在座標値を滑らかに目標座標値へ遷移
                Method.SmoothChangeEx(ref nowPosition.x, nextPosition.x, Speed);
                Method.SmoothChangeEx(ref nowPosition.z, nextPosition.z, Speed);
                //座標値反映
                this.transform.position = nowPosition;
                //プレイヤーの方向を向く
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(PlayerObject.transform.position - this.transform.transform.position), 0.07f);
                this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);
                //攻撃する時間になったら
                nowStayAttackTime += Method.GameTime();
                if (nowStayAttackTime > nextAttackTime)
                {
                    nowStayAttackTime = 0;
                    nextAttackTime = Random.Range(240f, 360f);
                    //使用スキルを決定
                    randomUse_NormalSkill = Random.Range(0, 3);
                    //前回使用したスキルが再使用されるのを防ぐ
                    while (oldUse_NormalSkill == randomUse_NormalSkill)
                    {
                        randomUse_NormalSkill = Random.Range(0, 3);
                    }
                    randomUse_BerserkSkill = Random.Range(0, 3);
                    while (oldUse_BerserkSkill == randomUse_BerserkSkill)
                    {
                        randomUse_BerserkSkill = Random.Range(0, 3);
                    }
                    oldUse_NormalSkill = randomUse_NormalSkill;
                    oldUse_BerserkSkill = randomUse_BerserkSkill;
                    AttackFlag = true;
                }
            }
            //攻撃状態
            else
            {
                nowStayTime = 0;
                //バーサク状態だったら中心に移動する
                if (isBerserk)
                {
                    float Speed = 0.05f;
                    //現在座標値を滑らかに目標座標値へ遷移
                    Method.SmoothChangeEx(ref nowPosition.x, 0, Speed);
                    Method.SmoothChangeEx(ref nowPosition.z, 0, Speed);
                    //座標値反映
                    this.transform.position = nowPosition;
                }
            }
            isShield = true;
        }
        else
        {
            isShield = false;
        }
    }

	/// <summary>
	/// ダウン状態
	/// </summary>
	void Down()
	{
		if(isDown)
		{
			//エフェクト関連初期化
			isDashEffect = false;
			AttackPoints[0].particleSystem.startSize = nowEffectSize = EfectSize_Min;
			AttackPoints[0].light.intensity = nowLightIntensity = EfectLightIntensity_Min;
			AttackPoints[1].particleSystem.startSize = nowEffectSize = EfectSize_Min;
			AttackPoints[1].light.intensity = nowLightIntensity = EfectLightIntensity_Min;
			//ダウン時間経過
			DownTime += Method.GameTime();
			//5秒間ダウン
			if(DownTime > 300 || this.transform.position.y < -20)
			{
                DownTime = 301;
				//上昇
				if(this.transform.position.y < 10f)
				{
					this.rigidbody.AddForce(this.transform.TransformDirection(Vector3.up).normalized * 1, ForceMode.VelocityChange);
				}
				else
				{
					isDown = false;
					//バーサクモードに切り替え
					isBerserk = true;
				}
			}
			//下に落ちる
			else if(!isGround)
			{
				this.rigidbody.AddForce(this.transform.TransformDirection(Vector3.down).normalized * 5, ForceMode.VelocityChange);
			}
		}
		else
		{
			DownTime = 0;
		}
	}

    #region ノーマルスキル
    /// <summary>
    /// ノーマルスキル・ハイラッシュ攻撃パターン
    /// </summary>
    void HighRash()
    {
		float maxScale = 50f;
		float maxDis = 600f;
        AttackIconObject.GetComponent<AttackIconScript>().setAttackIcon("sword");
        if (AttackFlag)
        {       
            //スキル発動
            isDashEffect = true;
            //中心からの距離
            float dis = Vector3.Distance(Vector3.zero + new Vector3(0, 20, 0), this.transform.position);
            //距離が遠いほど小さく、近いほど大きくする
            float flowScale = maxScale * (dis / maxDis);
            Vector3 flowScaleVector = new Vector3(maxScale, maxScale, maxScale) - new Vector3(flowScale, flowScale, flowScale);
            this.transform.localScale = flowScaleVector;
            //スキル使用
            himeSkill = new HimeSkill(this.transform.position, this.transform.rotation, this.gameObject);
            himeSkill.HighRash();
        }
        //一定時間経つと通常移動に戻る
        usingSkillTime += Method.GameTime();
        if (usingSkillTime > useHighRashTime)
        {
			this.transform.localScale = new Vector3(maxScale, maxScale, maxScale);
            isDashEffect = false;
            usingSkillTime = 0;
            AttackFlag = false;
            //HP半分以下だったらバーサクモードへ
            if (isHarf)
            {
                isBerserk = !isBerserk;
            }
        }
    }

    /// <summary>
    /// ノーマルスキル・ビッグメテオ攻撃パターン
    /// </summary>
    void BigMeteo()
    {
        isDashEffect = false;
        AttackIconObject.GetComponent<AttackIconScript>().setAttackIcon("bow");
        //移動
        //TODO:移動処理（必要なら）
        if (AttackFlag)
        {
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
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(PlayerObject.transform.position - this.transform.transform.position), 0.07f);
            this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);
        }
        //一定時間経つと通常移動に戻る
        usingSkillTime += Method.GameTime();
        if (usingSkillTime > useBigMeteoTime)
        {
            usingSkillTime = 0;
            //エフェクト関連初期化
            AttackPoints[0].particleSystem.startSize = nowEffectSize = EfectSize_Min;
            AttackPoints[0].light.intensity = nowLightIntensity = EfectLightIntensity_Min;
            AttackPoints[1].particleSystem.startSize = nowEffectSize = EfectSize_Min;
            AttackPoints[1].light.intensity = nowLightIntensity = EfectLightIntensity_Min;
            AttackFlag = false;

            //HP半分以下だったらバーサクモードへ
            if (isHarf)
            {
                isBerserk = !isBerserk;
            }
        }
    }

    /// <summary>
    /// ノーマルスキル・フォトンレーザー攻撃パターン
    /// </summary>
    void PhotonLaser()
    {
        isDashEffect = false;
        AttackIconObject.GetComponent<AttackIconScript>().setAttackIcon("magic");
        //チャージ行動（？）
        //TODO:チャージ行動処理

        if (AttackFlag)
        {
            //両羽から照射
            if (PhotonLaserNum < 50)
            {
                foreach (GameObject i in AttackPoints)
                {
                    //Debug.Log("Photon");
                    //スキル発動
                    himeSkill = new HimeSkill(i.transform.position, i.transform.rotation, i);
                    himeSkill.PhotonLaser();
                    PhotonLaserNum++;
                }
            }
        }
        //一定時間経つと通常移動に戻る
        usingSkillTime += Method.GameTime();
        if (usingSkillTime > usePhotonLaserTime)
        {
            PhotonLaserNum = 0;
            usingSkillTime = 0;
            AttackFlag = false;
            //HP半分以下だったらバーサクモードへ
            if (isHarf)
            {
                isBerserk = !isBerserk;
            }
        }
    }
#endregion

    #region バーサクスキル
    /// <summary>
    /// バーサクスキル・ハイトルネード攻撃パターン
    /// </summary>
    void HighTornado()
    {
        if (AttackFlag)
        {
            //Debug.Log("ハイトルネード");
            isDashEffect = false;
            himeSkill = new HimeSkill(this.transform.position, this.transform.rotation, this.gameObject);
            himeSkill.HighTornado();
        }
        //スキルが終了していたら
        if (himeSkill.getEndSkill())
        {
			isBerserk = false;
            AttackFlag = false;
        }
    }

    /// <summary>
    /// バーサクスキル・ビッグマイン攻撃パターン
    /// </summary>
    void BigMine()
    {
        if (AttackFlag)
        {
            //Debug.Log("ビッグマイン");
            isDashEffect = false;
            himeSkill = new HimeSkill(this.transform.position, this.transform.rotation, this.gameObject);
            himeSkill.BigMine();
        }
        //スキルが終了していたら
        if (himeSkill.getEndSkill())
        {
			isBerserk = false;
            AttackFlag = false;
        }
    }

    /// <summary>
    /// バーサクスキル・オメガビーム攻撃パターン
    /// </summary>
    void OmegaBeam()
    {
        if (AttackFlag)
        {
            //Debug.Log("オメガビーム");
            isDashEffect = false;
            himeSkill = new HimeSkill(this.transform.position, this.transform.rotation, this.gameObject);
            himeSkill.OmegaLaser();
            //プレイヤーの方向を向く
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(PlayerObject.transform.position - this.transform.transform.position), 0.07f);
            this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);
        }
        //スキルが終了していたら
        if (himeSkill.getEndSkill())
        {
			isBerserk = false;
            AttackFlag = false;
        }
    }
    #endregion

    /// <summary>
    /// アニメーション管理
    /// </summary>
    void AnimationController()
    {
        // 参照用のステート変数にBase Layer (0)の現在のステートを設定する
        currentBaseState = this.animator.GetCurrentAnimatorStateInfo(0);
        animator.SetBool("AttackFlag", AttackFlag);

		if(!isDown)
		{
            shieldController = ShieldObject.GetComponent<ShieldController>();
	        //攻撃態勢だったら
	        if (currentBaseState.nameHash == attack_02State)
	        {
#if skillDebug
	            //ノーマルスキル
	            if (useBigMeteo) BigMeteo();
	            else if (usePhotonLaser) PhotonLaser();
	            else if (useHighRash) HighRash();

	            //バーサクスキル
	            else if (useHighTornado) HighTornado();
	            else if (useBigMine) BigMine();
	            else if (useOmegaBeam) OmegaBeam();
#else
	            //バーサク状態か
	            if (!isBerserk)
	            {
	                //ノーマルスキル
	                switch (randomUse_NormalSkill)
	                {
	                    case 0:
                            shieldController.setToShieldCollision("");
	                        BigMeteo();
	                        break;
	                    case 1:
                            shieldController.setToShieldCollision("Arrow");
	                        PhotonLaser();
	                        break;
	                    case 2:
                            shieldController.setToShieldCollision("MagicBall");
	                        HighRash();
	                        break;
                    }
	            }
	            else
	            {
                    float dis = Vector2.Distance(this.transform.position, new Vector2(0, 0));
                    //Debug.Log(dis);
                    if (dis <= 10)
                    {
                        //バーサクスキル
                        switch (randomUse_BerserkSkill)
                        {
                            case 0:
                                shieldController.setToShieldCollision("");
                                HighTornado();
                                break;
                            case 1:
                                shieldController.setToShieldCollision("");
                                BigMine();
                                break;
                            case 2:
                                shieldController.setToShieldCollision("");
                                OmegaBeam();
                                break;
                        }
                    }
	            }
	#endif
	        }
	        else
	        {
                shieldController.setToShieldCollision("");
	            himeSkill = new HimeSkill(this.transform.position, this.transform.rotation, this.gameObject);
	        }
		}
    }

	void OnTriggerEnter(Collider collider)
	{
		//Debug.Log(collider.name);
		if(collider.name == "Floor")
		{
			isGround = true;
		}
	}

	void OnTriggerExit(Collider collider)
	{
		isGround = false;
	}

	/// <summary>
	/// GUI表示
	/// </summary>
	void OnGUI()
	{
        //GUIStyle guistyle = new GUIStyle();
        //guistyle.fontSize = 64;
        //guistyle.normal.textColor = Color.red;
        //GUI.Label( new Rect(Screen.width/2f, 0, 200, 200), "姫HP:" + this.status.HP , guistyle );
	}

    /// <summary>
    /// 姫の現在のHPを得る
    /// </summary>
    /// <returns></returns>
    public int getNowHP()
    {
        return this.GetComponent<EnemyStatusManager>().getStatus().HP;
    }
}