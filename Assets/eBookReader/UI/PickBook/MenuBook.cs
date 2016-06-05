using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MenuBook : MonoBehaviour, ISelectHandler, IPointerDownHandler
{
    private Vector3 startScale = Vector3.one;

    public Texture2D FrontCover = null;

    public Texture2D BackCover = null;

    public Color BookColor = Color.white;

    public UIMesh Cover = null;

    public UIMesh Side = null;

    public UIMesh Back = null;

    public Transform BookModel = null;

    public string BookBundle = null;

    public BookData.Book MyBook = null;

    // Use this for initialization
    void Start()
    {
        Cover.Texture = FrontCover;
        Cover.color = Color.white;

        Back.Texture = BackCover;
        Back.color = Color.white;

        Side.color = BookColor;

        startScale = transform.localScale;
    }

    public void OnSelect(BaseEventData eventData)
    {
        Select();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Select();
    }

    public void Select()
    {
        PickBook.Instance.SelectedBook = this;

        LeanTween.scale(gameObject, startScale * 1.5f, 0.15f);
    }

    public void Deselect()
    {
        LeanTween.scale(gameObject, startScale, 0.15f);
    }
}
