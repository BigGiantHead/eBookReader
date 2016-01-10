using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AvatarElement : MonoBehaviour
{
    public Image MyImage = null;

    // Use this for initialization
    void Start()
    {

    }

    public void OnClick()
    {
        CreateProfilePanel.instance.SelectedAvatarImage.overrideSprite = MyImage.sprite;
    }
}
