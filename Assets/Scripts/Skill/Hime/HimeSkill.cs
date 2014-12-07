using UnityEngine;
using System.Collections;

namespace HimeSkillClass
{
	public class HimeSkill : MonoBehaviour {

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
		private GameObject TargetObject;
		/// <summary>
		/// ターゲットに対する回転
		/// </summary>
		private Quaternion toTargetRotation;

        /// <summary>
        /// ノーマルスキル・ハイラッシュのボムオブジェクト
        /// </summary>
        private GameObject BombObject;
		/// <summary>
		/// ノーマルスキル・ビッグメテオのオブジェクト
		/// </summary>
		private GameObject BigMeteoObject;
        /// <summary>
        /// ノーマルスキル・フォトンレーザーのオブジェクト
        /// </summary>
        private GameObject PhotonLazerObject;

        /// <summary>
        /// ボムを投下するタイミング
        /// </summary>
        private static float BombTiming = 0;
        /// <summary>
        /// バーサクスキル・ハイトルネードで回転する角度
        /// </summary>
        private static float TornadoAngle = 0;
        /// <summary>
        /// バーサクスキル・ハイトルネードで回転する速度
        /// </summary>
        private static float TornadoSpeed = 0;
        /// <summary>
        /// バーサクスキル・ハイトルネードをキープする時間
        /// </summary>
        private static float TornadoKeepTime = 0;
        /// <summary>
        /// バーサクスキル・ハイトルネードの速度を減速するか
        /// </summary>
        private static bool isSpeedDown = false;
		

		/// <summary>
		/// スキルのコンストラクタ
		/// </summary>
		/// <param name="_EmmitPosition">生成位置</param>
		/// <param name="_EmmitRotation">生成角度</param>
		public HimeSkill(Vector3 _EmmitPosition, Quaternion _EmmitRotation)
		{
			EmmitPosition = _EmmitPosition;
			EmmitRotation = _EmmitRotation;
			Awake();
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
			Awake();
			Start();
		}

		void Awake()
		{
			TargetObject = GameObject.FindGameObjectWithTag("Player");
			BigMeteoObject = Resources.Load("Prefab/BigMeteoBall") as GameObject;
            PhotonLazerObject = Resources.Load("Prefab/PhotonLazerEmmiter") as GameObject;
            BombObject = Resources.Load("Prefab/Bomb") as GameObject;
		}

		// Use this for initialization
		void Start () {
			toTargetRotation = Quaternion.LookRotation(TargetObject.transform.position - EmmitPosition);
		}
		
		// Update is called once per frame
		void Update () {

		}

		/// <summary>
		/// ノーマルスキル・ハイラッシュ
		/// </summary>
		public void HighRash()
		{
            float Speed = 30.0f;
            float maxDis = 600f;
            float maxScale = 50f;
            //中心からの距離
            float dis = Vector3.Distance(Vector3.zero + new Vector3(0, 20, 0), EmmitObject.transform.position);
            //前進
            EmmitObject.rigidbody.AddForce(EmmitObject.transform.TransformDirection(Vector3.forward) * Speed, ForceMode.VelocityChange);
            //距離が遠いほど小さく、近いほど大きくする
            float flowScale = maxScale * (dis / maxDis);
            Vector3 flowScaleVector = new Vector3(maxScale, maxScale, maxScale) - new Vector3(flowScale, flowScale, flowScale);
            EmmitObject.transform.localScale = flowScaleVector;
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
                //Debug.Log(BombTiming);
                BombTiming += Method.GameTime();
                if (BombTiming > 15)
                {
                    BombTiming = 0;
                    GameObject bomb = (GameObject)Instantiate(BombObject, EmmitObject.transform.position + EmmitObject.transform.TransformDirection(Vector3.back * 5.0f), EmmitObject.transform.rotation);
                    //float random = 5.0f;
                    //bomb.rigidbody.AddForce(new Vector3(Random.Range(-random, random), 0, Random.Range(-random, random)),ForceMode.Impulse);
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

		/// <summary>
		/// バーサクスキル・ハイトルネード
		/// </summary>
        public void HighTornado()
        {
            float MaxTornadoSpeed = 40.0f;
            float MinTornadoSpeed = 0.0f;
            float AddTakeAngleSpeed = 0.1f;
            //徐々に加速
            if (!isSpeedDown)
            {
                if (TornadoSpeed < MaxTornadoSpeed)
                {
                    TornadoSpeed += Method.GameTime() * AddTakeAngleSpeed;
                }
                //最大まで加速したら
                else
                {
                    TornadoSpeed = MaxTornadoSpeed;
                    TornadoKeepTime += Method.GameTime();
                    //5秒キープしたら
                    if (TornadoKeepTime >= 180)
                    {
                        //減速フラグ
                        isSpeedDown = true;
                        TornadoKeepTime = 0;
                    }
                }
            }
            //徐々に減速
            else
            {
                if (TornadoSpeed > MinTornadoSpeed)
                {
                    TornadoSpeed -= Method.GameTime() * AddTakeAngleSpeed;
                }
                //最小まで減速したら
                else
                {
                    TornadoSpeed = MinTornadoSpeed;
                }
            }
            //Debug.Log(isSpeedDown);
            //Debug.Log(TornadoAngle);
            //if (TornadoAngle > 360) { TornadoAngle = 0; }
            //回転角度反映
            EmmitObject.transform.eulerAngles = new Vector3(0, EmmitObject.transform.eulerAngles.y + TornadoSpeed, 0);
        }
		
		/// <summary>
		/// バーサクスキル・ビッグマイン
		/// </summary>
		public void BigMine()
		{
			
		}

		/// <summary>
		/// バーサクスキル・オメガレーザー
		/// </summary>
		public void OmegaLaser()
		{
			
		}

		/// <summary>
		/// 何かに触れたら
		/// </summary>
		//void OnTrrigerEnter(Cllider collider)
		//{
		//	if (collider.tag == "Player") 
		//	{
		//		TargetObject.GetComponent<PlayerController>().Damage(5);
		//	}
		//}
	}
}