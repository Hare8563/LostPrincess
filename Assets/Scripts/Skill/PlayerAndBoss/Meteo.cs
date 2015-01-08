using UnityEngine;
using System.Collections;

public class Meteo : MonoBehaviour
{

    /// <summary>
    /// スピード
    /// </summary>
    [SerializeField]
    [Range(0, 100)]
    private float Speed;
    /// <summary>
    /// 目標落下地点
    /// </summary>
    private Vector3 TargetPosition;

    /// <summary>
    /// 落下地点カーソル
    /// </summary>
    private GameObject Cursor;
    /// <summary>
    /// カーソルが存在しているか
    /// </summary>
    private bool isCursor = false;
    /// <summary>
    /// インスタンス生成したカーソルオブジェクト
    /// </summary>
    private GameObject cursorInstance;
	/// <summary>
	/// Y座標
	/// </summary>
	private float y = 0;

    /// <summary>
    /// 初期化
    /// </summary>
    void Awake()
    {
        Cursor = Resources.Load("Prefab/MeteoPoint") as GameObject;
    }

    // Use this for initialization
    void Start()
    {
		y = this.transform.position.y;
        //Destroy(this.gameObject, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //移動
        Move();

        //下にカーソルを表示させる
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            //Debug.Log(hit.collider.name);
            if (hit.collider.name == "Cube_005")
            {
                if (!isCursor)
                {
                    isCursor = true;
                    cursorInstance = (GameObject)Instantiate(Cursor, hit.point, hit.transform.rotation);
                }
                //Debug.Log(cursorInstance);
                if (cursorInstance != null) cursorInstance.GetComponent<Cursor>().SetPosition = hit.point + new Vector3(0, 0.1f, 0);
            }
        }
    }

    /// <summary>
    /// 何かに当たったら
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerEnter(Collider collider)
    {
		//Debug.Log(collider.tag);
        if (collider.tag == TargetTag)
        {
            if (TargetTag == "Player") { collider.GetComponent<PlayerController>().Damage(5); }
            else if (TargetTag == "Boss") { collider.GetComponent<EnemyStatusManager>().Damage(5); }
            else if (TargetTag == "Enemy") { collider.GetComponent<EnemyStatusManager>().Damage(5); }
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
		y -= Speed * Method.GameTime();
		//下に行ったら削除
		if (this.transform.position.y < -50)
		{
			Destroy(this.gameObject);
		}
		//位置更新
		this.transform.position = new Vector3(this.transform.position.x, y, this.transform.position.z);
    }

    /// <summary>
    /// ターゲットのタグ
    /// </summary>
    /// <value>The target tag.</value>
    public string TargetTag { set; get; }

    /// <summary>
    /// 目標落下地点
    /// </summary>
    public Vector3 TargetDropPosition { set; get; }
}
