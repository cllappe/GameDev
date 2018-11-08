using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLight : MonoBehaviour {

    Light fireLight;
    float lightInt;
    int flicker = 0;
    public float minInt = 13f, maxInt = 16f;

	// Use this for initialization
	void Start () {
        fireLight = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        flicker += 1;
        if ((flicker % 4) == 0)
        {
            lightInt = Random.Range(minInt, maxInt);
            fireLight.intensity = lightInt;
        }
	}
}
