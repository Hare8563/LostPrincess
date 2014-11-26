using UnityEngine;
using System.Collections;
using StatusClass;

public class EnemyScript : MonoBehaviour {
	Status status;
    GameObject player;
	bool AttackFlag = false;
		float twoPointDistance;

	/// <summary>
	/// 初期化
	/// </summary>
	void Start () {	
		player = GameObject.Find(@"HERO_MOTION04");
		status = new Status (1, 0, 10, 5);
	}
	
	/// <summary>
	/// 更新
	/// </summary>
	void Update () {
		bool swordAttack = false;
		bool running = false;

		var distance = this.transform.position - player.transform.position;
		float val = distance.x * distance.x + distance.y * distance.y + distance.z * distance.z;
		twoPointDistance = Mathf.Sqrt (val);
		//Debug.Log (Mathf.Sqrt(val));
		if (twoPointDistance <= 30.0f && twoPointDistance > 8.0f) {
				this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (player.transform.position - this.transform.position), 1.0f);
				this.transform.rotation = new Quaternion (0, this.transform.rotation.y, 0, this.transform.rotation.w);
				transform.Translate (Vector3.forward* 0.2f);
				running = true;
		}
		if (twoPointDistance <= 8.0f) {
				running = false;
				swordAttack = true;
				var col = rigidbody.GetComponents<BoxCollider> ();
				col [1].center = new Vector3 (0.0f, 0.4f, 0.0f);
				col[1].size = new Vector3 (0.32f,0.26f, 0.33f);
		}

		if (this.status.HP <= 0) {
				StartCoroutine(@"Coroutine");
				Destroy(this.gameObject);
		}
		GetComponent<Animator> ().SetBool (@"isAttack", swordAttack);
		GetComponent<Animator> ().SetBool (@"isMove", running);
	}

    /// <summary>
    /// 経験値取得
    /// </summary>
    /// <returns></returns>
	IEnumerator Coroutine(){
		player.GetComponent<PlayerController> ().GetExp (3);
		yield return null;
	}

    /// <summary>
    /// 外部参照ダメージ処理
    /// </summary>
    /// <param name="val"></param>
	public void Damage(int val){
		AudioSource audio = this.GetComponent<AudioSource> ();
		audio.Play ();
		this.status.HP -= val;
	}

    /// <summary>
    /// 何かに触れたら
    /// </summary>
    /// <param name="collider"></param>
	public void OnTriggerEnter(Collider collider){
		if (collider.gameObject.tag == "Player") {
				var data = collider.GetComponent<PlayerController> ();
				data.Damage (1);
				var col = rigidbody.GetComponents<BoxCollider> ();
				col [1].center = new Vector3 (0.0f, 0.0f, 0.0f);
				col[1].size = new Vector3 (0.0f,0.0f, 0.0f);
		}
	}

    /// <summary>
    /// GUI表示
    /// </summary>
	void OnGUI(){
		if (twoPointDistance <= 30.0f) {
				GUI.Label (new Rect (100, 300, 200, 50), this.status.HP.ToString ());
		}
	}
}
