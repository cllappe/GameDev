using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Targets acceptableCard;
    public static String droppedOn;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter");
        if (eventData.pointerDrag == null)
            return;

        Dragable d = eventData.pointerDrag.GetComponent<Dragable>();
        if (d != null)
        {
            d.placeholderParent = this.transform;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerExit");
        if (eventData.pointerDrag == null)
            return;

        Dragable d = eventData.pointerDrag.GetComponent<Dragable>();
        if (d != null && d.placeholderParent == this.transform)
        {
            d.placeholderParent = d.parentToReturnTo;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);
        droppedOn = gameObject.name;

        Dragable d = eventData.pointerDrag.GetComponent<Dragable>();
        if (acceptableCard == d.targets)
        {
            //Debug.Log(acceptableCard);
            //Debug.Log(d.targets);
            Dragable.validDrop = true;
            if (d != null){
                //Debug.Log(acceptableCard);
                //Debug.Log(d.targets);
                d.parentToReturnTo = this.transform;
            }
        }

    }
}
