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

            prefs.SetInt("HP", TargetModel.status.HP);
            prefs.SetInt("EXP", TargetModel.status.EXP);
            prefs.SetInt("LEV", TargetModel.status.LEV);
            prefs.SetInt("Sword", TargetModel.status.Sword_Power);
            prefs.SetInt("Bow", TargetModel.status.BOW_POW);
            prefs.SetInt("Magic", TargetModel.status.Magic_Power);
            prefs.SetInt("MP", TargetModel.status.MP);
            prefs.Save(System.Environment.CurrentDirectory + "/saveData.xml");

		    Application.LoadLevel (@"Boss");
        }
	}
}