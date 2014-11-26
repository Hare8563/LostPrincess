using UnityEngine;
using System.Collections;
using StatusClass;

public class EnemyScript : MonoBehaviour {
	Status status;
    GameObject player;
		bool AttackFlag = false;

	// Use this for initialization
	void Start () {
		status = new Status (1, 0, 10, 5);
		player = GameObject.Find(@"HERO_MOTION04");
	}
	
	// Update is called once per frame
	void Update () {
			bool swordAttack = false;
			bool running = false;

			Vector3 distance = this.transform.position - player.transform.position;
				float val = distance.x * distance.x + distance.y * distance.y + distance.z * distance.z;
				//Debug.Log (Mathf.Sqrt(val));
				if (Mathf.Sqrt(val) <= 30.0f && Mathf.Sqrt(val) > 8.0f) {
						this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (player.transform.position - this.transform.position), 1.0f);
						this.transform.rotation = new Quaternion (0, this.transform.rotation.y, 0, this.transform.rotation.w);
						transform.Translate (Vector3.forward* 0.2f);
						running = true;
				}
				if (Mathf.Sqrt (val) <= 8.0f) {
						running = false;
						swordAttack = true;
						var col = rigidbody.GetComponents<BoxCollider> ();
						col [1].center = new Vector3 (0.0f, 0.4f, 0.0f);
						col[1].size = new Vector3 (0.32f,0.26f, 0.33f);
				}

		if (this.status.HP <= 0) {
				player.GetComponent<PlayerController> ().GetExp (3);
				Destroy(this.gameObject);
		}



				GetComponent<Animator> ().SetBool (@"isAttack", swordAttack);
				GetComponent<Animator> ().SetBool (@"isMove", running);
	}

	public void Damage(int val){
		AudioSource audio = this.GetComponent<AudioSource> ();
		audio.Play ();
		this.status.HP -= val;
	}

		public void OnTriggerEnter(Collider collider){
				if (collider.gameObject.tag == "Player") {
						var data = collider.GetComponent<PlayerController> ();
						data.Damage (1);
						var col = rigidbody.GetComponents<BoxCollider> ();
						col [1].center = new Vector3 (0.0f, 0.0f, 0.0f);
						col[1].size = new Vector3 (0.0f,0.0f, 0.0f);
				}
		}

	void OnGUI(){
				GUI.Label (new Rect (100, 300, 200, 50), this.status.HP.ToString());
	}
}
