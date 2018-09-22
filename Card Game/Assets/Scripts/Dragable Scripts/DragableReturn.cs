using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragableReturn : DragableActions
{
    private Vector3 savedPos;

    public override void OnDraggingInUpdate()
    {

    }

    public override void OnEndDrag()
    {
        iTween.MoveTo(gameObject, savedPos, 1f);
    }

    public override void OnStartDrag()
    {
        savedPos = transform.position;
    }

    protected override bool DragSuccessful()
    {
        return true;
    }
}
