using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PasswordFruit : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static PasswordFruit ItemBeingDragged;

    private Vector3 startPosition;

    private Transform startParent;

    public int Index = -1;

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
        transform.position = Input.mousePosition;
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
