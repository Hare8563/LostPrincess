using UnityEngine;
using System.Collections;
using StatusClass;

public class EnemyScript : MonoBehaviour
{
    /// <summary>
    /// ステータスクラス
    /// </summary>
    Status status;
    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    GameObject player;
    /// <summary>
    /// 攻撃フラグ
    /// </summary>
    bool AttackFlag = false;
    /// <summary>
    /// 二点間距離
    /// </summary>
    float twoPointDistance;

    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        player = GameObject.Find(@"HERO_MOTION07");
        status = new Status(1, 0, 10, 5);
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        bool swordAttack = false;
        bool running = false;
        float MaxDis = 30.0f;
        float MinDis = 8.0f;
        //二点間の距離
        twoPointDistance = Vector3.Distance(this.transform.position, player.transform.position);
		var distance = this.transform.position - player.transform.position;


        //Debug.Log (Mathf.Sqrt(val));
        //プレイヤーが近づいてきたら自分も近づく
        if (twoPointDistance <= MaxDis && twoPointDistance > MinDis)
        {
			this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (player.transform.position - this.transform.position), 1.0f);
			this.transform.rotation = new Quaternion (0, this.transform.rotation.y, 0, this.transform.rotation.w);

			Vector3 enemyVector = distance.normalized;
			this.rigidbody.AddForce (-5.0f*enemyVector, ForceMode.VelocityChange);
			//transform.Translate (Vector3.forward* 0.2f);
			running = true;
        }
        //目の前に来たら攻撃
        if (twoPointDistance <= MinDis)
        {
			running = false;
			swordAttack = true;
        }
        //HPが0になったら経験値を取得
        if (this.status.HP <= 0)
        {
			StartCoroutine(@"Coroutine");
			Destroy(this.gameObject);
        }
        GetComponent<Animator>().SetBool(@"IsAttack", swordAttack);
        GetComponent<Animator>().SetBool(@"IsRunning", running);
    }

	IEnumerator Coroutine(){
			player.GetComponent<PlayerController> ().GetExp (3);
			yield return null;
	}
    /// <summary>
    /// 外部参照ダメージ処理
    /// </summary>
    /// <param name="val"></param>
    public void Damage(int val)
    {
        AudioSource audio = this.GetComponent<AudioSource>();
        audio.Play();
        this.status.HP -= val;
    }

    /// <summary>
    /// 何かに触れたら
    /// </summary>
    /// <param name="collider"></param>
    public void OnTriggerStay(Collider col)
    {
		var anim = GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0);
		if (col.gameObject.CompareTag (@"Player") && anim.IsName ("Base Layer.swordB")) {
				Debug.Log (this.status.Sword_Power);
				player.GetComponent<PlayerController>().Damage (this.status.Sword_Power);
				Vector3 vector = player.transform.position - this.transform.position;

				player.rigidbody.AddForce (vector.normalized * 2.0f, ForceMode.VelocityChange);
		}
    }

    /// <summary>
    /// GUI表示
    /// </summary>
    void OnGUI()
    {
        if (twoPointDistance <= 30.0f)
        {
            GUI.Label(new Rect(100, 300, 200, 50), this.status.HP.ToString());
        }
    }
}