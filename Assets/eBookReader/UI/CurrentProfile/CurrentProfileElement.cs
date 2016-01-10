using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurrentProfileElement : MonoBehaviour
{
    private ProfileData currentProfile = null;

    public Image Avatar = null;

    public Text UserName = null;

    public ModalPanel MyPanel = null;

    public ModalPanel ProfilesPanel = null;

    public ModalPanel NewProfilePanel = null;

    void Awake()
    {
        MyPanel.OnShowStart = OnShowStart;
    }

    // Use this for initialization
    void Start()
    {
        MyPanel.Show();
    }

    void OnShowStart()
    {
        currentProfile = ProfilesManager.Instance.CurrentProfile;
        if (currentProfile == null)
        {
            Avatar.sprite = Resources.Load<Sprite>("Guest_Avatar");
            UserName.text = "New Profile +";
        }
        else
        {
            Avatar.sprite = Resources.Load<Sprite>("Avatars/" + currentProfile.Avatar);
            UserName.text = currentProfile.UserName;
        }
    }

    public void OnClick()
    {
        MyPanel.Hide();
        ProfilesPanel.Show();

        if (currentProfile == null)
        {
            //Show new profile panel
        }
        else
        {
            //Show change profile panel or profile settings panel
        }
    }
}
