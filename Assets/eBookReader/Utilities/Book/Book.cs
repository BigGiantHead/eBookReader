using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BookData
{
    [System.Serializable]
    public class Book
    {
        public string bookName;

        public int numPages;

        public List<Page> pages;

        public string coverColor;

        public Color coverColorC;

        public string spineColor;

        public Color spineColorC;

        public string pageColor;

        public Color pageColorC;

        public string frontCoverImage1;

        public Texture2D frontCoverImage1Tex;

        public string frontCoverImage2;

        public Texture2D frontCoverImage2Tex;

        public string backCoverImage1;

        public Texture2D backCoverImage1Tex;

        public string backCoverImage2;

        public Texture2D backCoverImage2Tex;

        public int width;

        public int height;

        public string titleReference;

        public string descriptionReference;

        public bool startFromEnd;

        public Book()
        {
            bookName = null;
            numPages = 0;
            pages = new List<Page>();
            coverColor = null;
            coverColorC = Color.white;
            spineColor = null;
            spineColorC = Color.white;
            pageColor = null;
            pageColorC = Color.white;
            frontCoverImage1 = null;
            frontCoverImage1Tex = null;
            backCoverImage1 = null;
            backCoverImage1Tex = null;
            frontCoverImage2 = null;
            frontCoverImage2Tex = null;
            backCoverImage2 = null;
            backCoverImage2Tex = null;
            width = 2048;
            height = 1024;
            titleReference = "";
            descriptionReference = "";
            startFromEnd = false;
        }

        public string Title
        {
            get
            {
                return Localization.Instance.GetEntry(titleReference, bookName);
            }
        }

        public string Description
        {
            get
            {
                return Localization.Instance.GetEntry(descriptionReference, bookName);
            }
        }
    }
}
