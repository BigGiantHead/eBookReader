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

        public Book()
        {
            numPages = 0;
            pages = new List<Page>();
        }
    }
}
