using UnityEngine;
using System.Collections;
using StatusClass;

public class CharController : MonoBehaviour {
	public Vector3 downForce;

	private bool deadFlag=false;
	private AttackDataBase adb;
	private Status status;
	private int attackMode;//this is attack mode. Value is that 0 is sword, 1 is magic, 2 is bow, other is sword
	public const float SPEED = 20.0f;
	public const float TURN_SPEED = 5.0f;
	public Status CharStatus;

	GameObject prefab;
	float attackTime=0.0f;
	bool flag = false;

	void Start () {
		this.CharStatus = new Status ();
		adb = new AttackDataBase ();
		attackMode = 0;//Defalt is Sword Value
		status = new Status ();
		downForce = Vector3.down;
	}
	
	void Update () {

		bool isRunning=false;
		bool useSword = false;
		bool useMagic = false;
		bool useBow = false;
		float inputH = Input.GetAxis("Horizontal");
		float inputV = Input.GetAxis("Vertical");
		if (inputH != 0.0f || inputV != 0.0f) {
			isRunning = true;
		}


		if (Input.GetKeyDown (KeyCode.V)) {

			switch(attackMode){
			case 0:
				attackMode = 1;
				break;
			case 1:
				attackMode = 2;
				break;
			case 2:
				attackMode = 0;
				break;
			default:
				break;
			}
		}

		Vector3 input = new Vector3(inputH, 0.0f, inputV).normalized;
		if (Input.GetKey(KeyCode.B)) {

			attackTime = Time.time;

			switch(attackMode){
			case 0:
				useSword = true;
				adb.Sword_Count++;
				prefab = (GameObject)Resources.Load (@"Attack");
				this.flag = true;
				break;
			case 1:
				useMagic = true;
				adb.Magic_Count++;
				break;
			case 2:
				useBow = true;
				adb.Bow_Count++;
				break;
			default:
				useSword = true;
				adb.Sword_Count++;
				break;
			}
		}

		if (Time.time >= attackTime + 0.5f && this.flag == true) {
			// プレハブからインスタンスを生成
			var instanse = Instantiate (prefab, new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z+0.2f), Quaternion.identity);
			
			Destroy(instanse, 0.5f);
			this.flag = false;
		}

		this.transform.position += VectorUtil.Multiple(input, SPEED * Time.deltaTime);

		if (!input.Equals(Vector3.zero)) {
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(input), TURN_SPEED * Time.deltaTime);
		}

		//常に下へ力を加える
		rigidbody.AddForce (downForce);

		GetComponent<Animator>().SetBool (@"IsRunning", isRunning);
		GetComponent<Animator>().SetBool (@"useSword", useSword);
		GetComponent<Animator>().SetBool (@"useBow", useBow);
		GetComponent<Animator>().SetBool (@"useMagic", useMagic);

	}

	//Define GetExp Method in order to call from other method
	public void GetExp(int exp){
		this.status.EXP += exp;
		if (this.status.EXP >= this.status.ExpLimit)
			status.LevUp ();
	}

	//Define Damage Method in order to call from other method
	public void Damage(int val){
		this.status.HP -= val; 
		if (this.status.HP <= 0)
						this.deadFlag = true;
	}

	void OnGUI(){
		GUI.Label (new Rect (0, 300, 200, 50), this.status.EXP.ToString());
	}

}


public class AttackDataBase
{
    private float _swordRatio;
    private float _magicRatio;
    private float _bowRatio;

    public float Sword_Ratio
    {
        get
        {
            int AllCount = Sword_Count + Magic_Count + Bow_Count;
            return (float)Sword_Count / (float)AllCount;
        }
        set
        {
            _swordRatio = value;
        }
    }
    public float Bow_Ratio
    {
        get
        {
            int AllCount = Sword_Count + Magic_Count + Bow_Count;
            return (float)Bow_Count / (float)AllCount;
        }
        set
        {
            _bowRatio = value;
        }

    }
    public float Magic_Ratio
    {
        get
        {
            int AllCount = Sword_Count + Magic_Count + Bow_Count;
            return (float)Magic_Count / (float)AllCount;
        }
        set
        {
            _magicRatio = value;
        }
    }

    public int Sword_Count { set; get; }
    public int Magic_Count { set; get; }
    public int Bow_Count { set; get; }

    public AttackDataBase()
    {
        this.Sword_Ratio = 0.0f;
        this.Bow_Ratio = 0.0f;
        this.Magic_Ratio = 0.0f;
    }

}

public class VectorUtil
{
    public static Vector3 Multiple(Vector3 vec, float scalar)
    {
        return new Vector3(vec.x * scalar, vec.y * scalar, vec.z * scalar);
    }
}