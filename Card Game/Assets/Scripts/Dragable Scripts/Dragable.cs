using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragable : MonoBehaviour {

    public bool useCursorOffset = true;

    private DragableActions da;
    private bool isDragging;
    private Vector3 cursorOffset = Vector3.zero;
    private float zOffset;

    private void Awake()
    {
        da = GetComponent<DragableActions>();
    }

    private void Update()
    {
        if (isDragging){
            Vector3 mousePos = MouseInWorldCoords();
            da.OnDraggingInUpdate();
            transform.position = new Vector3(mousePos.x - cursorOffset.x, mousePos.y - cursorOffset.y, transform.position.z);
        }
    }

    private void OnMouseDown()
    {
        if(da.CanDrag){
            isDragging = true;
            da.OnStartDrag();
            zOffset = -Camera.main.transform.position.z + transform.position.z;
        }

        if (useCursorOffset){
            cursorOffset = -transform.position + MouseInWorldCoords();
        }
        else{
            cursorOffset = Vector3.zero;
        }
    }

    private void OnMouseUp()
    {
        if (isDragging){
            isDragging = false;
            da.OnEndDrag();
        }
    }

    private Vector3 MouseInWorldCoords(){
        Vector3 screenMousePos = Input.mousePosition;
        screenMousePos.z = zOffset;
        return Camera.main.ScreenToWorldPoint(screenMousePos);
    }
}
