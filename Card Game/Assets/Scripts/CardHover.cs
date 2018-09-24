using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHover : MonoBehaviour {
    private Vector3 startPos;


    private void OnMouseEnter()
    {
        if (!Input.GetMouseButton(0)){
            startPos = transform.position;
            iTween.MoveTo(gameObject, new Vector3(transform.position.x-0.7f, transform.position.y + 1.2f, transform.position.z - 1), 1f);

        }

    }

    private void OnMouseExit()
    {
        if (!Input.GetMouseButton(0)){
            iTween.MoveTo(gameObject, startPos, 0.1f);
        }
    }
}
