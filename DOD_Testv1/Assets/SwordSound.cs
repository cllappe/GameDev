using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSound : MonoBehaviour {

    [SerializeField] private AudioClip slice;
    [SerializeField] private AudioSource swordSource;

    public void PlaySlice()
    {
        swordSource.PlayOneShot(slice, 1.0f);
    }
	
}
