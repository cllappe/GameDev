using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlowFlash : MonoBehaviour
{

    private Text text;
    public float time;
    public float fireDelay = 3.0f;

    private float fireTimestamp = 0.0f;
    // Use this for initialization
    void Start()
    {
        text = gameObject.GetComponent<Text>();
        fireTimestamp = Time.realtimeSinceStartup + fireDelay;

    }

    // Update is called once per frame
    void Update()
    {


        if (Time.realtimeSinceStartup > fireTimestamp)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Sin(Time.time * time));
        }
       // text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Sin(Time.time * time));
    }
}