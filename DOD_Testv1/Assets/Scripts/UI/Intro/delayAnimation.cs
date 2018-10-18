using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class delayAnimation : MonoBehaviour {

    public Animation animation;
    public int time;

    void Start()
    {
        animation = GetComponent<Animation>();
        StartCoroutine(AnimationDelay());
    }

    IEnumerator AnimationDelay()
    {
        animation.Stop();
        yield return new WaitForSeconds(time);
        animation.Play();
    }
}
