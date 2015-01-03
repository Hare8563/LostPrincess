using UnityEngine;
using System.Collections;
using System.Xml;
using AssemblyCSharp;

public class toBossStage : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider){
        if(collider.tag == "Player"){
            var TargetModel = collider.gameObject.GetComponent<PlayerController>();
            PlayerPrefsEx prefs = new PlayerPrefsEx();
            prefs.SetString("NAME", TargetModel.status.NAME);
            prefs.SetInt("HP", TargetModel.status.HP);
            prefs.SetInt("MP", TargetModel.status.MP);
            prefs.SetInt("LV", TargetModel.status.LEV);
            prefs.SetInt("EXP", TargetModel.status.EXP);
            prefs.SetInt("Sword", TargetModel.status.Sword_Power);
            prefs.SetInt("Bow", TargetModel.status.BOW_POW);
            prefs.SetInt("Magic", TargetModel.status.Magic_Power);
            prefs.Save(System.Environment.CurrentDirectory + "/saveData.xml");

		    Application.LoadLevel (@"Boss");
        }
	}
}