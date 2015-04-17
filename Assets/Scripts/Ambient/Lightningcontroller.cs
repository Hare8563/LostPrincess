using UnityEngine;
using System.Collections;

public class Lightningcontroller : MonoBehaviour
{
    /// <summary>
    /// 雷SE
    /// </summary>
    public AudioClip[] LightningSe;
    /// <summary>
    /// 経過時間
    /// </summary>
    private float SecondTime = 0;

    // Use this for initialization
    void Start()
    {
        GetComponent<AudioSource>().PlayOneShot(LightningSe[Random.Range(0, 3)], 0.3f);
        this.GetComponent<Light>().intensity = 8;
    }

    // Update is called once per frame
    void Update()
    {
        SecondTime += Time.deltaTime;
        //Debug.Log(SecondTime);
        if (SecondTime >= 10)
        {
            SecondTime = 0;
            GetComponent<AudioSource>().PlayOneShot(LightningSe[Random.Range(0, 3)], 0.3f);
            this.GetComponent<Light>().intensity = 8;
            this.transform.eulerAngles = new Vector3(15, Random.Range(0, 360), 0);
        }

        if (this.GetComponent<Light>().intensity > 0)
        {
            this.GetComponent<Light>().intensity -= 0.3f;
        }
    }
}
