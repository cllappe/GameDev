using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startExplosion : MonoBehaviour
{
    
    public Animator animation;
    public float time;
    AudioSource combustSound;

    void Start()
    {
        animation = GetComponent<Animator>();
        combustSound = GetComponent<AudioSource>();
        StartCoroutine(AnimationDelay());
    }

    IEnumerator AnimationDelay()
    {
        yield return new WaitForSeconds(time);
        animation.Play("ExplosionTorch");
        combustSound.Play();
    }
}
