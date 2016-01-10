using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateProfilePanel : MonoBehaviour
{
    public static CreateProfilePanel instance = null;

    public static CreateProfilePanel Instance
    {
        get
        {
            return instance;
        }
    }

    public ModalPanel MyPanel = null;

    public Image SelectedAvatarImage = null;

    public Sprite DefaultAvatar = null;

    [HideInInspector]
    public Sprite Avatar = null;

    [HideInInspector]
    public string Name = null;

    void Awake()
    {
        instance = this;
        MyPanel.OnShowStart = OnShowStart;
    }

    // Use this for initialization
    void Start()
    {

    }

    private void OnShowStart()
    {
        Avatar = null;
        SelectedAvatarImage.overrideSprite = null;
        Name = "";
    }
}
