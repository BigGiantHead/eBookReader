using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LINQtoCSV;
using System.IO;
using System.Linq;
using AssetBundles;

public enum Language { Auto, English, Turkish, Arabic }

public class Localization : MonoBehaviour
{
    private static Localization instance = null;
    public static Localization Instance
    {
        get
        {
            return instance;
        }
    }

    public Language testLanguage = Language.Auto;
    public Language defaultLanguage = Language.English;

    private Language currentLanguage;
    public Language CurrentLanguage
    {
        get
        {
            return this.currentLanguage;
        }
        set
        {
            this.currentLanguage = value;

            UpdateLocalization();
        }
    }

    private Dictionary<string, string> currentLocalizationEntries = new Dictionary<string,string>();

    private List<string> localizationsInResources = new List<string>();
    private List<string> localizationsInStreamingAssets = new List<string>();
    private List<string> localizationsOnWeb = new List<string>();
    private List<string> localizationsOnAssetBundle = new List<string>();

    public bool loadingFromBundle = false;

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

        UpdateLocalizationForce();
    }

    [ContextMenu("Update Localization")]
    private void UpdateLocalizationForce()
    {
        if (testLanguage != Language.Auto)
        {
            CurrentLanguage = testLanguage;
        }
        else
        {
            if (Application.systemLanguage == SystemLanguage.Arabic)
                CurrentLanguage = Language.Arabic;
            else if (Application.systemLanguage == SystemLanguage.English)
                CurrentLanguage = Language.English;
            else if (Application.systemLanguage == SystemLanguage.Turkish)
                CurrentLanguage = Language.Turkish;
            else if (defaultLanguage != Language.Auto)
                CurrentLanguage = defaultLanguage;
            else
                CurrentLanguage = Language.Arabic;
        }
    }

    private void UpdateLocalization()
    {
        currentLocalizationEntries.Clear();

        for (int i = 0; i < localizationsInResources.Count; i++)
        {
            LoadLocalizationFromResources(localizationsInResources[i]);
        }

        for (int i = 0; i < localizationsInStreamingAssets.Count; i++)
        {
            LoadLocalizationFromStreamingAssets(localizationsInStreamingAssets[i]);
        }

        for (int i = 0; i < localizationsOnWeb.Count; i++)
        {
            LoadLocalizationFromWebUrl(localizationsOnWeb[i]);
        }

        for (int i = 0; i < localizationsOnAssetBundle.Count; i += 4)
        {
            LoadLocalizationFromAssetBundle(localizationsOnAssetBundle[i], localizationsOnAssetBundle[i + 1], localizationsOnAssetBundle[i + 2], bool.Parse(localizationsOnAssetBundle[i + 3]));
        }
    }


    public void LoadLocalizationFromResources(string resFilePath)
    {
        if (string.IsNullOrEmpty(resFilePath))
            return;

        TextAsset localizationText = Resources.Load(resFilePath) as TextAsset;
        if (localizationText && !string.IsNullOrEmpty(localizationText.text))
        {
            LoadLocalization(localizationText.text, Path.GetFileNameWithoutExtension(resFilePath));
            Resources.UnloadAsset(localizationText);

            if (!localizationsInResources.Contains(resFilePath))
                localizationsInResources.Add(resFilePath);
        }
    }

    public void UnloadFromResources(string resFilePath)
    {
        if (localizationsInResources.Contains(resFilePath))
            localizationsInResources.Remove(resFilePath);
    }


    public void LoadLocalizationFromStreamingAssets(string assetsFilePath)
    {
        if (string.IsNullOrEmpty(assetsFilePath))
            return;

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        StartCoroutine(GoLoadLocalizationFromUrl(Application.streamingAssetsPath + assetsFilePath));
#else
        StartCoroutine(GoLoadLocalizationFromUrl("file://" + Application.streamingAssetsPath + assetsFilePath));
#endif

        if (!localizationsInStreamingAssets.Contains(assetsFilePath))
            localizationsInStreamingAssets.Add(assetsFilePath);
    }

    public void UnloadFromStreamingAssets(string assetsFilePath)
    {
        if (localizationsInStreamingAssets.Contains(assetsFilePath))
            localizationsInStreamingAssets.Remove(assetsFilePath);
    }

    public void LoadLocalizationFromWebUrl(string csvUrl)
    {
        if (string.IsNullOrEmpty(csvUrl))
            return;

        StartCoroutine(GoLoadLocalizationFromUrl(csvUrl));

        if (!localizationsOnWeb.Contains(csvUrl))
            localizationsOnWeb.Add(csvUrl);
    }

    public void UnloadFromWebUrl(string csvUrl)
    {
        if (localizationsOnWeb.Contains(csvUrl))
            localizationsOnWeb.Remove(csvUrl);
    }

    private IEnumerator GoLoadLocalizationFromUrl(string csvUrl)
    {
        WWW www = new WWW(csvUrl);
        yield return www;
        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (!string.IsNullOrEmpty(www.text))
            {
                LoadLocalization(www.text, Path.GetFileNameWithoutExtension(csvUrl));
            }
        }
        www.Dispose();
    }


    public void LoadLocalizationFromAssetBundle(string absolutePath, string bundleName, string localizationAssetName, bool unloadBundle = false)
    {
        if (!string.IsNullOrEmpty(absolutePath) && !string.IsNullOrEmpty(bundleName) && !string.IsNullOrEmpty(localizationAssetName))
            StartCoroutine(GoLoadLocalizationFromAssetBundle(absolutePath, bundleName, localizationAssetName, unloadBundle));
    }

    private IEnumerator GoLoadLocalizationFromAssetBundle(string absolutePath, string bundleName, string localizationAssetName, bool unloadBundle)
    {
        loadingFromBundle = true;

        AssetBundleManager.SetSourceAssetBundleURL(absolutePath);

        var request = AssetBundleManager.Initialize();
        if (request != null)
            yield return StartCoroutine(request);

        AssetBundleLoadAssetOperation obj = AssetBundleManager.LoadAssetAsync(bundleName, localizationAssetName, typeof(TextAsset));
        if (obj == null)
        {
            loadingFromBundle = false;
            yield break;
        }
        yield return StartCoroutine(obj);

        TextAsset textObj = obj.GetAsset<TextAsset>();
        if (textObj && !string.IsNullOrEmpty(textObj.text))
        {
            LoadLocalization(textObj.text, bundleName);
            Resources.UnloadAsset(textObj);

            if (!localizationsOnAssetBundle.Contains(bundleName) || !localizationsOnAssetBundle.Contains(localizationAssetName))
            {
                localizationsOnAssetBundle.Add(absolutePath);
                localizationsOnAssetBundle.Add(bundleName);
                localizationsOnAssetBundle.Add(localizationAssetName);
                localizationsOnAssetBundle.Add(unloadBundle.ToString());
            }
        }

        if (unloadBundle)
            AssetBundleManager.UnloadAssetBundle(bundleName);

        if (GameObject.Find("AssetBundleManager"))
            Destroy(GameObject.Find("AssetBundleManager"));

        loadingFromBundle = false;
    }

    public void UnloadFromAssetBundle(string bundleName, string localizationAssetName)
    {
        if (localizationsOnAssetBundle.Contains(bundleName) && localizationsOnAssetBundle.Contains(localizationAssetName))
        {
            int bundleNameIndex = localizationsOnAssetBundle.IndexOf(bundleName);
            int maxTimes = 10;
            while (bundleNameIndex > 0 && maxTimes > 0 && localizationsOnAssetBundle[bundleNameIndex + 1] != localizationAssetName)
            {
                bundleNameIndex = localizationsOnAssetBundle.IndexOf(bundleName, bundleNameIndex + 1);
                maxTimes--;
            }
            if (bundleNameIndex > 0 && localizationsOnAssetBundle[bundleNameIndex + 1] == localizationAssetName)
            {
                localizationsOnAssetBundle.RemoveAt(bundleNameIndex + 2);
                localizationsOnAssetBundle.RemoveAt(bundleNameIndex + 1);
                localizationsOnAssetBundle.RemoveAt(bundleNameIndex);
                localizationsOnAssetBundle.RemoveAt(bundleNameIndex - 1);
            }
        }
    }

    public void LoadLocalizationFromString(string csv)
    {
        if (!string.IsNullOrEmpty(csv))
            LoadLocalization(csv, "");
    }


    private void LoadLocalization(string csv, string bookName)
    {
        CsvFileDescription inputFileDescription = new CsvFileDescription();
        CsvContext cc = new CsvContext();
        IEnumerable<LocalizationEntry> locEntries = cc.Read<LocalizationEntry>(new StreamReader(Utils.GenerateStreamFromString(csv)), inputFileDescription);

        if (!string.IsNullOrEmpty(bookName))
            bookName = bookName + "_";

        foreach (LocalizationEntry lc in locEntries)
        {
            string locValue = "";
            if (CurrentLanguage == Language.Arabic)
                locValue = lc.Arabic;
            if (CurrentLanguage == Language.English)
                locValue = lc.English;
            if (CurrentLanguage == Language.Turkish)
                locValue = lc.Turkish;

            if (currentLocalizationEntries.ContainsKey(bookName + lc.Reference))
                currentLocalizationEntries[bookName + lc.Reference] = locValue;
            else
                currentLocalizationEntries.Add(bookName + lc.Reference, locValue);
        }

        locEntries = null;
        cc = null;
        inputFileDescription = null;

        LocalizedText[] localizedTexts = GameObject.FindObjectsOfType<LocalizedText>();
        for (int i = 0; i < localizedTexts.Length; i++)
        {
            localizedTexts[i].UpdateValue();
        }
    }


    #region GET ENTRY

    public string GetEntryRaw(string reference)
    {
        if (CurrentLanguage == null)
        {
            Debug.LogError("No language set up yet!");
            return "";
        }

        if (!string.IsNullOrEmpty(reference))
        {
            if (currentLocalizationEntries.ContainsKey(reference))
                return currentLocalizationEntries[reference];
            else
                Debug.LogError("There is no localization for `" + reference + "` language `" + CurrentLanguage + "`");
        }

        return "";
    }

    public string GetEntryRaw(string reference, string bookName)
    {
        if (string.IsNullOrEmpty(bookName))
            return GetEntryRaw(reference);
        return GetEntryRaw(bookName + "_" + reference);
    }

    public string GetEntry(string reference)
    {
        if (CurrentLanguage == null)
        {
            Debug.LogError("No language set up yet!");
            return "";
        }

        if (!string.IsNullOrEmpty(reference))
        {
            if (currentLocalizationEntries.ContainsKey(reference))
            {
                string value = currentLocalizationEntries[reference];
                if (CurrentLanguage.IsRightToLeft())
                    value = ArabicSupport.ArabicFixer.Fix(value, true, true);
                return value;
            }
            else
                Debug.LogError("There is no localization for `" + reference + "` language `" + CurrentLanguage + "`");
        }

        return "";
    }

    public string GetEntry(string reference, string bookName)
    {
        if (string.IsNullOrEmpty(bookName))
            return GetEntry(reference);
        return GetEntry(bookName + "_" + reference);
    }

    #endregion


    #region FIX ARABIC LINE BREAK

    public void FixArabicLineBreak(string text, Text label)
    {
        StartCoroutine(FixArabicLineBreakText(text, label));
    }

    private IEnumerator FixArabicLineBreakText(string text, Text label)
    {
        if (string.IsNullOrEmpty(text))
        {
            label.text = "";
        }
        else
        {
            if (Localization.Instance.CurrentLanguage.IsRightToLeft())
            {
                text = Utils.Reverse(text);
                label.text = text;

                yield return null;
                if (label.cachedTextGenerator.lineCount > 1)
                    text = Utils.ReverseLines(ConstructString(label.cachedTextGenerator, text));

                label.text = Utils.Reverse(text.Replace(System.Environment.NewLine, " ").Trim());
            }
            else
            {
                label.text = text;
            }
        }
    }

    private string ConstructString(TextGenerator tg, string initialText)
    {
        string newText = "";
        int lastCharStartIndex = -1;
        int currCharStartIntex = -1;

        for (int i = 1; i < tg.lineCount; i++)
        {
            lastCharStartIndex = tg.lines[i - 1].startCharIdx;
            currCharStartIntex = tg.lines[i].startCharIdx;

            if (i == tg.lineCount - 1)
                newText += initialText.Substring(lastCharStartIndex, currCharStartIntex - lastCharStartIndex) + System.Environment.NewLine + initialText.Substring(currCharStartIntex);
            else
                newText += initialText.Substring(lastCharStartIndex, currCharStartIntex - lastCharStartIndex) + System.Environment.NewLine;
        }
        return newText;
    }

    public void FixArabicLineBreak(string text, TypogenicText label)
    {
        if (string.IsNullOrEmpty(text))
        {
            label.Text = "";
        }
        else
        {
            if (Localization.Instance.CurrentLanguage.IsRightToLeft())
            {
                text = Utils.Reverse(text);
                text = Utils.ReverseLines(label.GetWrappedText(text));
                label.Text = Utils.Reverse(label.GetWrappedText(text));
            }
            else
            {
                label.Text = label.GetWrappedText(text);
            }
        }
    }

    #endregion
}

public static class LanguageExtensions
{
    public static bool IsRightToLeft(this Language lang)
    {
        return lang == Language.Arabic;
    }
}