using UnityEngine;
using System.Collections.Generic;

namespace BookData
{
    [System.Serializable]
    public class Page
    {
        public int nr;

        public string image;

        public Texture2D imageTex;

        public string audio;

        public AudioClip audioClip;

        public string video;

        public List<Text> texts;

        public List<Button> buttons;

        public string arObject = null;

        public GameObject arObjectPrefab = null;

        public Page(int pageNr)
        {
            nr = pageNr;
            image = null;
            imageTex = null;
            audio = null;
            audioClip = null;
            video = null;
            arObject = null;
            arObjectPrefab = null;
            texts = new List<Text>();
            buttons = new List<Button>();
        }
    }
}
