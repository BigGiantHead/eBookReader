using UnityEngine;
using System.Collections;
using AssetBundles;
using System.Xml;
using System;

namespace BookData
{
    public class BookLoader : MonoBehaviour
    {
        private static BookLoader instance = null;
        public static BookLoader Instance
        {
            get
            {
                return instance;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);

            if (Instance == null)
            {
                instance = this;
            }
            else if (Instance != this)
            {
                DestroyImmediate(this.gameObject);
            }
        }

        [HideInInspector]
        public Book book = null;

        public bool loadingBook = false;

        public string localizationAssetName = "Localization";

        public string bookAssetName = "Book";

        public void LoadFromAssetBundle(string absolutePath, string bundleName)
        {
            loadingBook = true;
            book = null;

            StartCoroutine(GoLoadFromAssetBundle(absolutePath, bundleName));
        }

        private IEnumerator GoLoadFromAssetBundle(string absolutePath, string bundleName)
        {
            Localization.Instance.LoadLocalizationFromAssetBundle(absolutePath, bundleName, localizationAssetName, false);
            while (Localization.Instance.loadingFromBundle)
            {
                yield return null;
            }

            yield return StartCoroutine(InitAssetBundleManager(absolutePath));

            AssetBundleLoadAssetOperation xmlLoad = AssetBundleManager.LoadAssetAsync(bundleName, bookAssetName, typeof(TextAsset));
            if (xmlLoad == null)
            {
                loadingBook = false;
                yield break;
            }
            yield return StartCoroutine(xmlLoad);

            TextAsset xmlText = xmlLoad.GetAsset<TextAsset>();
            if (xmlText && !string.IsNullOrEmpty(xmlText.text))
            {
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.LoadXml(xmlText.text);
                }
                catch (Exception exc)
                {
                    xmlDoc = null;
                    Debug.LogException(exc);
                }

                if (xmlDoc != null)
                {
                    XmlNode xmlBookNode = xmlDoc.FirstChild.NextSibling;
                    if (xmlBookNode.Name.ToLower() == "book")
                    {
                        book = new Book();
                        book.bookName = bundleName;
                        yield return StartCoroutine(LoadBookAttributes(bundleName, xmlBookNode, book));

                        XmlNodeList xmlPages = xmlBookNode.SelectNodes("page");
                        for (int i = 0; i < xmlPages.Count; i++)
                        {
                            if (xmlPages[i].Attributes["nr"] != null)
                            {
                                int pageNr = i + 1;
                                int.TryParse(xmlPages[i].Attributes["nr"].Value, out pageNr);
                                Page page = new Page(pageNr);

                                if (xmlPages[i].Attributes["image"] != null && !string.IsNullOrEmpty(xmlPages[i].Attributes["image"].Value))
                                {
                                    page.image = xmlPages[i].Attributes["image"].Value;
                                    yield return StartCoroutine(LoadImageFromBundle(bundleName, page));
                                }

                                if (xmlPages[i].Attributes["audio"] != null && !string.IsNullOrEmpty(xmlPages[i].Attributes["audio"].Value))
                                {
                                    page.audio = xmlPages[i].Attributes["audio"].Value;
                                    yield return StartCoroutine(LoadAudioFromBundle(bundleName, page));
                                }

                                if (xmlPages[i].Attributes["video"] != null && !string.IsNullOrEmpty(xmlPages[i].Attributes["video"].Value))
                                {
                                    page.video = xmlPages[i].Attributes["video"].Value;
                                    yield return StartCoroutine(LoadVideoFromBundle(bundleName, page));
                                }

                                XmlNodeList xmlTexts = xmlPages[i].SelectNodes("text");
                                for (int j = 0; j < xmlTexts.Count; j++)
                                {
                                    LoadText(xmlTexts[j], page, bundleName);
                                }
                                xmlTexts = null;

                                XmlNodeList xmlButtons = xmlPages[i].SelectNodes("button");
                                for (int j = 0; j < xmlButtons.Count; j++)
                                {
                                    LoadButton(xmlButtons[j], page, bundleName);
                                }
                                xmlButtons = null;

                                book.pages.Add(page);
                            }
                        }
                        xmlPages = null;

                        if (Mathf.FloorToInt(book.pages.Count / 2f) > book.numPages)
                            book.numPages = Mathf.FloorToInt(book.pages.Count / 2f);
                    }

                    xmlBookNode = null;
                    xmlDoc = null;
                }

                Resources.UnloadAsset(xmlText);
            }

            AssetBundleManager.UnloadAssetBundle(bundleName);

            loadingBook = false;
        }

        private IEnumerator InitAssetBundleManager(string absolutePath)
        {
            AssetBundleManager.SetSourceAssetBundleURL(absolutePath);

            var request = AssetBundleManager.Initialize();
            if (request != null)
                yield return StartCoroutine(request);
        }

        private IEnumerator LoadBookAttributes(string bundleName, XmlNode bookNode, Book book)
        {
            int bookNumPages = 0;
            if (bookNode.Attributes["numPages"] != null)
                int.TryParse(bookNode.Attributes["numPages"].Value, out bookNumPages);
            book.numPages = bookNumPages;

            if (bookNode.Attributes["coverColor"] != null && !string.IsNullOrEmpty(bookNode.Attributes["coverColor"].Value))
            {
                book.coverColor = bookNode.Attributes["coverColor"].Value;
                book.coverColorC = book.coverColor.ToColor();
            }

            if (bookNode.Attributes["spineColor"] != null && !string.IsNullOrEmpty(bookNode.Attributes["spineColor"].Value))
            {
                book.spineColor = bookNode.Attributes["spineColor"].Value;
                book.spineColorC = book.spineColor.ToColor();
            }

            if (bookNode.Attributes["pageColor"] != null && !string.IsNullOrEmpty(bookNode.Attributes["pageColor"].Value))
            {
                book.pageColor = bookNode.Attributes["pageColor"].Value;
                book.pageColorC = book.pageColor.ToColor();
            }

            if (bookNode.Attributes["frontCoverImage1"] != null && !string.IsNullOrEmpty(bookNode.Attributes["frontCoverImage1"].Value))
            {
                book.frontCoverImage1 = bookNode.Attributes["frontCoverImage1"].Value;
                yield return StartCoroutine(LoadImageFromBundle(bundleName, book, book.frontCoverImage1, (Texture2D image) => { book.frontCoverImage1Tex = image; }));
            }

            if (bookNode.Attributes["frontCoverImage2"] != null && !string.IsNullOrEmpty(bookNode.Attributes["frontCoverImage2"].Value))
            {
                book.frontCoverImage2 = bookNode.Attributes["frontCoverImage2"].Value;
                yield return StartCoroutine(LoadImageFromBundle(bundleName, book, book.frontCoverImage2, (Texture2D image) => { book.frontCoverImage2Tex = image; }));
            }

            if (bookNode.Attributes["backCoverImage1"] != null && !string.IsNullOrEmpty(bookNode.Attributes["backCoverImage1"].Value))
            {
                book.backCoverImage1 = bookNode.Attributes["backCoverImage1"].Value;
                yield return StartCoroutine(LoadImageFromBundle(bundleName, book, book.backCoverImage1, (Texture2D image) => { book.backCoverImage1Tex = image; }));
            }

            if (bookNode.Attributes["backCoverImage2"] != null && !string.IsNullOrEmpty(bookNode.Attributes["backCoverImage2"].Value))
            {
                book.backCoverImage2 = bookNode.Attributes["backCoverImage2"].Value;
                yield return StartCoroutine(LoadImageFromBundle(bundleName, book, book.backCoverImage2, (Texture2D image) => { book.backCoverImage2Tex = image; }));
            }

            if (bookNode.Attributes["titleReference"] != null)
            {
                book.titleReference = bookNode.Attributes["titleReference"].Value;
            }

            if (bookNode.Attributes["descriptionReference"] != null)
            {
                book.descriptionReference = bookNode.Attributes["descriptionReference"].Value;
            }

            int width = 2048;
            if (bookNode.Attributes["width"] != null)
            {
                int.TryParse(bookNode.Attributes["width"].Value, out width);
            }
            book.width = width;

            int height = 2048;
            if (bookNode.Attributes["height"] != null)
            {
                int.TryParse(bookNode.Attributes["height"].Value, out height);
            }
            book.height = height;
        }

        private IEnumerator LoadImageFromBundle(string bundleName, Book book, string image, Action<Texture2D> onLoadFinish)
        {
            if (onLoadFinish == null)
                yield break;

            AssetBundleLoadAssetOperation imageLoad;

            imageLoad = AssetBundleManager.LoadAssetAsync(bundleName, image, typeof(Texture2D));

            if (imageLoad == null)
                yield break;

            yield return StartCoroutine(imageLoad);

            onLoadFinish(imageLoad.GetAsset<Texture2D>());
        }

        private IEnumerator LoadImageFromBundle(string bundleName, Page page)
        {
            AssetBundleLoadAssetOperation imageLoad = AssetBundleManager.LoadAssetAsync(bundleName, page.image, typeof(Texture2D));
            if (imageLoad == null)
                yield break;
            yield return StartCoroutine(imageLoad);

            page.imageTex = imageLoad.GetAsset<Texture2D>();
        }

        private IEnumerator LoadAudioFromBundle(string bundleName, Page page)
        {
            AssetBundleLoadAssetOperation audioLoad = AssetBundleManager.LoadAssetAsync(bundleName, page.audio, typeof(AudioClip));
            if (audioLoad == null)
                yield break;
            yield return StartCoroutine(audioLoad);

            page.audioClip = audioLoad.GetAsset<AudioClip>();
        }

        private IEnumerator LoadVideoFromBundle(string bundleName, Page page)
        {
            AssetBundleLoadAssetOperation videoLoad = AssetBundleManager.LoadAssetAsync(bundleName, page.video, typeof(Texture2D));
            if (videoLoad == null)
                yield break;
            yield return StartCoroutine(videoLoad);

            page.videoMovie = videoLoad.GetAsset<Texture2D>();
        }

        private void LoadText(XmlNode textNode, Page page, string bookName)
        {
            if (textNode.Attributes["language"] != null && !textNode.Attributes["language"].Value.Contains(Localization.Instance.CurrentLanguage.ToString()))
                return;

            Text text = new Text();
            text.bookName = bookName;

            if (textNode.Attributes["reference"] != null)
                text.reference = textNode.Attributes["reference"].Value;

            if (textNode.Attributes["color"] != null && !string.IsNullOrEmpty(textNode.Attributes["color"].Value))
            {
                text.color = textNode.Attributes["color"].Value;
                text.colorC = text.color.ToColor();
            }

            float width = 0;
            if (textNode.Attributes["width"] != null)
                float.TryParse(textNode.Attributes["width"].Value, out width);
            text.width = width;

            float fontSize = 0;
            if (textNode.Attributes["fontSize"] != null)
                float.TryParse(textNode.Attributes["fontSize"].Value, out fontSize);
            text.fontSize = fontSize;

            float posX = 0;
            if (textNode.Attributes["posX"] != null)
                float.TryParse(textNode.Attributes["posX"].Value, out posX);
            text.posX = posX;

            float posY = 0;
            if (textNode.Attributes["posY"] != null)
                float.TryParse(textNode.Attributes["posY"].Value, out posY);
            text.posY = posY;

            float rotation = 0;
            if (textNode.Attributes["rotation"] != null)
                float.TryParse(textNode.Attributes["rotation"].Value, out rotation);
            text.rotation = rotation;

            if (textNode.Attributes["alignment"] != null)
            {
                text.alignment = textNode.Attributes["alignment"].Value;
            }

            page.texts.Add(text);
        }

        private void LoadButton(XmlNode buttonNode, Page page, string bookName)
        {
            if (buttonNode.Attributes["language"] != null && !buttonNode.Attributes["language"].Value.Contains(Localization.Instance.CurrentLanguage.ToString()))
                return;

            Button button = new Button();
            button.bookName = bookName;

            if (buttonNode.Attributes["reference"] != null && !string.IsNullOrEmpty(buttonNode.Attributes["reference"].Value))
                button.reference = buttonNode.Attributes["reference"].Value;

            float width = 0;
            if (buttonNode.Attributes["width"] != null)
                float.TryParse(buttonNode.Attributes["width"].Value, out width);
            button.width = width;

            float height = 0;
            if (buttonNode.Attributes["height"] != null)
                float.TryParse(buttonNode.Attributes["height"].Value, out height);
            button.height = height;

            float posX = 0;
            if (buttonNode.Attributes["posX"] != null)
                float.TryParse(buttonNode.Attributes["posX"].Value, out posX);
            button.posX = posX;

            float posY = 0;
            if (buttonNode.Attributes["posY"] != null)
                float.TryParse(buttonNode.Attributes["posY"].Value, out posY);
            button.posY = posY;

            float rotation = 0;
            if (buttonNode.Attributes["rotation"] != null)
                float.TryParse(buttonNode.Attributes["rotation"].Value, out rotation);
            button.rotation = rotation;

            if (buttonNode.Attributes["actionName"] != null && !string.IsNullOrEmpty(buttonNode.Attributes["actionName"].Value))
                button.actionName = buttonNode.Attributes["actionName"].Value;

            if (buttonNode.Attributes["actionParameter"] != null && !string.IsNullOrEmpty(buttonNode.Attributes["actionParameter"].Value))
                button.actionParameter = buttonNode.Attributes["actionParameter"].Value;

            page.buttons.Add(button);
        }
    }
}
