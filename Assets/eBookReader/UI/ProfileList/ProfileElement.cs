using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProfileElement : MonoBehaviour
{
    [HideInInspector]
    public ProfileData MyData = null;

    public Button MyButton = null;

    public Image Background = null;

    public Image Avatar = null;

    public AvatarRenderingRequester AvatarRaw = null;

    public Text UserName = null;

    public Button EditButton = null;

    public void BindAsNewPofile(UnityAction myButtonAction)
    {
        AvatarRaw.StopAvatar();
        AvatarRaw.gameObject.SetActive(false);
        Avatar.gameObject.SetActive(true);

        UserName.text = "";
        EditButton.gameObject.SetActive(false);
        MyButton.onClick.AddListener(myButtonAction);

        MyData = null;
    }

    public void BindAsExistingProfile(ProfileData data, UnityAction myButtonAction, UnityAction editButtonAction)
    {
        MyData = data;

        AvatarRaw.gameObject.SetActive(true);
        AvatarRaw.StartAvatar(MyData.Avatar);
        Avatar.gameObject.SetActive(false);

        UserName.text = MyData.UserName;

        if (MyData.ID == ProfilesManager.Instance.CurrentProfile.ID)
        { 
            EditButton.gameObject.SetActive(true);
            EditButton.onClick.AddListener(editButtonAction);
        }

        MyButton.onClick.AddListener(myButtonAction);
    }
}
