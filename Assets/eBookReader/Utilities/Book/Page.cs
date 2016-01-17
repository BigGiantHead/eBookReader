﻿using UnityEngine;
using System.Collections;
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
        public Texture2D videoMovie;
        public List<Text> texts;
        public List<Button> buttons;

        public Page(int pageNr)
        {
            nr = pageNr;
            image = null;
            imageTex = null;
            audio = null;
            audioClip = null;
            video = null;
            videoMovie = null;
            texts = new List<Text>();
            buttons = new List<Button>();
        }
    }
}