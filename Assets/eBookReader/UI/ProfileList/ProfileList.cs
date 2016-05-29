using UnityEngine;
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

    public HorizontalLayoutGroup List = null;

    public Sprite CurrentBackground = null;

    void Awake()
    {
        instance = this;
        MyPanel.OnShowEnd = PopulateList;
    }

    private void PopulateList()
    {        
        Layout.ClearChildren();

        //Create Current Profile Button
        ProfileElement pe = CreateProfileElement(ProfilesManager.Instance.CurrentProfile);
        pe.Background.sprite = CurrentBackground;

        //Create New Pfofile Button
        CreateProfileElement(null, "New Profile +");

        foreach (ProfileData data in ProfilesManager.Instance.Profiles)
        {
            if (data != ProfilesManager.Instance.CurrentProfile)
            {
                CreateProfileElement(data);
            }
        }

        LayoutElement profileSampleLayoutElement = ProfileSample.GetComponent<LayoutElement>();
        Vector2 size = Layout.sizeDelta;
        size.x = (ProfilesManager.Instance.Profiles.Count + 1) * profileSampleLayoutElement.preferredWidth + (ProfilesManager.Instance.Profiles.Count) * List.spacing;
        Layout.sizeDelta = size;

        Vector3 position = Layout.localPosition;
        position.x = size.x / 2f;
        Layout.localPosition = position;
    }

    private ProfileElement CreateProfileElement(ProfileData data, string overrideUserName = null)
    {
        //TODO: refactor avatar
        GameObject profile = Instantiate(ProfileSample) as GameObject;
        ProfileElement profileElement = profile.GetComponent<ProfileElement>();

        if (data != null)
        {
            profileElement.Avatar.sprite = Resources.Load<Sprite>("Avatars/" + data.Avatar);
            profileElement.Avatar.enabled = true;
            profileElement.UserName.text = data.UserName;
        }
        if (string.IsNullOrEmpty(overrideUserName))
        {
            profileElement.UserName.text = overrideUserName;
        }

        profileElement.MyData = data;

        Button profileButton = profile.GetComponent<Button>();
        profileButton.onClick.AddListener(() =>
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
        });

        profile.transform.SetParent(Layout);

        profile.transform.localScale = Vector3.one;
        profile.transform.localPosition = Vector3.zero;
        profile.transform.localRotation = Quaternion.identity;

        profile.SetActive(true);

        return profileElement;
    }
}
