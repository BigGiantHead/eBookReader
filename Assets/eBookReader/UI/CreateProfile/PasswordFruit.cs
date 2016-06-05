using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class PasswordFruit : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPosition;

    private Transform startParent;

    [HideInInspector]
    public int Index = -1;

    public FruitType Type = FruitType.None;

    #region IBeginDragHandler implementation
    public void OnBeginDrag(PointerEventData eventData)
    {
        PickPassword.Instance.ItemBeingDragged = this;
        startPosition = transform.localPosition;

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
        if (PickPassword.Instance.ItemBeingDragged == this)
        {
            PickPassword.Instance.ItemBeingDragged = null;
        }

        if (Index == -1)
        {
            transform.localPosition = startPosition;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
    #endregion
}
