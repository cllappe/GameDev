using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonAudioController : MonoBehaviour {

    private AudioSource soundSource;
    public AudioClip[] sounds;

    void Awake()
    {
        soundSource = GetComponent<AudioSource>();
    }

	void Footstep()
    {
        soundSource.PlayOneShot(sounds[0], 1.0f);
    }

    public void Slide()
    {
        soundSource.PlayOneShot(sounds[1], 1.0f);
    }

    void Attack()
    {
        soundSource.PlayOneShot(sounds[2], 1.0f);
    }

    public void Barrel()
    {
        //soundSource.PlayOneShot(sounds[3], 1.0f);
    }

    public void Chest()
    {
        //soundSource.PlayOneShot(sounds[4], 1.0f);
    }
}
