using UnityEngine;
using System.Collections;
using StatusClass;
using SkillClass;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// メインカメラ
    /// </summary>
    private GameObject mainCamera;
    /// <summary>
    /// ボス戦かどうか
    /// </summary>
    [SerializeField]
    private bool isBossBattle = false;
    /// <summary>
    /// ターゲットオブジェクト
    /// </summary>
    private GameObject TargetObject;
    /// <summary>
    /// 道中スピード
    /// </summary>
    [SerializeField]
    [Range(0, 100)]
    private float RoadSpeed = 0;
    /// <summary>
    /// 通常移動スピード
    /// </summary>
    [SerializeField]
    [Range(0, 100)]
    private float NormalSpeed = 0;
    /// <summary>
    /// 攻撃移動スピード
    /// </summary>
    [SerializeField]
    [Range(0, 100)]
    private float AttackSpeed = 0;
    /// <summary>
    /// 魔法ボール
    /// </summary>
    private GameObject MagicBallObject;
    /// <summary>
    /// 矢
    /// </summary>
    private GameObject ArrowObject;
    /// <summary>
    /// 魔法/弓発射位置
    /// </summary>
    [SerializeField]
    private GameObject ShotPoint;
    /// <summary>
    /// アニメーションコントローラ
    /// </summary>
    private Animator animator;
    /// <summary>
    /// 魔法を撃っているかどうか
    /// </summary>
    private bool isShotMagic = false;
    /// <summary>
    /// 魔法を一発打ったか(短時間に連続して出さないようにする)
    /// </summary>
    private bool isOneShotMagic = false;
    /// <summary>
    /// 矢を撃っているかどうか
    /// </summary>
    private bool isShotArrow = false;
    /// <summary>
    /// 矢を一発打ったか(短時間に連続して出さないようにする)
    /// </summary>
    private bool isOneShotArrow = false;
    /// <summary>
    /// 剣を振っているかどうか
    /// </summary>
    private bool isAttackSword = false;
    /// <summary>
    /// 剣スキルを一発打ったか(短時間に連続して出さないようにする)
    /// </summary>
    private bool isOneShotSword = false;
    /// <summary>
    /// 移動しているかどうか
    /// </summary>
    private bool isMove = false;
    /// <summary>
    /// base layerで使われる、アニメーターの現在の状態の参照
    /// </summary>
    private AnimatorStateInfo currentBaseState;
    //各アニメーションへのステート
    static int idleState = Animator.StringToHash("Base Layer.Idle");
    static int runState = Animator.StringToHash("Base Layer.Run");
    static int swordRunState = Animator.StringToHash("Base Layer.Sword_Run");
    static int sword_01State = Animator.StringToHash("Base Layer.Sword");
	static int sword_02State = Animator.StringToHash ("Base Layer.Sword_02");
    static int magic_01State = Animator.StringToHash("Base Layer.Magic_01");
    static int magic_02State = Animator.StringToHash("Base Layer.Magic_02");
    static int arrow_01State = Animator.StringToHash("Base Layer.Arrow_01");
    static int arrow_02State = Animator.StringToHash("Base Layer.Arrow_02");
	static int LvUpState = Animator.StringToHash("Base Layer.Take 0001");
	static int DamageState = Animator.StringToHash("Base Layer.Damage");
    /// <summary>
    /// Statusクラス
    /// </summary>
    public Status status;
    /// <summary>
    /// 死亡フラグ
    /// </summary>
    private bool deadFlag = false;
	/// <summary>
	/// ダメージを受けたかの判定
	/// </summary>
	private bool isDamage = false;
	/// <summary>
	/// LvUPのためのフラグ
	/// </summary>
	private bool LvUp = false;
	/// <summary>
	/// スキルクラス
	/// </summary>
	private Skill skill;

	/// <summary>
	/// 武器/剣
	/// </summary>
	private GameObject Weapon_Sword;
	/// <summary>
	/// 武器/杖
	/// </summary>
	private GameObject Weapon_Rod;
	/// <summary>
	/// 武器/弓
	/// </summary>
	private GameObject Weapon_Bow;
    /// <summary>
    /// 新しい座標
    /// </summary>
    private Vector3 newPos;
    /// <summary>
    /// 古い座標
    /// </summary>
    private Vector3 oldPos;
	/// <summary>
	/// ヒットエフェクト
	/// </summary>
	private GameObject HitEffect;
	/// <summary>
	/// 歩行エフェクト
	/// </summary>
	private GameObject RunSmokeEffect;
    /// <summary>
    /// マウスボタンの列挙体
    /// </summary>
    private enum MouseButtonEnum
    {
        RIGHT_BUTTON = 0,
        LEFT_BUTTON,
        CENTER_BUTTON,
    }
    /// <summary>
    /// マウスボタンの構造体
    /// </summary>
    private struct MouseButtonStruct
    {
        public bool right;
        public bool left;
        public bool center;
        public bool rightDown;
        public bool leftDown;
        public bool centerDown;
    }
    /// <summary>
    /// マウスボタン
    /// </summary>
    private MouseButtonStruct mouseButton;
    /// <summary>
    /// 武器の列挙体
    /// </summary>
    private enum WeaponEnum
    {
        SWORD = 0,
        MAGIC,
        BOW,
    }
    /// <summary>
    /// 現在選択中の武器
    /// </summary>
    private int nowWeapon = 0;

    void Awake()
	{
        if (GameObject.FindGameObjectWithTag("Boss") != null)
        {
            TargetObject = GameObject.FindGameObjectWithTag("Boss");
        }
        else if (GameObject.FindGameObjectWithTag("Hime") != null)
        {
            TargetObject = GameObject.FindGameObjectWithTag("Hime");
        }
        MagicBallObject = Resources.Load("Prefab/MagicBall") as GameObject;
        ArrowObject = Resources.Load("Prefab/Arrow") as GameObject;
        animator = this.gameObject.GetComponent<Animator>();

		Weapon_Sword = GameObject.FindGameObjectWithTag("Weapon_Sword");
		Weapon_Rod = GameObject.FindGameObjectWithTag("Weapon_Rod");
		Weapon_Bow = GameObject.FindGameObjectWithTag("Weapon_Bow");

		HitEffect = Resources.Load("Prefab/HitEffect") as GameObject;

		RunSmokeEffect = Resources.Load ("Prefab/RunSmoke") as GameObject;

        mainCamera = GameObject.Find("MainCamera");
    }

    // Use this for initialization
    void Start()
    {
        StatusManager statusManager = new StatusManager();
        status = new Status(
            statusManager.getLoadStatus().LV,
            statusManager.getLoadStatus().EXP,
            statusManager.getLoadStatus().HP,
            statusManager.getLoadStatus().MP,
            statusManager.getLoadStatus().NAME);
		Weapon_Sword.renderer.enabled = false;
		Weapon_Rod.renderer.enabled = false;
		Weapon_Bow.renderer.enabled = false;
    }

    void Update()
    {
        //マウスイベント
        MouseEvent();
        //武器切り替え
        WeaponChange();
        //アニメーション管理
        AnimationController();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //移動
        if (!isAttackSword &&
            !isShotMagic &&
            !isShotArrow )
        {
			oldPos = this.transform.position;
            Move();
        }
        //攻撃
        Attack();
    }

	void LateUpdate()
	{
		newPos = this.transform.position;
		//Debug.Log (newPos - oldPos);
		//Debug.Log ("newPos = " + newPos + "\noldPos = " + oldPos);
	}

    /// <summary>
    /// 移動
    /// </summary>
    void Move()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");
        animator.SetFloat("Horizontal", inputH);
        isMove = false;
        ////ボス戦だったら
        //if (isBossBattle)
        //{
        //    if (inputH != 0.0f || inputV != 0.0f)
        //    {
        //        isMove = true;
        //    }
        //    //敵方向を向く
        //    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(TargetObject.transform.position - this.transform.transform.position), 0.07f);
        //    this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);

        //    //前
        //    if (Input.GetKey(KeyCode.W))
        //    {
        //        isMove = true;
        //        rigidbody.AddForce(transform.TransformDirection(Vector3.forward).normalized * NormalSpeed, ForceMode.VelocityChange);
        //        //rigidbody.velocity = transform.TransformDirection(Vector3.forward).normalized * NormalSpeed;
        //    }
        //    //後ろ
        //    else if (Input.GetKey(KeyCode.S))
        //    {
        //        isMove = true;
        //        rigidbody.AddForce(transform.TransformDirection(Vector3.back).normalized * NormalSpeed, ForceMode.VelocityChange);
        //        //rigidbody.velocity = transform.TransformDirection(Vector3.back).normalized * NormalSpeed;
        //    }
        //    //左
        //    if (Input.GetKey(KeyCode.A))
        //    {
        //        isMove = true;
        //        rigidbody.AddForce(transform.TransformDirection(Vector3.left).normalized * NormalSpeed, ForceMode.VelocityChange);
        //        //rigidbody.velocity = transform.TransformDirection(Vector3.left).normalized * NormalSpeed;
        //    }
        //    //右
        //    else if (Input.GetKey(KeyCode.D))
        //    {
        //        isMove = true;
        //        rigidbody.AddForce(transform.TransformDirection(Vector3.right).normalized * NormalSpeed, ForceMode.VelocityChange);
        //        //rigidbody.velocity = transform.TransformDirection(Vector3.right).normalized * NormalSpeed;
        //    }
        //}
        ////道中だったら
        //else
        //{
            //if (inputH != 0.0f || inputV != 0.0f)
            //{
            //    isMove = true;
            //}
            ////進行方向取得
            //Vector3 inputVec = new Vector3(inputH, 0.0f, inputV).normalized;
            ////キャラ回転
            //if (!inputVec.Equals(Vector3.zero))
            //{
            //    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(inputVec), 0.2f);
            //    rigidbody.AddForce(inputVec * RoadSpeed * 10.0f, ForceMode.VelocityChange);
            //    //this.transform.Translate(Vector3.forward * RoadSpeed);
            //}

            Vector3 forward = mainCamera.GetComponent<CameraController>().getCameraDirection(Vector3.forward).normalized;
            Vector3 back = mainCamera.GetComponent<CameraController>().getCameraDirection(Vector3.back).normalized;
            Vector3 right = mainCamera.GetComponent<CameraController>().getCameraDirection(Vector3.right).normalized;
            Vector3 left = mainCamera.GetComponent<CameraController>().getCameraDirection(Vector3.left).normalized;
            float rotSpeed = 0.2f;
            //前
            if (Input.GetKey(KeyCode.W))
            {
                isMove = true;
                rigidbody.AddForce(forward * NormalSpeed, ForceMode.VelocityChange);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(forward), rotSpeed);
            }
            //後ろ
            else if (Input.GetKey(KeyCode.S))
            {
                isMove = true;
                rigidbody.AddForce(back * NormalSpeed, ForceMode.VelocityChange);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(back), rotSpeed);
            }
            //左
            if (Input.GetKey(KeyCode.A))
            {
                isMove = true;
                rigidbody.AddForce(left * NormalSpeed, ForceMode.VelocityChange);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(left), rotSpeed);
            }
            //右
            else if (Input.GetKey(KeyCode.D))
            {
                isMove = true;
                rigidbody.AddForce(right * NormalSpeed, ForceMode.VelocityChange);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(right), rotSpeed);
            }
            
            //rigidbody.AddForce(direction * NormalSpeed, ForceMode.VelocityChange);
        //}
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    void Attack()
    {
        skill = new Skill(ShotPoint.transform.position, this.transform.rotation, "Boss");
        currentBaseState = this.animator.GetCurrentAnimatorStateInfo(0);
        if ((currentBaseState.nameHash != LvUpState || 
            currentBaseState.nameHash != DamageState) &&
            mouseButton.right)
        {
            switch (nowWeapon)
            {
                case (int)WeaponEnum.SWORD:
                    isAttackSword = true;
                    break;
                case (int)WeaponEnum.MAGIC:
                    isShotMagic = true;
                    break;
                case (int)WeaponEnum.BOW:
                    isShotArrow = true;
                    break;
            }

            ////剣
            //if (Input.GetKeyDown(KeyCode.J))
            //{
            //    isAttackSword = true;
            //}
            ////魔法
            //else if (Input.GetKeyDown(KeyCode.I))
            //{
            //    isShotMagic = true;
            //}
            ////弓
            //else if (Input.GetKeyDown(KeyCode.O))
            //{
            //    isShotArrow = true;
            //}
        }
    }

    /// <summary>
    /// アニメーション管理
    /// </summary>
    void AnimationController()
    {
        // 参照用のステート変数にBase Layer (0)の現在のステートを設定する
        currentBaseState = this.animator.GetCurrentAnimatorStateInfo(0);
        //Text HP = GameObject.Find ("HP").GetComponent<Text> ();
        //Text MP = GameObject.Find ("MP").GetComponent<Text> ();
        //Text Lv = GameObject.Find ("LV").GetComponent<Text> ();

        //HP.text = this.status.HP.ToString();
        //MP.text = this.status.MP.ToString ();
        //Lv.text = this.status.LEV.ToString ();
		
        //Debug.Log(animator.GetBool("isAttackSword"));
		

        //立ちモーションの時
        if (currentBaseState.nameHash == idleState)
        {
			//ダメージフラグを初期化
			isDamage = false;
			//LvUpフラグを初期化
			LvUp = false;
			//武器表示を初期化
			Weapon_Sword.renderer.enabled = false;
			Weapon_Rod.renderer.enabled = false;
			Weapon_Bow.renderer.enabled = false;
        }
        //走りモーションの時
        else if (currentBaseState.nameHash == runState)
        {
			//武器表示を初期化
			Weapon_Sword.renderer.enabled = false;
			Weapon_Rod.renderer.enabled = false;
			Weapon_Bow.renderer.enabled = false;
        }
        //剣モーションの時
        else if (currentBaseState.nameHash == sword_01State)
        {
            isOneShotSword = false;
			//武器表示を設定
			Weapon_Sword.renderer.enabled = true;
			Weapon_Rod.renderer.enabled = false;
			Weapon_Bow.renderer.enabled = false;
            //攻撃フラグを初期化
            isAttackSword = false;
        }
		//剣モーション02の時
		else if (currentBaseState.nameHash == sword_02State)
		{
            if (!isOneShotSword)
            {
                isOneShotSword = true;
                //skill.SwordSlash();
            }
		}
        //魔法モーション(_01)の時
        else if (currentBaseState.nameHash == magic_01State)
        {
            isOneShotMagic = false;
			//武器表示を設定
			Weapon_Sword.renderer.enabled = false;
			Weapon_Rod.renderer.enabled = true;
			Weapon_Bow.renderer.enabled = false;
            //攻撃フラグを初期化
            isShotMagic = false;

        }
        //魔法モーション(_02)の時
        else if (currentBaseState.nameHash == magic_02State)
        {
            if (!isOneShotMagic)
            {
                isOneShotMagic = true;
                if (isBossBattle)
                {
                    GameObject magic = Instantiate(MagicBallObject, ShotPoint.transform.position, Quaternion.LookRotation(TargetObject.transform.position - this.transform.position)) as GameObject;
                    magic.GetComponent<MagicController>().setTargetObject(TargetObject);
                }
                else
                {
                    Instantiate(MagicBallObject, ShotPoint.transform.position, this.transform.rotation);
                }
                MagicController.EnemyDamage = this.status.Magic_Power;
				//skill.Meteo ();
            }
        }
        //弓モーション(_01)の時
        else if (currentBaseState.nameHash == arrow_01State)
        {
            isOneShotArrow = false;
			//武器表示を設定
			Weapon_Sword.renderer.enabled = false;
			Weapon_Rod.renderer.enabled = false;
			Weapon_Bow.renderer.enabled = true;
            //攻撃フラグを初期化
            isShotArrow = false;
        }
        //弓モーション(_02)の時
        else if (currentBaseState.nameHash == arrow_02State)
        {
            if (!isOneShotArrow)
            {
                isOneShotArrow = true;
                if (isBossBattle)
                {
                    GameObject arrow = Instantiate(ArrowObject, ShotPoint.transform.position, Quaternion.LookRotation(TargetObject.transform.position - this.transform.position)) as GameObject;
                    arrow.GetComponent<BowController>().setTargetObject(TargetObject);
                }
                else
                {
                    Instantiate(ArrowObject, ShotPoint.transform.position, this.transform.rotation);
                }
                BowController.EnemyDamage = this.status.BOW_POW;
				//skill.SpreadArrow ();
            }
        }
        animator.SetBool("isMove", isMove);
        //animator.SetBool("isAttackSwordRun", isAttackSwordRun);
        animator.SetBool("isAttackSword", isAttackSword);
        animator.SetBool("isShotMagic", isShotMagic);
        animator.SetBool("isShotArrow", isShotArrow);
        animator.SetBool("DeadFlag", deadFlag);
        animator.SetBool("isDamage", isDamage);
        animator.SetBool("isLvUp", LvUp);
    }

	/// <summary>
	/// アニメーションイベント
	/// </summary>
	void RunSeTiming()
	{
		//Debug.Log("Se Play");
		Instantiate (RunSmokeEffect, this.transform.position, this.transform.rotation);
	}

    /// <summary>
    /// マウスイベント
    /// </summary>
    void MouseEvent()
    {
        mouseButton.right = Input.GetMouseButton((int)MouseButtonEnum.RIGHT_BUTTON);
        mouseButton.left = Input.GetMouseButton((int)MouseButtonEnum.LEFT_BUTTON);
        mouseButton.center = Input.GetMouseButton((int)MouseButtonEnum.CENTER_BUTTON);
        mouseButton.rightDown = Input.GetMouseButtonDown((int)MouseButtonEnum.RIGHT_BUTTON);
        mouseButton.leftDown = Input.GetMouseButtonDown((int)MouseButtonEnum.LEFT_BUTTON);
        mouseButton.centerDown = Input.GetMouseButtonDown((int)MouseButtonEnum.CENTER_BUTTON);
    }

    /// <summary>
    /// 武器切り替え
    /// </summary>
    void WeaponChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            nowWeapon = (int)WeaponEnum.SWORD;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            nowWeapon = (int)WeaponEnum.MAGIC;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            nowWeapon = (int)WeaponEnum.BOW;
        }
        //Debug.Log(nowWeapon);
    }

    /// <summary>
    /// 何かに当たったら
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.collider.name);
        if (collision.collider.name == "toBossEventWall")
        {
            Application.LoadLevel("Boss");
        }
    }

    /// <summary>
    /// 何かに当たり続けたら
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerStay(Collider collider)
    {
        currentBaseState = this.animator.GetCurrentAnimatorStateInfo(0);
        
		if (collider.gameObject.CompareTag("Enemy") && currentBaseState.nameHash == sword_02State)
        {
			var enemy = collider.gameObject.GetComponent<EnemyScript>();
			enemy.Damage(this.status.Sword_Power);
        }
		else if (collider.gameObject.CompareTag("Hime") && currentBaseState.nameHash == sword_02State)
		{
			Instantiate(HitEffect, this.transform.position, this.transform.rotation);
			var hime = collider.gameObject.GetComponent<RastBossController>();
			hime.Damage(this.status.Sword_Power);
		}
    }

    /// <summary>
    /// 外部から経験値取得を呼び出す関数
    /// </summary>
    /// <param name="exp">経験値</param>
    public void GetExp(int exp)
    {
        this.status.EXP += exp;
        if (this.status.EXP >= this.status.ExpLimit)
        {
            status.LevUp();
            LvUp = true;
            isMove = false;
            //攻撃フラグを初期化
            isAttackSword = false;
            isShotMagic = false;
            isShotArrow = false;
        }
    }

    /// <summary>
    /// 外部からダメージを呼び出す関数
    /// </summary>
    /// <param name="val">ダメージ値</param>
    public void Damage(int val)
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        this.status.HP -= val;
		if (this.status.HP < 0) {
			this.status.HP = 0;		
		}
        if (this.status.HP < 0 && deadFlag == false) 
		{
			this.deadFlag = true;
		}
        else
        {
            this.isDamage = true;
        }
    }

    /// <summary>
    /// プレイヤーの移動差分を返す
    /// </summary>
    /// <returns></returns>
    public Vector3 getVectorDistance()
    {
		//Debug.Log (oldPos - newPos);
        return newPos - oldPos;
    }

    /// <summary>
    /// プレイヤーのステータス値を得る
    /// </summary>
    /// <returns></returns>
    public Status getPlayerStatus()
    {
        return status;
    }

    /// <summary>
    /// 現在使用中の武器を得る
    /// </summary>
    /// <returns></returns>
    public int getNowWeapon()
    {
        return nowWeapon;
    }
}