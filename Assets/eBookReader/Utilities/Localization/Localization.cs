using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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

    public string localizationFileResPath = "Localization";

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
        TextAsset localizationText = Resources.Load(localizationFileResPath) as TextAsset;

        if (localizationText)
        {
            currentLocalizationEntries.Clear();

            int currentLangIndex = -1;
            string[] locLines = localizationText.text.Split('\n');
            for (int i = 0; i < locLines.Length; i++)
            {
                locLines[i] = locLines[i].Trim();
                if (!string.IsNullOrEmpty(locLines[i]))
                {
                    if (i == 0)
                    {
                        string[] locCol = locLines[i].Split(',');
                        for (int k = 1; k < locCol.Length; k++)
                        {
                            if (locCol[k] == CurrentLanguage.ToString())
                                currentLangIndex = k;
                        }
                    }
                    else
                    {
                        string locKey = "";
                        string locValue = "";

                        string[] locCol = locLines[i].Split(',');
                        string currentVal = "";
                        int j = 0;
                        int k = 0;
                        while (k < locCol.Length)
                        {
                            currentVal = locCol[k];
                            if (currentVal.StartsWith("\""))
                            {
                                bool isLast = currentVal.EndsWith("\"");
                                while (!isLast)
                                {
                                    k++;
                                    currentVal += "," + locCol[k];
                                    isLast = locCol[k].EndsWith("\"");
                                }
                                currentVal = currentVal.Substring(1, currentVal.Length - 2);
                            }
                            currentVal = currentVal.Replace("\"\"", "\"");

                            if (j == 0)
                            {
                                locKey = currentVal;
                            }
                            else if (j == currentLangIndex)
                            {
                                locValue = currentVal;
                                break;
                            }

                            j++;
                            k++;
                        }

                        currentLocalizationEntries.Add(locKey, locValue);
                    }
                }
            }

            Resources.UnloadAsset(localizationText);
        }
    }

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
}

public static class LanguageExtensions
{
    public static bool IsRightToLeft(this Language lang)
    {
        return lang == Language.Arabic;
    }
}