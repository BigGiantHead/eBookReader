using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BookData;

public class PickBook : MonoBehaviour
{
    private static PickBook instance = null;

    public static PickBook Instance
    {
        get
        {
            return instance;
        }
    }

    private float targetScroll = 0;

    private float scrollDelta = 0;

    public List<string> Books = null;

    private List<RenderTexture> bookTextures = null;

    public GameObject BookSample = null;

    public RectTransform Layout = null;

    public ModalPanel MyPanel = null;

    public Pagination BooksPagination = null;

    public GameObject LeftArrow = null;

    public GameObject RightArrow = null;

    public UninteractableScrollRect Scroll = null;

    void Awake()
    {
        instance = this;
        BookSample.transform.SetParent(null);

        MyPanel.OnShowEnd = PopulateBooks;
        MyPanel.OnShowStart = OnShowStart;
    }

    void Start()
    {
    }

    void Update()
    {
        Scroll.horizontalNormalizedPosition = Mathf.Lerp(Scroll.horizontalNormalizedPosition, targetScroll, Time.deltaTime * 4);
    }

    public void PressLeftArrow()
    {
        BooksPagination.PreviousPage();

        RightArrow.SetActive(true);
        if (BooksPagination.CurrentPage == 1)
        {
            LeftArrow.SetActive(false);
        }
        else
        {
            LeftArrow.SetActive(true);
        }

        targetScroll = Mathf.Clamp01(targetScroll - scrollDelta);
    }

    public void PressRightArrow()
    {
        BooksPagination.NextPage();

        LeftArrow.SetActive(true);
        if (BooksPagination.CurrentPage == Books.Count)
        {
            RightArrow.SetActive(false);
        }
        else
        {
            RightArrow.SetActive(true);
        }

        targetScroll = Mathf.Clamp01(targetScroll + scrollDelta);
    }

    private void OnShowStart()
    {
        LeftArrow.SetActive(false);
        RightArrow.SetActive(Books.Count > 1);
        targetScroll = 0;
    }

    private void PopulateBooks()
    {
        Layout.ClearChildren();

        StartCoroutine(DoPopulateBooks());        
    }

    private IEnumerator DoPopulateBooks()
    {
        bookTextures = new List<RenderTexture>();

        Vector2 size = Layout.sizeDelta;
        size.x = (Books.Count) * 785 + (Books.Count - 1) * 50;
        Layout.sizeDelta = size;

        Vector3 position = Layout.localPosition;
        position.x = size.x / 2f - 392;
        Layout.localPosition = position;

        BooksPagination.SetPages(Books.Count);

        scrollDelta = 1f / (Books.Count - 1);

        for (int i = 0; i < Books.Count; ++i)
        {
            BookLoader.Instance.LoadFromAssetBundle(Application.streamingAssetsPath + "/", Books[i]);

            while (BookLoader.Instance.loadingBook)
            {
                yield return null;
            }

            BookData.Book loadedBook = BookLoader.Instance.book;

            GameObject book = Instantiate(BookSample) as GameObject;

            book.transform.SetParent(Layout);

            book.transform.localScale = Vector3.one;
            book.transform.localPosition = Vector3.zero;
            book.transform.localRotation = Quaternion.identity;

            MenuBook mBook = book.GetComponent<MenuBook>();
            mBook.Title.text = loadedBook.Title;
            mBook.Description.text = loadedBook.Description;
            mBook.FrontCover = loadedBook.frontCoverImage1Tex;
            mBook.BackCover = loadedBook.backCoverImage2Tex;
            mBook.BookColor = loadedBook.coverColorC;
            mBook.BookModel.localScale = new Vector3(1.4f, 1.4f * ((float)loadedBook.height / loadedBook.width), 1.4f);
            mBook.BookBundle = Books[i];
        }        

        yield break;
    }
}
