using UnityEngine;
using System.Collections;

public class Method : MonoBehaviour
{
    /// <summary>
    /// ゲーム内時間
    /// </summary>
    /// <returns>ゲーム内での一秒</returns>
    public static float GameTime()
    {
        float time;
        time = Time.deltaTime * 60;
        return time;
    }

    /// <summary>
    /// 値を滑らかに変化させる
    /// </summary>
    /// <param name="from">開始地点</param>
    /// <param name="to">目標地点</param>
    /// <param name="speed">変化速度</param>
    public static void SmoothChange(ref float from, float to, float speed)
    {
        float gap = Mathf.Abs(from - to);
        if (from > to) from -= speed * GameTime() * gap;
        if (from < to) from += speed * GameTime() * gap;
        //if ((int)from == (int)to) from = to;
        //Debug.Log(gap);
        //return from;
    }

    /// <summary>
    /// 角度を滑らかに変化させる
    /// </summary>
    /// <param name="from">開始地点</param>
    /// <param name="to">目標地点</param>
    /// <param name="speed">変化速度</param>
    public static void SmoothAngleChange(ref float from, float to, float speed)
    {
        float gapRad = Mathf.Abs(from - to);
        float gapAngle = gapRad * 180 / Mathf.PI;

        //差が180度より高い
        if (gapAngle > 180)
        {
            if (from < to)
            {
                from -= speed;
                if (from <= 0.000f)
                {
                    from = 360.00f * Mathf.PI / 180.0f;
                }
            }
            else if (from > to)
            {
                from += speed;
                if (from >= 360.00f * Mathf.PI / 180.0f)
                {
                    from = 0.000f;
                }
            }
        }
        //差が180度以下
        if (gapAngle <= 180)
        {
            if (from > to) from -= speed * gapRad;
            if (from < to) from += speed * gapRad;
        }
    }
    
    /// <summary>
    /// p2からp1へのXZ平面での角度を求める
    /// </summary>
    /// <param name="p1">自分の座標</param>
    /// <param name="p2">相手の座標</param>
    /// <returns>2点の角度</returns>
    public static float GetAim(Vector3 p1, Vector3 p2)
    {
        float dx = p2.x - p1.x;
        float dz = p2.z - p1.z;
        float rad = Mathf.Atan2(dz, dx);
        return rad * Mathf.Rad2Deg;
    }

    /// <summary>
    /// 選択肢の挙動
    /// </summary>
    /// <param name="value">選択肢に用いる変数</param>
    /// <param name="selectNum">選択肢の数</param>
    /// <param name="Direction">選択肢の移動する方向(up, downのみ)</param>
    /// <returns></returns>
    public static void Selecting(ref int value, int selectNum, string Direction)
    {
        if (Direction == "up" || Direction == "UP" || Direction == "Up")
        {
            value = (value + 1) % selectNum;
        }
        else if (Direction == "down" || Direction == "DOWN" || Direction == "Down")
        {
            value = (value + (selectNum - 1)) % selectNum;
        }
    }

    /// <summary>
    /// 60FPSカウントを得る
    /// </summary>
    public static float FlameCount()
    {
        return Time.time * 60.0f;
    }
}