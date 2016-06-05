using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProfileList : MonoBehaviour
{
    private static ProfileList instance = null;

    public static ProfileList Instance
    {
        get
        {
            return instance;
        }
    }

    public GameObject ProfileSample = null;

    public RectTransform Layout = null;

    public ModalPanel MyPanel = null;

    //public GridLayoutGroup List = null;

    public Sprite CurrentBackground = null;

    void Awake()
    {
        instance = this;
        MyPanel.OnShowEnd = PopulateList;
    }

    private void PopulateList()
    {        
        Layout.ClearChildren();

        if (ProfilesManager.Instance.CurrentProfile != null)
        {
            ProfileElement pe = CreateProfileElement(ProfilesManager.Instance.CurrentProfile);
            pe.Background.sprite = CurrentBackground;
        }

        foreach (ProfileData data in ProfilesManager.Instance.Profiles)
        {
            if (data != ProfilesManager.Instance.CurrentProfile)
            {
                CreateProfileElement(data);
            }
        }

        CreateProfileElement(null);
    }

    private ProfileElement CreateProfileElement(ProfileData data)
    {
        GameObject profile = Instantiate(ProfileSample) as GameObject;
        ProfileElement profileElement = profile.GetComponent<ProfileElement>();
        UnityAction profileAction = () =>
        {
            MyPanel.Hide();
            if (profileElement.MyData == null)
            {
                CreateProfilePanel.Instance.MyPanel.Show();
            }
            else if (profileElement.MyData == ProfilesManager.Instance.CurrentProfile)
            {
                CurrentProfileElement.Instance.MyPanel.Show();
            }
            else
            {
                EnterPassword.Instance.TargetData = profileElement.MyData;
                EnterPassword.Instance.MyPanel.Show();
            }
        };

        if (data != null)
        {
            UnityAction editAction = () =>
            {
                MyPanel.Hide();

                CreateProfilePanel.Instance.Profile = data;
                CreateProfilePanel.Instance.MyPanel.Show();
            };

            profileElement.BindAsExistingProfile(data, profileAction, editAction);
        }
        else
        {
            profileElement.BindAsNewPofile(profileAction);
        }

        profile.transform.SetParent(Layout);

        profile.transform.localScale = Vector3.one;
        profile.transform.localPosition = Vector3.zero;
        profile.transform.localRotation = Quaternion.identity;

        profile.SetActive(true);

        return profileElement;
    }
}
