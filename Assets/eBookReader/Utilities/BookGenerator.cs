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
            "text1",
            Color.white,
            55, 25,
            50, 60);

        AddButtonToPage(
            1,
            "button1",
            99, 33,
            50, 70, 30, () => Turn(5));

        AddTextToPage(
            2,
            "text2",
            Color.black,
            55, 25,
            50, 60);

        AddTextToPage(
            3,
            "text3",
            Color.white,
            55, 25,
            50, 60, 10);

        AddTextToPage(
            4,
            "text4",
            Color.black,
            55, 25,
            50, 60, -10);

        AddButtonToPage(
            5,
            "button2",
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

    public void AddTextToPage(int page, string textRef, Color textColor, float width, float fontSize, float x, float y, float rotation = 0)
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

        LocalizedText localizedText = tText.gameObject.GetComponent<LocalizedText>();
        localizedText.UpdateReference(textRef);

        myPage.objects.Add(textObject);
    }

    public void AddButtonToPage(int page, string buttonTextRef, float width, float height, float x, float y, float rotation = 0, UnityAction action = null)
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
        button.CanvasRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        button.CanvasRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

        LocalizedText localizedText = button.Text.gameObject.GetComponent<LocalizedText>();
        localizedText.UpdateReference(buttonTextRef);

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
