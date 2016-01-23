using UnityEngine;
using System.Collections;

namespace BookData
{
    [System.Serializable]
    public class Text
    {
        public string bookName;
        public string reference;

        public string color;

        public Color colorC;

        public float width;

        public float fontSize;

        public float posX;

        public float posY;

        public float rotation;

        public string alignment;

        public TTextAlignment Alignment
        {
            get
            {
                return (TTextAlignment)System.Enum.Parse(typeof(TTextAlignment), alignment);
            }
        }
        public Text()
        {
            bookName = null;
            reference = null;
            color = null;
            colorC = Color.white;
            width = 0;
            fontSize = 0;
            posX = 0;
            posY = 0;
            rotation = 0;
            alignment = "Center";
        }

        public string Title
        {
            get
            {
                return Localization.Instance.GetEntry(reference, bookName);
            }
        }
    }
}
