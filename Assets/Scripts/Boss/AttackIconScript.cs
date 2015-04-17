using UnityEngine;
using System.Collections;

public class AttackIconScript : MonoBehaviour {
    /// <summary>
    /// 剣アイコンマテリアル
    /// </summary>
    [SerializeField]
    private Material swordMaterial;
    /// <summary>
    /// 魔法アイコンマテリアル
    /// </summary>
    [SerializeField]
    private Material magicMaterial;
    /// <summary>
    /// 弓アイコンマテリアル
    /// </summary>
    [SerializeField]
    private Material bowMaterial;

	// Use this for initialization
	void Start () {
        this.GetComponent<Renderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// 攻撃アイコンの設定
    /// </summary>
    /// <param name="name"></param>
    public void setAttackIcon(string name)
    {
        //Debug.Log(name);
        this.GetComponent<Renderer>().enabled = true;
        switch (name)
        {
            case "sword":
                this.GetComponent<Renderer>().material = swordMaterial;
                break;
            case "magic":
                this.GetComponent<Renderer>().material = magicMaterial;
                break;
            case "bow":
                this.GetComponent<Renderer>().material = bowMaterial;
                break;
            case "":
                this.GetComponent<Renderer>().enabled = false;
                break;
        }
    }
}
