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
        this.renderer.enabled = false;
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
        this.renderer.enabled = true;
        switch (name)
        {
            case "sword":
                this.renderer.material = swordMaterial;
                break;
            case "magic":
                this.renderer.material = magicMaterial;
                break;
            case "bow":
                this.renderer.material = bowMaterial;
                break;
            case "":
                this.renderer.enabled = false;
                break;
        }
    }
}
