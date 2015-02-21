using UnityEngine;
using System.Collections;

/// <summary>
/// 自身がオブジェクト方向を向くだけ
/// </summary>
public class RotationFollowOnly : MonoBehaviour
{
    /// <summary>
    /// ターゲット
    /// </summary>
    [SerializeField]
    private GameObject target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.LookAt(target.transform.position);
	}
}
