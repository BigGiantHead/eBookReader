using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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

    private MenuBook selectedBook = null;

    private float targetScroll = 0;

    private float scrollDelta = 0;

    public List<string> Books = null;

    private List<RenderTexture> bookTextures = null;

    public RectTransform Layout = null;

    public ModalPanel MyPanel = null;

    public List<MenuBook> MenuBooks = null;

    [Header("Book Display Elements")]
    public RawImage DisplayImage = null;

    public Text Description = null;

    public MenuBook SelectedBook
    {
        get
        {
            return selectedBook;
        }
        set
        {
            if (selectedBook != null)
            {
                selectedBook.Deselect();
            }

            selectedBook = value;
            if (selectedBook != null)
            {
                DisplayImage.enabled = true;
                Description.enabled = true;
                Description.text = selectedBook.MyBook.Description;

                float aspectSize = selectedBook.MyBook.width / selectedBook.MyBook.height;

                DisplayImage.rectTransform.sizeDelta = new Vector2(DisplayImage.rectTransform.sizeDelta.y * aspectSize, DisplayImage.rectTransform.sizeDelta.y);

                if (selectedBook.MyBook.startFromEnd)
                {
                    DisplayImage.texture = selectedBook.MyBook.backCoverImage2Tex;
                }
                else
                {
                    DisplayImage.texture = selectedBook.MyBook.frontCoverImage1Tex;
                }
            }
            else
            {
                DisplayImage.enabled = false;
                Description.enabled = false;
            }
        }
    }

    void Awake()
    {
        instance = this;

        MyPanel.OnShowStart = Start;
        MyPanel.OnShowEnd = PopulateBooks;
    }

    void Start()
    {
        SelectedBook = null;
    }

    private void PopulateBooks()
    {
        StartCoroutine(DoPopulateBooks());        
    }

    private IEnumerator DoPopulateBooks()
    {
        bookTextures = new List<RenderTexture>();

        for (int i = 0; i < Books.Count; ++i)
        {
            BookData.BookLoader.Instance.LoadFromAssetBundle(Application.streamingAssetsPath + "/", Books[i]);

            while (BookData.BookLoader.Instance.loadingBook)
            {
                yield return null;
            }

            BookData.Book loadedBook = BookData.BookLoader.Instance.book;

            GameObject book = MenuBooks[i].gameObject;

            book.SetActive(true);

            MenuBook mBook = MenuBooks[i];

            if (loadedBook.startFromEnd)
            {
                mBook.FrontCover = loadedBook.backCoverImage2Tex;
                mBook.BackCover = loadedBook.frontCoverImage1Tex;
            }
            else
            {
                mBook.FrontCover = loadedBook.frontCoverImage1Tex;
                mBook.BackCover = loadedBook.backCoverImage2Tex;
            }

            mBook.BookColor = loadedBook.coverColorC;
            mBook.BookModel.localScale = new Vector3(1.4f, 1.4f * ((float)loadedBook.height / loadedBook.width), 1.4f);

            //Modify y position to accomodate for book height
            Vector3 bookLocalPosition = mBook.BookModel.localPosition;
            bookLocalPosition.y = 171 * (mBook.BookModel.localScale.y / 1.4f);
            mBook.BookModel.localPosition = bookLocalPosition;

            mBook.MyBook = loadedBook;
            mBook.BookBundle = Books[i];
        }        

        yield break;
    }

    public void ReadBook()
    {
        if (SelectedBook != null)
        {
            BookGenerator.Instance.LoadBookFromBundle(SelectedBook.BookBundle);
        }

        MyPanel.Hide();
        CurrentProfileElement.Instance.MyPanel.Show();
    }
}
