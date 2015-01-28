using UnityEngine;
using System.Collections;

public class SwordEffectOperator : MonoBehaviour {
		[SerializeField]
	private GameObject player = null;


	// Use this for initialization
	void Start () {
				player = GameObject.Find (@"HERO_MOTION07");
				this.GetComponent<TrailRenderer> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
				var playerState = player.GetComponent<Animator> ();
				var animState = playerState.GetCurrentAnimatorStateInfo (0);
				if (animState.IsName(@"Base Layer.Sword") || animState.IsName(@"Base Layer.Sword_02")) {
				} else {
						this.GetComponent<TrailRenderer> ().enabled = false;
				}
	}

}
