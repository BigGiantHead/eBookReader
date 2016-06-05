using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurrentProfileElement : MonoBehaviour
{
    private static CurrentProfileElement instance = null;

    public static CurrentProfileElement Instance
    {
        get
        {
            return instance;
        }
    }

    public AvatarRenderingRequester Avatar = null;

    public ModalPanel MyPanel = null;

    public ModalPanel ProfilesPanel = null;

    public ModalPanel NewProfilePanel = null;

    void Awake()
    {
        instance = this;
        MyPanel.OnShowStart = OnShowEnd;
        MyPanel.OnHideEnd = OnHideEnd;
    }

    // Use this for initialization
    void Start()
    {
        MyPanel.Show();
    }

    void OnShowEnd()
    {
        if (ProfilesManager.Instance.CurrentProfile != null)
        {
            Avatar.StartAvatar(ProfilesManager.Instance.CurrentProfile.Avatar);
        }
    }

    void OnHideEnd()
    {
        Avatar.StopAvatar();
    }

    public void OnProfileClick()
    {
        MyPanel.Hide();

        ProfilesPanel.Show();
    }

    public void OnPickBookClick()
    {
        MyPanel.Hide();

        PickBook.Instance.MyPanel.Show();
    }
}
