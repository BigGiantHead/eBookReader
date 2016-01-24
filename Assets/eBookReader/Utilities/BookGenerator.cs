using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using BookData;

public class BookGenerator : MonoBehaviour
{
    private static BookGenerator instance = null;

    public static BookGenerator Instance
    {
        get
        {
            return instance;
        }
    }

    private string currentBook = "";

    public MegaBookBuilder Book = null;

    public GameObject TextObject = null;

    public GameObject ButtonObject = null;

    public GameObject PlayVideoButtonObject = null;

    public GameObject PlayAudioButtonObject = null;

    public Transform PageObjectsRoot = null;

    public GameObject BookDummy = null;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(LoadBook(Application.streamingAssetsPath + "/", currentBook));
        }
    }
#endif

    public void LoadBookFromBundle(string bundle)
    {
        currentBook = bundle;
        StartCoroutine(LoadBook(Application.streamingAssetsPath + "/", currentBook));
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
        BookDummy.SetActive(true);

        PageObjectsRoot.ClearChildren();

        int i = 0;

        Book.page = -1;
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
                AddTextToPage(book.pages[i].nr,
                              book.bookName,
                              book.pages[i].texts[j].reference,
                              book.pages[i].texts[j].colorC,
                              book.pages[i].texts[j].width, book.pages[i].texts[j].fontSize,
                              book.pages[i].texts[j].posX, book.pages[i].texts[j].posY,
                              book.pages[i].texts[j].Alignment,
                              book.pages[i].texts[j].rotation);
            }

            for (int j = 0; j < book.pages[i].buttons.Count; j++)
            {
                string actionName = book.pages[i].buttons[j].actionName;
                string actionParameter = book.pages[i].buttons[j].actionParameter;
                AddButtonToPage(book.pages[i].nr,
                                book.pages[i].buttons[j].reference,
                                book.pages[i].buttons[j].width, book.pages[i].buttons[j].height,
                                book.pages[i].buttons[j].posX, book.pages[i].buttons[j].posY, book.pages[i].buttons[j].rotation,
                                () => DoAction(actionName, actionParameter));
            }

            if (book.pages[i].imageTex != null)
            {
                SetPageTexture(book.pages[i].nr, book.pages[i].imageTex);
            }

            if (!string.IsNullOrEmpty(book.pages[i].video))
            {
                AddPlayVideoButtonToPage(book.pages[i].nr, book.pages[i].video, 95, 100);
            }
            if (!string.IsNullOrEmpty(book.pages[i].audio))
            {
                AddPlayAudioButtonToPage(book.pages[i].nr, book.pages[i].audioClip, 86f, 100);
            }
        }

        Book.basematerial.color = book.pageColorC;
        Book.basematerial1.color = book.pageColorC;
        Book.basematerial2.color = book.pageColorC;

        Book.rebuild = true;

        //Set covers
        Material mat;
        if (Book.frontcover != null)
        {
            if (Book.frontcover.childCount > 0)
            {
                MeshRenderer frontCover = Book.frontcover.GetChild(0).GetComponent<MeshRenderer>();

                mat = frontCover.materials[0];
                mat.color = book.coverColorC;

                mat = frontCover.materials[1];
                mat.color = book.coverColorC;
                mat.mainTexture = book.frontCoverImage1Tex;

                mat = frontCover.materials[2];
                mat.color = book.coverColorC;
                mat.mainTexture = book.frontCoverImage2Tex;
            }
        }
        if (Book.backcover != null)
        {
            if (Book.backcover.childCount > 0)
            {
                MeshRenderer backCover = Book.backcover.GetChild(0).GetComponent<MeshRenderer>();

                mat = backCover.materials[0];
                mat.color = book.coverColorC;

                mat = backCover.materials[1];
                mat.color = book.coverColorC;
                mat.mainTexture = book.backCoverImage1Tex;

                mat = backCover.materials[2];
                mat.color = book.coverColorC;
                mat.mainTexture = book.backCoverImage2Tex;
            }
        }
        if (Book.transform.FindChild("Spine") != null)
        {
            mat = Book.transform.FindChild("Spine").GetComponent<MeshRenderer>().material;
            mat.color = book.spineColorC;
        }

        float booksizeaspect = (float)book.height / book.width;
        
        Book.transform.parent.localScale = new Vector3(1, 1, booksizeaspect);
        Book.ChangeBookThickness(0.0009375f * book.numPages, true);
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

    public void AddTextToPage(int page, string bookName, string textRef, Color textColor, float width, float fontSize, float x, float y, TTextAlignment alignment, float rotation)
    {
        page = Mathf.Clamp(page, 1, Book.GetPageCount() * 2);
        page -= 1;

        MegaBookPageParams myPage = Book.pageparams[page / 2];

        MegaBookPageObject textObject = new MegaBookPageObject();

        textObject.obj = Instantiate(TextObject, Vector3.zero, Quaternion.identity) as GameObject;
        textObject.obj.transform.parent = PageObjectsRoot;

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
        tText.Alignment = alignment;

        LocalizedText localizedText = tText.gameObject.GetComponent<LocalizedText>();
        localizedText.UpdateReference(textRef, bookName);

        myPage.objects.Add(textObject);
    }

    public void AddPlayAudioButtonToPage(int page, AudioClip file, float x, float y)
    {
        page = Mathf.Clamp(page, 1, Book.GetPageCount() * 2);
        page -= 1;

        MegaBookPageParams myPage = Book.pageparams[page / 2];

        MegaBookPageObject buttonObject = new MegaBookPageObject();
        buttonObject.obj = Instantiate(PlayAudioButtonObject, Vector3.zero, Quaternion.identity) as GameObject;
        buttonObject.obj.transform.parent = PageObjectsRoot;

        buttonObject.rot = new Vector3(90, 0, 0);
        buttonObject.overridevisi = true;
        buttonObject.attachforward = Vector3.zero;
        buttonObject.attached = true;
        if (page % 2 == 0)
        {
            buttonObject.pos = new Vector3(x, 0, y);
            buttonObject.visilow = -0.5f;
            buttonObject.visihigh = 0.01f;
            buttonObject.offset = -0.01f;
        }
        else
        {
            buttonObject.pos = new Vector3(x, 0, y);
            buttonObject.visilow = 0.99f;
            buttonObject.visihigh = 1.99f;
            buttonObject.offset = 0.01f;
        }

        AudioPlayButton button = buttonObject.obj.GetComponentInChildren<AudioPlayButton>();
        button.CanvasRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 50);
        button.CanvasRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50);
        button.Audio.clip = file;

        myPage.objects.Add(buttonObject);
    }

    public void AddPlayVideoButtonToPage(int page, string file, float x, float y)
    {
        page = Mathf.Clamp(page, 1, Book.GetPageCount() * 2);
        page -= 1;

        MegaBookPageParams myPage = Book.pageparams[page / 2];

        MegaBookPageObject buttonObject = new MegaBookPageObject();
        buttonObject.obj = Instantiate(PlayVideoButtonObject, Vector3.zero, Quaternion.identity) as GameObject;
        buttonObject.obj.transform.parent = PageObjectsRoot;

        buttonObject.rot = new Vector3(90, 0, 0);
        buttonObject.overridevisi = true;
        buttonObject.attachforward = Vector3.zero;
        buttonObject.attached = true;
        if (page % 2 == 0)
        {
            buttonObject.pos = new Vector3(x, 0, y);
            buttonObject.visilow = -0.5f;
            buttonObject.visihigh = 0.01f;
            buttonObject.offset = -0.01f;
        }
        else
        {
            buttonObject.pos = new Vector3(x, 0, y);
            buttonObject.visilow = 0.99f;
            buttonObject.visihigh = 1.99f;
            buttonObject.offset = 0.01f;
        }

        PageButton button = buttonObject.obj.GetComponentInChildren<PageButton>();
        button.CanvasRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 50);
        button.CanvasRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50);

        button.Button.onClick.AddListener(() => { Handheld.PlayFullScreenMovie(file, Color.black, FullScreenMovieControlMode.CancelOnInput, FullScreenMovieScalingMode.AspectFit); });

        myPage.objects.Add(buttonObject);
    }

    public void AddButtonToPage(int page, string buttonTextRef, float width, float height, float x, float y, float rotation = 0, UnityAction action = null)
    {
        page = Mathf.Clamp(page, 1, Book.GetPageCount() * 2);
        page -= 1;

        MegaBookPageParams myPage = Book.pageparams[page / 2];

        MegaBookPageObject buttonObject = new MegaBookPageObject();
        buttonObject.obj = Instantiate(ButtonObject, Vector3.zero, Quaternion.identity) as GameObject;
        buttonObject.obj.transform.parent = PageObjectsRoot;

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
