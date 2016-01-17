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

    public List<Book> Books = null;

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
        if (BooksPagination.CurrentPage == 10)
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
    }

    private void PopulateBooks()
    {
        Layout.ClearChildren();

        bookTextures = new List<RenderTexture>();

        for (int i = 0; i < 10; ++i)
        {
            GameObject book = Instantiate(BookSample) as GameObject;

            book.transform.SetParent(Layout);

            book.transform.localScale = Vector3.one;
            book.transform.localPosition = Vector3.zero;
            book.transform.localRotation = Quaternion.identity;
        }

        Vector2 size = Layout.sizeDelta;
        size.x = (10) * 785 + (9) * 50;
        Layout.sizeDelta = size;

        Vector3 position = Layout.localPosition;
        position.x = size.x / 2f - 392;
        Layout.localPosition = position;

        BooksPagination.SetPages(10);

        scrollDelta = 1f / 9f;
    }
}
