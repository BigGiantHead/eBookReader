using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class BookGenerator : MonoBehaviour
{
    public MegaBookBuilder Book = null;

    public GameObject TextObject = null;

    public GameObject ButtonObject = null;

    public Texture2D TestTexture1 = null;

    public Texture2D TestTexture2 = null;

    void Start()
    {
        Book.NumPages = 16;
        Book.pageparams = new System.Collections.Generic.List<MegaBookPageParams>(16);
        for (int i = 0; i < 16; ++i)
        {
            Book.pageparams.Add(new MegaBookPageParams());
        }

        AddTextToPage(
            1,
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse erat sapien, efficitur nec sem eget, finibus dictum elit.",
            Color.white,
            55, 25,
            50, 60);

        AddButtonToPage(
            1,
            "Clicky",
            99, 33,
            50, 70, 30, () => Turn(5));

        AddTextToPage(
            2,
            "Vestibulum et risus non erat luctus pretium. Fusce eget justo in orci facilisis volutpat et nec lorem. In sed sem vitae massa vehicula tristique quis eu turpis.",
            Color.black,
            55, 25,
            50, 60);

        AddTextToPage(
            3,
            "In hac habitasse platea dictumst.Phasellus vitae imperdiet sapien. Pellentesque bibendum pretium est eu sollicitudin. Phasellus quis arcu vel nibh ultrices blandit non a est.",
            Color.white,
            55, 25,
            50, 60, 10);

        AddTextToPage(
            4,
            "Nullam sed nisi eu mauris tincidunt pulvinar.Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Phasellus felis enim, ornare non pretium eu, imperdiet ut ligula. Praesent efficitur ullamcorper ex vel molestie. Lorem ipsum dolor sit amet, consectetur adipiscing elit.Pellentesque fermentum euismod turpis nec fermentum.",
            Color.black,
            55, 25,
            50, 60, -10);

        AddButtonToPage(
            5,
            "Click back",
            99, 33,
            50, 70, 30, () => Turn(0));

        SetPageTexture(1, TestTexture1);
        SetPageTexture(2, TestTexture2);
        SetPageTexture(3, TestTexture1);
        SetPageTexture(4, TestTexture2);
        SetPageTexture(5, TestTexture1);
        SetPageTexture(6, TestTexture2);
        SetPageTexture(7, TestTexture1);
        SetPageTexture(8, TestTexture2);
        SetPageTexture(9, TestTexture1);
        SetPageTexture(10, TestTexture2);
        SetPageTexture(11, TestTexture1);
        SetPageTexture(12, TestTexture2);
        SetPageTexture(13, TestTexture1);
    }

    public void SetPageTexture(int page, Texture2D texture)
    {
        page = (int)Mathf.Clamp(page, 1, Book.GetPageCount() * 2);
        page -= 1;

        MegaBookPageParams myPage = Book.pageparams[page / 2];

        if (page % 2 == 0)
        {
            myPage.front = texture;
        }
        else
        {
            myPage.back = texture;
        }
    }

    public void AddTextToPage(int page, string text, Color textColor, float width, float fontSize, float x, float y, float rotation = 0)
    {
        page = Mathf.Clamp(page, 1, Book.GetPageCount() * 2);
        page -= 1;

        MegaBookPageParams myPage = Book.pageparams[page / 2];

        MegaBookPageObject textObject = new MegaBookPageObject();
        textObject.obj = Instantiate(TextObject) as GameObject;
        textObject.pos = new Vector3(x, 0, y);
        textObject.rot = new Vector3(90, rotation, 0);
        textObject.overridevisi = true;
        textObject.attachforward = Vector3.zero;
        textObject.attached = true;
        if (page % 2 == 0)
        {
            textObject.visilow = -0.5f;
            textObject.visihigh = 0.01f;
            textObject.offset = -0.01f;
        }
        else
        {
            textObject.visilow = 0.99f;
            textObject.visihigh = 1.99f;
            textObject.offset = 0.01f;
        }

        TypogenicText tText = textObject.obj.GetComponent<TypogenicText>();
        tText.ColorBottomLeft = textColor;
        tText.ColorBottomRight = textColor;
        tText.ColorTopLeft = textColor;
        tText.ColorTopRight = textColor;
        tText.WordWrap = width;
        tText.Size = fontSize;
        tText.Text = tText.GetWrappedText(text);

        myPage.objects.Add(textObject);
    }

    public void AddButtonToPage(int page, string buttonText, float width, float height, float x, float y, float rotation = 0, UnityAction action = null)
    {
        page = Mathf.Clamp(page, 1, Book.GetPageCount() * 2);
        page -= 1;

        MegaBookPageParams myPage = Book.pageparams[page / 2];

        MegaBookPageObject textObject = new MegaBookPageObject();
        textObject.obj = Instantiate(ButtonObject) as GameObject;
        textObject.pos = new Vector3(x, 0, y);
        textObject.rot = new Vector3(90, rotation, 0);
        textObject.overridevisi = true;
        textObject.attachforward = Vector3.zero;
        textObject.attached = true;
        if (page % 2 == 0)
        {
            textObject.visilow = -0.5f;
            textObject.visihigh = 0.01f;
            textObject.offset = -0.01f;
        }
        else
        {
            textObject.visilow = 0.99f;
            textObject.visihigh = 1.99f;
            textObject.offset = 0.01f;
        }

        PageButton button = textObject.obj.GetComponent<PageButton>();
        button.Text.text = buttonText;
        button.CanvasRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        button.CanvasRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

        if (action != null)
        {
            button.Button.onClick.AddListener(action);
        }

        myPage.objects.Add(textObject);
    }

    public void Turn(int page)
    {
        if (Book != null)
        {
            page = Mathf.Clamp(page, 1, Book.GetPageCount() * 2);

            Book.SetPage(page / 2, false);
        }
    }
}
