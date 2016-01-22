using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using BookData;

public class BookGenerator : MonoBehaviour
{
    public MegaBookBuilder Book = null;

    public GameObject TextObject = null;
    public GameObject ButtonObject = null;

    void Start()
    {
        StartCoroutine(LoadBook(Application.streamingAssetsPath + "/", "book1"));
    }

    private IEnumerator LoadBook(string absolutePath, string bundleName)
    {
        BookLoader.Instance.LoadFromAssetBundle(absolutePath, bundleName);

        while (BookLoader.Instance.loadingBook)
        {
            yield return null;
        }

        GenerateBook(BookLoader.Instance.book);
    }

    public void GenerateBook(Book book)
    {
        int i = 0;
        
        Book.NumPages = book.numPages;

        Book.pageparams = new System.Collections.Generic.List<MegaBookPageParams>(book.numPages);
        for (i = 0; i < book.numPages; i++)
        {
            Book.pageparams.Add(new MegaBookPageParams());
        }

        for (i = 0; i < book.pages.Count; i++)
        {
            for (int j = 0; j < book.pages[i].texts.Count; j++)
            {
                AddTextToPage(book.bookName,
                              book.pages[i].nr, 
                              book.pages[i].texts[j].reference,
                              book.pages[i].texts[j].colorC,
                              book.pages[i].texts[j].width, book.pages[i].texts[j].fontSize,
                              book.pages[i].texts[j].posX, book.pages[i].texts[j].posY, book.pages[i].texts[j].rotation);
            }

            for (int j = 0; j < book.pages[i].buttons.Count; j++)
            {
                string actionName = book.pages[i].buttons[j].actionName;
                string actionParameter = book.pages[i].buttons[j].actionParameter;
                AddButtonToPage(book.bookName, 
                                book.pages[i].nr,
                                book.pages[i].buttons[j].reference,
                                book.pages[i].buttons[j].width, book.pages[i].buttons[j].height,
                                book.pages[i].buttons[j].posX, book.pages[i].buttons[j].posY, book.pages[i].buttons[j].rotation,
                                () => DoAction(actionName, actionParameter));
            }

            if (book.pages[i].imageTex != null)
            {
                SetPageTexture(book.pages[i].nr, book.pages[i].imageTex);
            }
        }

        Book.basematerial.color = book.pageColorC;
        Book.basematerial1.color = book.pageColorC;
        Book.basematerial2.color = book.pageColorC;

        Book.rebuild = true;

        Material mat;
        if (Book.frontcover)
        {
            if (Book.frontcover.childCount > 0)
            {
                mat = Book.frontcover.GetChild(0).GetComponent<MeshRenderer>().material;
                mat.color = book.coverColorC;
                mat.mainTexture = book.frontCoverImageTex;
            }
        }
        if (Book.backcover)
        {
            if (Book.backcover.childCount > 0)
            {
                mat = Book.backcover.GetChild(0).GetComponent<MeshRenderer>().material;
                mat.color = book.coverColorC;
                mat.mainTexture = book.backCoverImageTex;
            }
        }
        if (Book.transform.FindChild("Spine"))
        {
            mat = Book.transform.FindChild("Spine").GetComponent<MeshRenderer>().material;
            mat.color = book.coverColorC;
        }
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

    public void AddTextToPage(string bookName, int page, string textRef, Color textColor, float width, float fontSize, float x, float y, float rotation = 0)
    {
        page = Mathf.Clamp(page, 1, Book.GetPageCount() * 2);
        page -= 1;

        MegaBookPageParams myPage = Book.pageparams[page / 2];

        MegaBookPageObject textObject = new MegaBookPageObject();
        textObject.obj = Instantiate(TextObject, Vector3.zero, Quaternion.identity) as GameObject;
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
        localizedText.BookName = bookName;
        localizedText.UpdateReference(textRef);

        myPage.objects.Add(textObject);
    }

    public void AddButtonToPage(string bookName, int page, string buttonTextRef, float width, float height, float x, float y, float rotation = 0, UnityAction action = null)
    {
        page = Mathf.Clamp(page, 1, Book.GetPageCount() * 2);
        page -= 1;

        MegaBookPageParams myPage = Book.pageparams[page / 2];

        MegaBookPageObject buttonObject = new MegaBookPageObject();
        buttonObject.obj = Instantiate(ButtonObject, Vector3.zero, Quaternion.identity) as GameObject;
        buttonObject.pos = new Vector3(x, 0, y);
        buttonObject.rot = new Vector3(90, rotation, 0);
        buttonObject.overridevisi = true;
        buttonObject.attachforward = Vector3.zero;
        buttonObject.attached = true;
        if (page % 2 == 0)
        {
            buttonObject.visilow = -0.5f;
            buttonObject.visihigh = 0.01f;
            buttonObject.offset = -0.01f;
        }
        else
        {
            buttonObject.visilow = 0.99f;
            buttonObject.visihigh = 1.99f;
            buttonObject.offset = 0.01f;
        }

        PageButton button = buttonObject.obj.GetComponent<PageButton>();
        button.CanvasRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        button.CanvasRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

        LocalizedText localizedText = button.Text.gameObject.GetComponent<LocalizedText>();
        localizedText.BookName = bookName;
        localizedText.UpdateReference(buttonTextRef);

        if (action != null)
        {
            button.Button.onClick.AddListener(action);
        }

        myPage.objects.Add(buttonObject);
    }

    public void DoAction(string methodName, string value)
    {
        if (!string.IsNullOrEmpty(methodName))
            gameObject.SendMessage(methodName, value, SendMessageOptions.DontRequireReceiver);
    }

    public void Turn(int page)
    {
        if (Book != null)
        {
            page = Mathf.Clamp(page, 1, Book.GetPageCount() * 2);

            Book.SetPage(page / 2, false);
        }
    }

    public void Turn(string page)
    {
        int pagenr = 0;
        try
        {
            pagenr = int.Parse(page);
            Turn(pagenr);
        }
        catch { }
    }
}
