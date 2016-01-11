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

    [Flags]
    public enum FruitType
    {
        None = 0,

        One = 1,

        Two = 2,

        Three = 4,

        Four = 8,

        Five = 16,

        Six = 32,

        Seven = 64,

        Eight = 128,

        Nine = 256,

        Ten = 512,

        Eleven = 1024,

        Twelve = 2048,

        Empty = 4096
    }
}
