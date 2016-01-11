﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PasswordFruitPosition : MonoBehaviour, IDropHandler
{
    [HideInInspector]
    public PasswordFruit MyItem = null;

    public int Index = -1;

    #region IDropHandler implementation
    public void OnDrop(PointerEventData eventData)
    {
        if (MyItem != null)
        {
            MyItem.Index = -1;
            MyItem.OnEndDrag(eventData);
        }

        if (PasswordFruit.ItemBeingDragged != null)
        {
            PasswordFruit.ItemBeingDragged.transform.position = transform.position;
            PasswordFruit.ItemBeingDragged.Index = Index;
        }

        MyItem = PasswordFruit.ItemBeingDragged;
    }
    #endregion
}