using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class PasswordFruitPosition : MonoBehaviour, IDropHandler
{
    [HideInInspector]
    public PasswordFruit MyItem = null;

    public FruitType MyFruitType
    {
        get
        {
            if (MyItem == null)
            {
                return FruitType.None;
            }

            return MyItem.Type;
        }
    }

    public int Index = -1;

    public Action PasswordFruitPositionChanged = null;

    #region IDropHandler implementation
    public void OnDrop(PointerEventData eventData)
    {
        if (MyItem != null)
        {
            MyItem.Index = -1;
            MyItem.OnEndDrag(eventData);
        }

        if (PickPassword.Instance.ItemBeingDragged != null)
        {
            PickPassword.Instance.ItemBeingDragged.transform.position = transform.position;
            PickPassword.Instance.ItemBeingDragged.Index = Index;
        }

        MyItem = PickPassword.Instance.ItemBeingDragged;

        if (PasswordFruitPositionChanged != null)
        {
            PasswordFruitPositionChanged();
        }
    }
    #endregion

    public void ResetItem()
    {
        MyItem.Index = -1;
        MyItem.OnEndDrag(null);
        MyItem = null;
    }
}