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

    public Image Avatar = null;

    public ModalPanel MyPanel = null;

    public ModalPanel ProfilesPanel = null;

    public ModalPanel NewProfilePanel = null;

    void Awake()
    {
        instance = this;
        MyPanel.OnShowStart = OnShowStart;
    }

    // Use this for initialization
    void Start()
    {
        MyPanel.Show();
    }

    void OnShowStart()
    {
        if (ProfilesManager.Instance.CurrentProfile == null)
        {
            Avatar.sprite = Resources.Load<Sprite>("Guest_Avatar");
        }
        else
        {
            Avatar.sprite = Resources.Load<Sprite>("Avatars/" + ProfilesManager.Instance.CurrentProfile.Avatar);
        }
    }

    public void OnProfileClick()
    {
        MyPanel.Hide();

        if (ProfilesManager.Instance.Profiles.Count == 0)
        {
            NewProfilePanel.Show();
        }
        else
        {
            ProfilesPanel.Show();
        }
    }

    public void OnPickBookClick()
    {
        MyPanel.Hide();

        PickBook.Instance.MyPanel.Show();
    }
}
