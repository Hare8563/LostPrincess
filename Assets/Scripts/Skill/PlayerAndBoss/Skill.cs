using UnityEngine;
using System.Collections;

namespace SkillClass
{
    public class Skill : MonoBehaviour
    {
        /// <summary>
        /// 斬撃オブジェクト
        /// </summary>
        private GameObject slashObject;
        /// <summary>
        /// メテオオブジェクト
        /// </summary>
        private GameObject meteoObject;
        /// <summary>
        /// 矢オブジェクト
        /// </summary>
        private GameObject arrowObject;
        /// <summary>
        /// インスタンス生成位置
        /// </summary>
        private Vector3 EmmitPosition;
        /// <summary>
        /// インスタンス生成角度
        /// </summary>
        private Quaternion EmmitRotation;
        /// <summary>
        /// ターゲットタグ名
        /// </summary>
        private string TargetTagName;
        /// <summary>
        /// 斬撃オブジェクト
        /// </summary>
        private GameObject slash;
        /// <summary>
        /// メテオオブジェクト
        /// </summary>
        private GameObject meteo;
        /// <summary>
        /// 矢オブジェクト
        /// </summary>
        private GameObject spleadArrow;

        /// <summary>
        /// スキルのコンストラクタ
        /// </summary>
        /// <param name="_EmmitPosition">生成位置</param>
        /// <param name="_EmmitRotation">生成角度</param>
        /// <param name="_TargetTagName">ターゲットタグ名</param>
        public Skill(Vector3 _EmmitPosition, Quaternion _EmmitRotation, string _TargetTagName)
        {
            EmmitPosition = _EmmitPosition;
            EmmitRotation = _EmmitRotation;
            TargetTagName = _TargetTagName;
            Awake();
            Start();
        }

        void Awake()
        {
            if (slashObject == null) slashObject = Resources.Load("Prefab/SwordSlashEffect") as GameObject;
            if (meteoObject == null) meteoObject = Resources.Load("Prefab/MeteoBall") as GameObject;
            if (arrowObject == null) arrowObject = Resources.Load("Prefab/SpreadArrow") as GameObject;
        }

        // Use this for initialization
        void Start()
        {
            //slash = null;
            //meteo = null;
            //spleadArrow = null;
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// スキル・ソードスラッシュ
        /// </summary>
        public void SwordSlash()
        {
            //Debug.Log(slashObject);
            //Debug.Log(slash);
            if (slash == null)
            {
                slash = (GameObject)Instantiate(slashObject, EmmitPosition, EmmitRotation);
                slash.GetComponent<SwordSlash>().TargetTag = TargetTagName;
            }
        }

        /// <summary>
        /// スキル・メテオ
        /// </summary>
        public void Meteo()
        {
			float Range = 50;
			float Num = 5;
            if (meteo == null)
            {
				for(int i=0; i<Num; i++)
				{
					float x = Random.Range(-Range, Range);
					float y = Random.Range(300, 600);
					float z = Random.Range(-Range, Range);
	                meteo = (GameObject)Instantiate(meteoObject, EmmitPosition + new Vector3(x,y,z), EmmitRotation);
	                meteo.GetComponent<Meteo>().TargetTag = TargetTagName;
				}
            }
        }

        /// <summary>
        /// スキル・スプレッドアロー
        /// </summary>
        public void SpreadArrow()
        {
            if (spleadArrow == null)
            {
                spleadArrow = (GameObject)Instantiate(arrowObject, EmmitPosition, EmmitRotation);
                spleadArrow.transform.FindChild("SpreadArrow").GetComponent<SpreadArrow>().TargetTag = TargetTagName;
            }
        }
    }
}