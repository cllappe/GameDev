using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAnimation : MonoBehaviour {

    Animator anim;

	void Start () {
        anim = GetComponent<Animator> ();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetTrigger("InRoom");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        anim.SetTrigger("InRoom");
    }
}
