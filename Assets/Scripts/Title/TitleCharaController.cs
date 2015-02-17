using UnityEngine;
using System.Collections;

public class TitleCharaController : MonoBehaviour {
    /// <summary>
    /// 初期座標
    /// </summary>
    private Vector3 InitPos;
    /// <summary>
    /// 初期回転
    /// </summary>
    private Quaternion InitRot;
    /// <summary>
    /// 辿るノード配列
    /// </summary>
    private GameObject[] Nodes;
    /// <summary>
    /// 次のノード
    /// </summary>
    private GameObject node;
    /// <summary>
    /// 今向かうべきノード
    /// </summary>
    private int nextNode = 1;
    /// <summary>
    /// 通常移動スピード
    /// </summary>
    [SerializeField]
    [Range(0, 100)]
    private float Speed = 0;

    void Awake()
    {
        Nodes = GameObject.FindGameObjectsWithTag("Node");
    }

	// Use this for initialization
	void Start () 
    {
        nextNode = 1;
        node = GameObject.Find("Node_" + nextNode);
        InitPos = this.transform.position;
        InitRot = this.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //ノード方向へ進む
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(node.transform.position - this.transform.transform.position), 0.1f);
        this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);
        this.transform.Translate(Vector3.forward * Speed * Method.GameTime());

        //目標ノードへ到達したら次のノードへ
        if (Vector3.Distance(this.transform.position, node.transform.position) < 5.0f)
        {
            nextNode++;
            if (GameObject.Find("Node_" + nextNode) != null)
            {
                node = GameObject.Find("Node_" + nextNode);
            }
        }
	}

    public void Initialize()
    {
        nextNode = 1;
        node = GameObject.Find("Node_" + nextNode);
        this.transform.position = InitPos;
        this.transform.rotation = InitRot;
    }

    void RunSeTiming()
    {

    }
}
