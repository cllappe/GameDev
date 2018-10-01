using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// Script for dragging and card hover on mouse over.
/// </summary>

public class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Targets targets;

    public Transform parentToReturnTo = null;
    public Transform placeholderParent = null;
    public float mouseOffset;
    public static bool validDrop = false;
    public static bool dragging = false;
    private bool validDropHappened = false;
    public static bool playerTurn;

    private Vector3 startPos;

    GameObject placeholder = null;

    private void OnMouseEnter()
    {
        if (!Input.GetMouseButton(0))
        {
            startPos = transform.position;
            iTween.MoveTo(gameObject, new Vector3(transform.position.x - 0.5f, transform.position.y + 1.2f, transform.position.z - 1.2f), 1f);
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = true;

        placeholder = new GameObject();
        placeholder.transform.SetParent(this.transform.parent);
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        parentToReturnTo = this.transform.parent;
        placeholderParent = parentToReturnTo;
        this.transform.SetParent(this.transform.parent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
        mouseOffset = Vector3.Distance(this.transform.position, Camera.main.transform.position);
    }

    public void OnDrag(PointerEventData eventData)
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 rayPoint = ray.GetPoint(mouseOffset);
        rayPoint.z = 0;
        this.transform.position = rayPoint;
        

        if (placeholder.transform.parent != placeholderParent)
            placeholder.transform.SetParent(placeholderParent);

        int newSiblingIndex = placeholderParent.childCount;

        for (int i = 0; i < placeholderParent.childCount; i++)
        {
            if (this.transform.position.x < placeholderParent.GetChild(i).position.x)
            {

                newSiblingIndex = i;

                if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                    newSiblingIndex--;

                break;
            }
        }

        placeholder.transform.SetSiblingIndex(newSiblingIndex);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (validDrop){
            //Debug.Log("In Valid Drop");
            startPos = this.transform.position;
            validDropHappened = true;
            this.transform.SetParent(parentToReturnTo);
            this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
            foreach (Transform child in transform) {
                GameObject.Destroy(child.gameObject);
            }
        }
        else{
            startPos = new Vector3(this.transform.position.x, startPos.y, startPos.z);
            this.transform.SetParent(parentToReturnTo);
            this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        dragging = false;
        Destroy(placeholder);
    }

    private void OnMouseExit()
    {
        if (!Input.GetMouseButton(0) && !validDropHappened)
        {
            iTween.MoveTo(gameObject, startPos, 0f);
        }
    }



}
