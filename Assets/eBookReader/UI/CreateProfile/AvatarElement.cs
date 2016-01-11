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
        CreateProfilePanel.Instance.SelectedAvatarImage.overrideSprite = MyImage.sprite;
        CreateProfilePanel.Instance.NewProfile.Avatar = MyImage.sprite.name;
    }
}
