using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BookData
{
    [System.Serializable]
    public class Book
    {
        public int numPages;
        public List<Page> pages;
        public string coverColor;
        public Color coverColorC;
        public string pageColor;
        public Color pageColorC;
        public string frontCoverImage;
        public Texture2D frontCoverImageTex;
        public string backCoverImage;
        public Texture2D backCoverImageTex;

        public Book()
        {
            numPages = 0;
            pages = new List<Page>();
            coverColor = null;
            coverColorC = Color.white;
            pageColor = null;
            pageColorC = Color.white;
            frontCoverImage = null;
            frontCoverImageTex = null;
            backCoverImage = null;
            backCoverImageTex = null;
        }
    }
}
