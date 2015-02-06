using UnityEngine;
using System.Collections;
using StatusClass;

public class Mao : MonoBehaviour {
	Status MaoStatus;
	GameObject player;
	//bool DeadFlag = false;
	// Use this for initialization
	void Start () {
				MaoStatus = new Status (10, 0, 300, "CSV/RastBassTable");
		player = GameObject.FindWithTag (@"Player").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (MaoStatus.HP <= 0) {
			StartCoroutine("Coroutine");
			Destroy(this.gameObject);
		}
	}
	private IEnumerator Coroutine(){
		var playerData = player.GetComponent<CharController>();
		playerData.GetExp(5);
		yield return null;

	}

	private void OnTriggerEnter(Collider col){
		Debug.Log (@"Hello");
	}

	void Damage(int val){
		this.MaoStatus.HP -= val;
		var audioSource = gameObject.GetComponent<AudioSource> ();
		audioSource.Play ();
		}

	void OnGUI() {
		GUIStyle gui = new GUIStyle ();
		gui.fontSize = 20;
		gui.normal.textColor = Color.red;
		GUI.Label (new Rect (0, 0, 200, 50), MaoStatus.HP.ToString(), gui);
	}
}
