using UnityEngine;
using System.Collections;

public class CageScript : MonoBehaviour {

    /// <summary>
    /// アニメーター
    /// </summary>
    private Animator animator;
    /// <summary>
    /// 籠が開いたかどうか
    /// </summary>
    private bool isOpen = false;
    /// <summary>
    /// 効果音
    /// </summary>
    [SerializeField]
    private AudioClip se;

	// Use this for initialization
	void Start () {
        animator = this.gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void CageOpenEvent()
    {
        GetComponent<AudioSource>().PlayOneShot(se);
    }

    void CageOpenEndEvent()
    {
        this.animator.speed = 0;
        isOpen = true;
    }

    public bool getIsCageOpen()
    {
        return isOpen;
    }
}
