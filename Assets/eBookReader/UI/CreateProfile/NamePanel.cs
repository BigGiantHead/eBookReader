using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class NamePanel : MonoBehaviour
{
    private int currentIndex = 0;

    private List<Text> userNameLetters = null;

    [Header("User Name Container")]
    public GameObject UserNameLetterContainerPrefab = null;

    public HorizontalLayoutGroup UserNameList = null;

    [Header("Letters Container")]
    public char[] Characters = null;

    public Text LetterPrefab = null;

    public GridLayoutGroup LettersList = null;

    public string UserName
    {
        set
        {
            string name = value;
            currentIndex = 0;

            if (string.IsNullOrEmpty(name))
            {
                for (int i = 0; i < userNameLetters.Count; ++i)
                {
                    userNameLetters[i].text = "";
                }
            }
            else
            {
                for (int i = 0; i < userNameLetters.Count; ++i)
                {
                    if (name.Length > i)
                    {
                        userNameLetters[i].text = name[i].ToString().ToUpperInvariant();
                        currentIndex = i + 1;
                    }
                    else
                    {
                        userNameLetters[i].text = "";
                    }
                }
            }            
        }
        get
        {
            string name = "";

            for (int i = 0; i < userNameLetters.Count; ++i)
            {
                name += userNameLetters[i].text;
            }

            return name;
        }
    }

    void Awake()
    {
        UserNameLetterContainerPrefab.transform.SetParent(transform.parent);
        LetterPrefab.transform.SetParent(transform.parent);

        LettersList.transform.ClearChildren();

        for (int i = 0; i < Characters.Length; ++i)
        {
            GameObject letter = Instantiate(LetterPrefab.gameObject);
            letter.transform.SetParent(LettersList.transform);
            letter.transform.localPosition = Vector3.zero;
            letter.transform.localRotation = Quaternion.identity;
            letter.transform.localScale = Vector3.one;

            letter.SetActive(true);

            Text letterText = letter.GetComponent<Text>();
            letterText.text = Characters[i].ToString().ToUpperInvariant();

            Button button = letter.GetComponent<Button>();
            button.onClick.AddListener(() => { LetterPressed(letterText.text); });
        }

        currentIndex = 0;

        UserNameList.transform.ClearChildren();

        userNameLetters = new List<Text>(9);
        userNameLetters.Clear();

        for (int i = 0; i < 9; ++i)
        {
            GameObject letterContainer = Instantiate(UserNameLetterContainerPrefab);
            letterContainer.transform.SetParent(UserNameList.transform);
            letterContainer.transform.localPosition = Vector3.zero;
            letterContainer.transform.localRotation = Quaternion.identity;
            letterContainer.transform.localScale = Vector3.one;

            letterContainer.SetActive(true);

            userNameLetters.Add(letterContainer.GetComponentInChildren<Text>());
        }
    }

    // Use this for initialization
    void OnEnable()
    {
        
    }

    public void LetterPressed(string letter)
    {
        if (currentIndex < userNameLetters.Count)
        {
            userNameLetters[currentIndex++].text = letter;
        }
    }

    public void ClearUserName()
    {
        currentIndex = 0;
        for (int i = 0; i < userNameLetters.Count; ++i)
        {
            userNameLetters[i].text = "";
        }
    }
}
