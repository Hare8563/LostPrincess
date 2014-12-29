using UnityEngine;
using System.Collections;

namespace HimeSkillClass
{
	public class HimeSkill : MonoBehaviour {

        /// <summary>
        /// リソース読み込みを行ったか
        /// </summary>
        private static bool isAwake = false;
		/// <summary>
		/// インスタンス生成位置
		/// </summary>
		private Vector3 EmmitPosition;
		/// <summary>
		/// インスタンス生成角度
		/// </summary>
		private Quaternion EmmitRotation;
		/// <summary>
		/// インスタンスを生成しているオブジェクト
		/// </summary>
		private GameObject EmmitObject;
		/// <summary>
		/// ターゲットオブジェクト
		/// </summary>
		private static GameObject TargetObject;
		/// <summary>
		/// ターゲットに対する回転
		/// </summary>
		private Quaternion toTargetRotation;

        /// <summary>
        /// ノーマルスキル・ハイラッシュのボムオブジェクト
        /// </summary>
        private static GameObject BombObject;
		/// <summary>
		/// ノーマルスキル・ビッグメテオのオブジェクト
		/// </summary>
		private static GameObject BigMeteoObject;
        /// <summary>
        /// ノーマルスキル・フォトンレーザーのオブジェクト
        /// </summary>
        private static GameObject PhotonLazerObject;

        /// <summary>
        /// 姫オブジェクト
        /// </summary>
        private GameObject HimeObject;

        /// <summary>
        /// スキルが終了したか
        /// </summary>
        private bool isEndSkill = false;

        /// <summary>
        /// ボムを投下するタイミング
        /// </summary>
        private static float BombTiming = 0;

        #region バーサクスキル・ハイトルネードの変数定義
        /// <summary>
        /// バーサクスキル・ハイトルネードで回転する角度
        /// </summary>
        private static float Tornado_Angle = 0;
        /// <summary>
        /// バーサクスキル・ハイトルネードで回転する速度
        /// </summary>
        private static float Tornado_Speed = 0;
        /// <summary>
        /// バーサクスキル・ハイトルネードをキープする時間
        /// </summary>
        private static float Tornado_KeepTime = 0;
        /// <summary>
        /// バーサクスキル・ハイトルネードの速度を減速するか
        /// </summary>
        private static bool Tornado_isSpeedDown = false;
        /// <summary>
        /// バーサクスキル・ハイトルネードのエフェクトを生成したかどうか
        /// </summary>
        private static bool Tornado_isCreate = false;
        /// <summary>
        /// バーサクスキル・ハイトルネードのエフェクトオブジェクト
        /// </summary>
        private static GameObject Tornado_EffectObject;
        /// <summary>
        /// バーサクスキル・ハイトルネードの最大速度
        /// </summary>
        private static float Tornado_MaxSpeed = 40.0f;
        /// <summary>
        /// バーサクスキル・ハイトルネードの最小速度
        /// </summary>
        private static float Tornado_MinSpeed = 0.0f;
        /// <summary>
        /// バーサクスキル・ハイトルネードの加算回転角度
        /// </summary>
        private static float Tornado_AddTakeAngleSpeed = 0.1f;
        /// <summary>
        /// バーサクスキル・ハイトルネードのエフェクトオブジェクトの子オブジェクト
        /// </summary>
        private static GameObject Tornado_EffectChildObject;
        /// <summary>
        /// バーサクスキル・ハイトルネードのパーティクルを生成する数
        /// </summary>
        private static float Tornado_EmmitCount = 0;
        #endregion

        #region バーサクスキル・ビッグマインの変数定義
        /// <summary>
        /// オブジェクト生成のタイミング
        /// </summary>
        private static float BigMine_Timing = 0;
        /// <summary>
        /// ビッグマインブジェクト
        /// </summary>
        private static GameObject BigMineObject;
        /// <summary>
        /// ビッグマインを出た個数
        /// </summary>
        private static int BigMine_CreateCount = 0;
        #endregion

        #region バーサクスキル・オメガビーム
        /// <summary>
        /// オメガビームエフェクトを生成したかどうか
        /// </summary>
        private static bool OmegaBeam_isCreate = false;
        /// <summary>
        /// オメガビームオブジェクト
        /// </summary>
        private static GameObject OmegaBeamObject;
        /// <summary>
        /// オメガビーム発生位置オブジェクト
        /// </summary>
        private static GameObject OmegaBeamEmmitObject;
        /// <summary>
        /// オメガビームのクローン
        /// </summary>
        private static GameObject OmegaBeamClone;
        #endregion

        /// <summary>
		/// スキルのコンストラクタ
		/// </summary>
		/// <param name="_EmmitPosition">生成位置</param>
		/// <param name="_EmmitRotation">生成角度</param>
		public HimeSkill(Vector3 _EmmitPosition, Quaternion _EmmitRotation)
		{
			EmmitPosition = _EmmitPosition;
			EmmitRotation = _EmmitRotation;
            if (!isAwake)
            {
                //Debug.Log("Awake");
                Awake();
            }
            Start();
		}

		/// <summary>
		/// スキルのコンストラクタ
		/// </summary>
		/// <param name="_EmmitPosition">生成位置</param>
		/// <param name="_EmmitRotation">生成角度</param>
		/// <param name="_EmmitObject">生成しているオブジェクト</param>
		public HimeSkill(Vector3 _EmmitPosition, Quaternion _EmmitRotation, GameObject _EmmitObject)
		{
			EmmitPosition = _EmmitPosition;
			EmmitRotation = _EmmitRotation;
			EmmitObject = _EmmitObject;
            if (!isAwake)
            {
                //Debug.Log("Awake");
                Awake();
            }
            Start();
		}

		void Awake()
		{
            isAwake = true;
            Tornado_EffectObject = Resources.Load("Prefab/TornadoEffect") as GameObject;
            TargetObject = GameObject.FindGameObjectWithTag("Player");
            BombObject = Resources.Load("Prefab/Bomb") as GameObject;
            BigMeteoObject = Resources.Load("Prefab/BigMeteoBall") as GameObject;
            PhotonLazerObject = Resources.Load("Prefab/PhotonLazerEmmiter") as GameObject;
            BigMineObject = Resources.Load("Prefab/BigMine") as GameObject;
            OmegaBeamObject = Resources.Load("Prefab/OmegaBeam") as GameObject;
            OmegaBeamEmmitObject = GameObject.Find("OmegaBeamEmmit");
            HimeObject = GameObject.FindGameObjectWithTag("Hime");
		}

		// Use this for initialization
        void Start()
        {
            //Debug.Log("Start");
            toTargetRotation = Quaternion.LookRotation(TargetObject.transform.position - EmmitPosition);
        }
		
		// Update is called once per frame
		void Update () {

		}

        #region ノーマル
        /// <summary>
		/// ノーマルスキル・ハイラッシュ
		/// </summary>
		public void HighRash()
		{
            float Speed = 30.0f;
            float maxDis = 600f;
            //前進
            EmmitObject.rigidbody.AddForce(EmmitObject.transform.TransformDirection(Vector3.forward) * Speed, ForceMode.VelocityChange);
            //サイズが最小になったら
            if (EmmitObject.transform.localScale.x < 0 ||
                EmmitObject.transform.localScale.y < 0 ||
                EmmitObject.transform.localScale.z < 0)
            {
                //maxDisの距離のままランダム位置に再配置
                float angle = Random.Range(0f, 360f) * Mathf.PI / 180;
                float x = maxDis * Mathf.Cos(angle);
                float z = maxDis * Mathf.Sin(angle);
                EmmitObject.transform.position = new Vector3(x, EmmitObject.transform.position.y, z);
                //プレイヤーの方向を向く
                EmmitObject.transform.LookAt(TargetObject.transform.position + new Vector3(0, 20, 0));
            }

            //下にステージが存在したら背中からボムを投下
            Vector3 down = EmmitObject.transform.TransformDirection(Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(EmmitObject.transform.position, down, out hit, 50))
            {
                if (hit.collider.name == "Floor")
                {
                    //Debug.Log(BombTiming);
                    BombTiming += Method.GameTime();
                    if (BombTiming > 10)
                    {
                        BombTiming = 0;
                        GameObject bomb = (GameObject)Instantiate(BombObject, EmmitObject.transform.position + EmmitObject.transform.TransformDirection(Vector3.back * 10.0f), EmmitObject.transform.rotation);
                        //float random = 5.0f;
                        //bomb.rigidbody.AddForce(new Vector3(Random.Range(-random, random), 0, Random.Range(-random, random)),ForceMode.Impulse);
                    }
                }
            }
            
            //Debug.Log(dis);
		}

		/// <summary>
		/// ノーマルスキル・ビッグメテオ
		/// </summary>
		public void BigMeteo()
		{
			//Debug.Log(BigMeteoObject);
			Instantiate(BigMeteoObject, EmmitPosition, toTargetRotation);
		}

		/// <summary>
		/// ノーマルスキル・フォトンレーザー
		/// </summary>
		public void PhotonLaser()
		{
			//GameObject emptyGameObject = new GameObject("Empty Game Object");
            Instantiate(PhotonLazerObject, EmmitPosition, toTargetRotation);
		}
        #endregion

        #region バーサク
        /// <summary>
		/// バーサクスキル・ハイトルネード
		/// </summary>
        public void HighTornado()
        {
			OmegaBeam_isCreate = false;
            //徐々に加速
            if (!Tornado_isSpeedDown)
            {
                isEndSkill = false;
                //最大速度まで加速
                if (Tornado_Speed < Tornado_MaxSpeed)
                {
                    Tornado_Speed += Method.GameTime() * Tornado_AddTakeAngleSpeed;
					//回転速度が最大速度の1/3以上になったら
                    if (Tornado_Speed >= Tornado_MaxSpeed / 3)
					{
						//トルネードエフェクトの生成
                        if (!Tornado_isCreate)
						{
                            //Debug.Log("Tornado_EffectChildObject");
                            Tornado_isCreate = true;
							Tornado_EffectChildObject = (GameObject)Instantiate(Tornado_EffectObject, EmmitObject.transform.position - new Vector3(0,10 ,0), EmmitObject.transform.rotation);
						}
					}
                }
                //最大まで加速したら
                else
                {
                    Tornado_Speed = Tornado_MaxSpeed;
                    Tornado_KeepTime += Method.GameTime();

                    //5秒キープしたら
                    if (Tornado_KeepTime >= 300)
                    {
                        //減速フラグ
                        Tornado_isSpeedDown = true;
                        Tornado_KeepTime = 0;
                    }
                }

                //エフェクトを生成していたら
                if (Tornado_EffectObject != null && Tornado_EffectChildObject != null)
                {
                    if (Tornado_EmmitCount < 400) Tornado_EmmitCount += Method.GameTime() * 2;
                    //トルネードエフェクトに含まれる全ての子オブジェクトを取得
                    foreach (Transform child in Tornado_EffectChildObject.transform)
                    {
                        if (child.name != "WindZone")
                        {
                            //パーティクル個数を変更
                            child.particleSystem.emissionRate = Tornado_EmmitCount;
                        }
                    }
                }
            }
            //徐々に減速
            else
            {
                if (Tornado_Speed > Tornado_MinSpeed)
                {
                    Tornado_Speed -= Method.GameTime() * Tornado_AddTakeAngleSpeed;
					//回転速度が最大速度の2/3以下になったら
                    if (Tornado_Speed <= Tornado_MaxSpeed * 2 / 3)
					{
                        if (Tornado_EmmitCount > 0) Tornado_EmmitCount -= Method.GameTime() * 2;
						//トルネードエフェクトに含まれる全ての子オブジェクトを取得
						foreach(Transform child in Tornado_EffectChildObject.transform)
						{
                            if (child.name != "WindZone")
                            {
                                //パーティクル個数を変更
                                child.particleSystem.emissionRate = Tornado_EmmitCount;
                            }
						}
					}
                }
                //最小まで減速したら
                else
                {
                    Tornado_isSpeedDown = false;
                    Tornado_Speed = Tornado_MinSpeed;
                    Tornado_isCreate = false;
                    isEndSkill = true;
                    Destroy(Tornado_EffectChildObject);
                }
            }

            //プレイヤーを引き寄せる
            if (Tornado_Speed > 10)
            {
                Vector3 forceVec = (EmmitObject.transform.position - new Vector3(0, 20, 0)) - TargetObject.transform.position;
                TargetObject.rigidbody.AddForce(forceVec * 0.14f, ForceMode.VelocityChange);
            }
            //回転角度反映
            EmmitObject.transform.eulerAngles = new Vector3(0, EmmitObject.transform.eulerAngles.y + Tornado_Speed, 0);
        }
		
		/// <summary>
		/// バーサクスキル・ビッグマイン
		/// </summary>
		public void BigMine()
		{
			OmegaBeam_isCreate = false;
            BigMine_Timing += Method.GameTime();
            if (BigMine_Timing > 90)
            {
                BigMine_Timing = 0;
                Instantiate(BigMineObject, TargetObject.transform.position + new Vector3(0, 10, 0), TargetObject.transform.rotation);
                BigMine_CreateCount++;
            }
            if (BigMine_CreateCount > 10)
            {
                isEndSkill = true;
            }
            else
            {
                isEndSkill = false;
            }
		}

		/// <summary>
		/// バーサクスキル・オメガビーム
		/// </summary>
		public void OmegaLaser()
		{
            if (!OmegaBeam_isCreate)
            {
                isEndSkill = false;
                OmegaBeam_isCreate = true;
                OmegaBeamClone = (GameObject)Instantiate(OmegaBeamObject, OmegaBeamEmmitObject.transform.position, OmegaBeamEmmitObject.transform.rotation);
            }
            //if (OmegaBeamClone != null)
            //{
            //    Debug.Log(OmegaBeamClone.GetComponent<OmegaBeam>().getEndOmegaBeam());
            //}
            if (OmegaBeamClone != null && OmegaBeamClone.GetComponent<OmegaBeam>().getEndOmegaBeam())
            {
                OmegaBeam_isCreate = false;
                isEndSkill = true;
                Destroy(OmegaBeamClone);
            }
        }
        #endregion

        /// <summary>
        /// スキルが終わったかどうかを知らせる
        /// </summary>
        public bool getEndSkill()
        {
            return isEndSkill;
        }
	}
}