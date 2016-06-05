using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AvatarItem : MonoBehaviour
{
    public Button MyButton = null;

    public Image ElementImage = null;

    public Image BackgroundImage = null;

    public Sprite SelectedSprite = null;

    [HideInInspector]
    public int Index = 0;

    // Use this for initialization
    void Start()
    {
    }

    public void Select(int index)
    {
        if (Index == index)
        {
            BackgroundImage.overrideSprite = SelectedSprite;
        }
        else
        {
            BackgroundImage.overrideSprite = null;
        }
    }
}
