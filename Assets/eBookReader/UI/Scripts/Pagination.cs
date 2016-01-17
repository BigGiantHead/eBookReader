using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Pagination : MonoBehaviour
{
    private RectTransform rect;

    private int currentPage = -1;

    private int pageCount = -1;

    private List<Image> pageImages = null;

    public Sprite Active = null;

    public Sprite Inactive = null;

    public GameObject ImageSample = null;

    public int CurrentPage
    {
        get
        {
            return currentPage;
        }
    }

    void Awake()
    {
        rect = (RectTransform)transform;
        ImageSample.transform.SetParent(null);
    }

    // Use this for initialization
    void Start()
    {

    }

    public void SetPages(int pageCount)
    {
        this.pageCount = pageCount;
        rect.ClearChildren();
        pageImages = new List<Image>(pageCount);

        for (int i = 0; i < pageCount; ++i)
        {
            GameObject pageGO = Instantiate(ImageSample);

            pageGO.transform.SetParent(rect);
            pageGO.transform.localPosition = Vector3.zero;
            pageGO.transform.localRotation = Quaternion.identity;
            pageGO.transform.localScale = Vector3.one;
            pageGO.SetActive(true);

            Image pageImage = pageGO.GetComponent<Image>();
            pageImages.Add(pageImage);
        }

        SetCurrentPage(1);
    }

    public void SetCurrentPage(int page)
    {
        if (currentPage > 0)
        {
            pageImages[currentPage - 1].overrideSprite = Inactive;
        }

        currentPage = Mathf.Clamp(page, 1, pageCount);
        pageImages[currentPage - 1].overrideSprite = Active;
    }

    public void NextPage()
    {
        if (currentPage > 0)
        {
            pageImages[currentPage - 1].overrideSprite = Inactive;
        }

        currentPage = Mathf.Clamp(currentPage + 1, 1, pageCount); ;
        pageImages[currentPage - 1].overrideSprite = Active;
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            pageImages[currentPage - 1].overrideSprite = Inactive;
        }

        currentPage = Mathf.Clamp(currentPage - 1, 1, pageCount); ;
        pageImages[currentPage - 1].overrideSprite = Active;
    }
}
