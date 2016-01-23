using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour {

    public string Reference = "";
    public string BookName = "";

    void Start()
    {
        UpdateValue();
    }

    [ContextMenu("Update Value")]
    public void UpdateValue()
    {
        if (string.IsNullOrEmpty(Reference))
            return;

        string text = Localization.Instance.GetEntry(Reference, BookName);

        Text labelText = GetComponent<Text>();
        if (labelText)
        {
            Localization.Instance.FixArabicLineBreak(text, labelText);
        }
        else
        {
            TypogenicText tText = GetComponent<TypogenicText>();
            if (tText)
                Localization.Instance.FixArabicLineBreak(text, tText);
        }
    }

    private void OnEnable()
    {
        UpdateValue();
    }

    public void UpdateReference(string value, string bookName)
    {
        Reference = value;
        BookName = bookName;
        UpdateValue();
    }

    public void UpdateReference(string value)
    {
        Reference = value;
        UpdateValue();
    }
}
