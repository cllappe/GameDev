using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startTorch : MonoBehaviour {

    public Animator animation;
    public float time;

    void Start()
    {
        animation = GetComponent<Animator>();
        StartCoroutine(AnimationDelay());
    }

    IEnumerator AnimationDelay()
    {
        yield return new WaitForSeconds(time);
        animation.Play("TorchStart");
    }
}
