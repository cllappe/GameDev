using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonAudioController : MonoBehaviour {

    private AudioSource soundSource;
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioClip slideClip;
    [SerializeField] private AudioClip attackClip;
    [SerializeField] private AudioClip barrelClip;
    [SerializeField] private AudioClip chestClip;
    [SerializeField] private AudioClip foodClip;
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip bagClip;
    [SerializeField] private AudioClip barClip;
    [SerializeField] private AudioClip spikeHit;

    void Awake()
    {
        soundSource = GetComponent<AudioSource>();
    }

	public void Footstep()
    {
        soundSource.PlayOneShot(footstepClip, 1.0f);
    }

    public void Slide()
    {
        soundSource.PlayOneShot(slideClip, 1.0f);
    }

    public void Attack()
    {
        soundSource.PlayOneShot(attackClip, 1.0f);
    }

    public void Barrel()
    {
        soundSource.PlayOneShot(barrelClip, 1.0f);
    }

    public void Chest()
    {
        soundSource.PlayOneShot(chestClip, 1.0f);
    }

    public void Food()
    {
        soundSource.PlayOneShot(foodClip, 1.0f);
    }

    public void Coin()
    {
        soundSource.PlayOneShot(coinClip, 1.0f);
    }

    public void Bag()
    {
        soundSource.PlayOneShot(bagClip, 1.0f);
    }

    public void Bar()
    {
        soundSource.PlayOneShot(barClip, 1.0f);
    }

    public void SpikeHit()
    {
        soundSource.PlayOneShot(spikeHit, 1.0f);
    }
}
