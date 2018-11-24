using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{

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
        animation.Play("Cutscene_Text");
    }
}
