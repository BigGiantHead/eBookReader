using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LocalizedFont : MonoBehaviour
{
	private void Start()
    {
        UpdateFont();
	}

    private void OnEnable()
    {
        UpdateFont();
    }

    public void UpdateFont()
    {
        Text labelText = GetComponent<Text>();
        if (labelText)
        {
            labelText.font = Localization.Instance.CurrentLanguage.FontReferences().font;
            labelText = null;
        }
        else
        {
            TypogenicText tText = GetComponent<TypogenicText>();
            if (tText)
            {
                tText.GetComponent<MeshRenderer>().material = Localization.Instance.CurrentLanguage.FontReferences().fontMat;
                tText.Font = Localization.Instance.CurrentLanguage.FontReferences().fontTypo;
                tText = null;
            }
        }
    }
}
