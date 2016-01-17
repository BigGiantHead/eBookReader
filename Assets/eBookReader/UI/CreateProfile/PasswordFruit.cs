using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class PasswordFruit : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static PasswordFruit ItemBeingDragged;

    private Vector3 startPosition;

    private Transform startParent;

    [HideInInspector]
    public int Index = -1;

    public FruitType Type = FruitType.None;

    #region IBeginDragHandler implementation
    public void OnBeginDrag(PointerEventData eventData)
    {
        ItemBeingDragged = this;
        startPosition = transform.position;

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    #endregion

    #region IDragHandler implementation
    public void OnDrag(PointerEventData eventData)
    {
        transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0);
    }
    #endregion

    #region IEndDragHandler implementation
    public void OnEndDrag(PointerEventData eventData)
    {
        if (ItemBeingDragged == this)
        {
            ItemBeingDragged = null;
        }

        if (Index == -1)
        {
            transform.position = startPosition;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
    #endregion
}
