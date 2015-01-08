using UnityEngine;
using System.Collections;

public class collision : MonoBehaviour {
//	// Use this for initialization
//	void Start () {
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	}
		Collider collider;
	private void OnTriggerEnter(Collider collider){
				if (collider.gameObject.CompareTag ("Enemy")) {
						this.collider = collider;
						StartCoroutine ("Coroutine");
						Destroy (this);
				} else
						Destroy (this, .5f);
		}

		IEnumerator Coroutine(){
                var enemy = this.collider.gameObject.GetComponent<EnemyStatusManager>();
				var player = GameObject.FindGameObjectWithTag (@"Player").gameObject.GetComponent<PlayerController> ();
				enemy.Damage (player.status.Sword_Power);
				yield return null;
		}
}
