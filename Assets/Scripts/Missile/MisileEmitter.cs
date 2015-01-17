using UnityEngine;
using System.Collections;

public class MisileEmitter : MonoBehaviour {
    /// <summary>
    /// 1秒間に進む距離
    /// </summary>
    public float _speed = 100.0f;
    /// <summary>
    /// 1秒間に回転する角度
    /// </summary>
    public float _rotSpeed = 60.0f;
    /// <summary>
    /// ミサイルプレハブ
    /// </summary>
    private GameObject MisileObject;
    /// <summary>
    /// 分裂までの時間
    /// </summary>
    private float EmmiterCount;
    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    private GameObject PlayerObject;
    /// <summary>
    /// 爆発プレハブ
    /// </summary>
    private GameObject ExprosionPrehub;
    /// <summary>
    /// 分裂効果音
    /// </summary>
    public AudioClip SubEmitSe;

    void Awake()
    {
        MisileObject = Resources.Load("Prefab/MisileSub") as GameObject;
    }

	// Use this for initialization
	void Start () 
    {
        EmmiterCount = 0;
    }
	
	// Update is called once per frame
	void Update () 
    {
        PlayerObject = GetObjecter.GetPlayerObject;
        // ターゲットとの距離
        float Distance = Vector3.Distance(PlayerObject.transform.position, this.transform.position);
        Vector3 InitvecTarget = PlayerObject.transform.position - this.transform.position; // ターゲットへのベクトル
        Vector3 InitvecForward = transform.TransformDirection(Vector3.forward);   // 弾の正面ベクトル
        float InitangleDiff = Vector3.Angle(InitvecForward, InitvecTarget);            // ターゲットまでの角度
        float InitangleAdd = (_rotSpeed * Time.deltaTime);                    // 回転角
        Quaternion InitrotTarget = Quaternion.LookRotation(InitvecTarget);
        EmmiterCount += Method.GameTime();
        if (Distance < 200f && EmmiterCount > 60f)
        {
            /*　　　③
             * 　②　　④
             * ①　　　　⑤
             * 　⑧　　⑥
             * 　　 ⑦　
             * x:縦回転(+:下,-:上) , y:横回転(+:左,-:右)
             * */
            int rote = 60;
            float calcRote = rote * 2 / 3;
            Instantiate(MisileObject, this.transform.position, InitrotTarget * Quaternion.Euler(new Vector3(0, rote, 0)));              //①
            Instantiate(MisileObject, this.transform.position, InitrotTarget * Quaternion.Euler(new Vector3(-calcRote, calcRote, 0)));  //②
            Instantiate(MisileObject, this.transform.position, InitrotTarget * Quaternion.Euler(new Vector3(-rote, 0, 0)));             //③
            Instantiate(MisileObject, this.transform.position, InitrotTarget * Quaternion.Euler(new Vector3(-calcRote, -calcRote, 0))); //④
            Instantiate(MisileObject, this.transform.position, InitrotTarget * Quaternion.Euler(new Vector3(0, -rote, 0)));             //⑤
            Instantiate(MisileObject, this.transform.position, InitrotTarget * Quaternion.Euler(new Vector3(calcRote, -calcRote, 0)));  //⑥
            Instantiate(MisileObject, this.transform.position, InitrotTarget * Quaternion.Euler(new Vector3(rote, 0, 0)));              //⑦
            Instantiate(MisileObject, this.transform.position, InitrotTarget * Quaternion.Euler(new Vector3(calcRote, calcRote, 0)));   //⑧
            audio.PlayOneShot(SubEmitSe);
            Destroy(this.gameObject);
        }
        //Debug.Log(Distance);
        // ターゲットまでの角度を取得
        Vector3 vecTarget = PlayerObject.transform.position + new Vector3(0, 4.0f, 0) - this.transform.position; // ターゲットへのベクトル
        Vector3 vecForward = transform.TransformDirection(Vector3.forward);   // 弾の正面ベクトル
        float angleDiff = Vector3.Angle(vecForward, vecTarget);            // ターゲットまでの角度
        float angleAdd = (_rotSpeed * Time.deltaTime);                    // 回転角
        Quaternion rotTarget = Quaternion.LookRotation(vecTarget);
        // ターゲットが回転角の外なら、指定角度だけターゲットに向ける
        float t = (angleAdd / angleDiff);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotTarget, t);
        // 前進
        transform.position += transform.TransformDirection(Vector3.forward) * _speed * Time.deltaTime;
	}

    /// <summary>
    /// 何かに触れた瞬間
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other != PlayerObject.collider)
        {
            //Instantiate(ExprosionPrehub, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
    }

}
