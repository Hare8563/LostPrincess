using UnityEngine;
using System.Collections;

public class Pendulum : MonoBehaviour
{
    /// <summary>
    /// 回転軸の列挙体
    /// </summary>
    private enum Axis
    {
        X,
        Y,
        Z,
    }
    /// <summary>
    /// 回転軸
    /// </summary>
    [SerializeField]
    private Axis axis;
    /// <summary>
    /// 振る角度
    /// </summary>
    [SerializeField]
    [Range(-360, 360)]
    private float Angle = 0;
    /// <summary>
    /// 振る周期
    /// </summary>
    [SerializeField]
    private float Period = 0;
    /// <summary>
    /// 返り値
    /// </summary>
    private float PingpongValue = 0;
    /// <summary>
    /// 自身の初期角度
    /// </summary>
    private Vector3 InitAngle;

    // Use this for initialization
    void Start()
    {
        InitAngle = this.transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        PingpongValue = Mathf.Sin(Mathf.PI * 2 / Period * (Time.time * 60)) * Angle; 

        switch (axis)
        {
            case Axis.X:
                this.transform.localEulerAngles = new Vector3(PingpongValue, InitAngle.y, InitAngle.z);
                break;
            case Axis.Y:
                this.transform.localEulerAngles = new Vector3(InitAngle.x, PingpongValue, InitAngle.z);
                break;
            case Axis.Z:
                this.transform.localEulerAngles = new Vector3(InitAngle.x, InitAngle.y, PingpongValue);
                break;
        }
    }
}