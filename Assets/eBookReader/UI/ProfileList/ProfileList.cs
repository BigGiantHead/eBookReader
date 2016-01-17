using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

    void Awake()
    {
        instance = this;
        MyPanel.OnShowEnd = PopulateList;
    }

    void PopulateList()
    {
        Layout.ClearChildren();

        {
            GameObject profile = Instantiate(ProfileSample) as GameObject;
            ProfileElement profileElem = profile.GetComponent<ProfileElement>();
            profileElem.Avatar.sprite = Resources.Load<Sprite>("Guest_Avatar");
            profileElem.UserName.text = "New Profile +";
            profileElem.MyData = null;
            
            Button profileButton = profile.GetComponent<Button>();
            profileButton.onClick.AddListener(() => 
            {
                MyPanel.Hide();
                CreateProfilePanel.Instance.MyPanel.Show();
            });

            profile.transform.SetParent(Layout);

            profile.transform.localScale = Vector3.one;
            profile.transform.localPosition = Vector3.zero;
            profile.transform.localRotation = Quaternion.identity;

            profile.SetActive(true);
        }

        foreach (ProfileData data in ProfilesManager.Instance.Profiles)
        {
            GameObject profile = Instantiate(ProfileSample) as GameObject;
            ProfileElement profileElem = profile.GetComponent<ProfileElement>();
            profileElem.Avatar.sprite = Resources.Load<Sprite>("Avatars/" + data.Avatar);
            profileElem.UserName.text = data.UserName;
            profileElem.MyData = data;

            Button profileButton = profile.GetComponent<Button>();
            profileButton.onClick.AddListener(() =>
            {
                MyPanel.Hide();
                if (profileElem.MyData != null)
                {
                    profileElem.MyData.LastUsed = System.DateTime.Now.Ticks;
                }
                ProfilesManager.Instance.UpdateProfiles();
                ProfilesManager.Instance.CurrentProfile = profileElem.MyData;
                CurrentProfileElement.Instance.MyPanel.Show();
            });

            profile.transform.SetParent(Layout);

            profile.transform.localScale = Vector3.one;
            profile.transform.localPosition = Vector3.zero;
            profile.transform.localRotation = Quaternion.identity;

            profile.SetActive(true);
        }

        Vector2 size = Layout.sizeDelta;
        size.x = (ProfilesManager.Instance.Profiles.Count + 1) * 325 + (ProfilesManager.Instance.Profiles.Count) * 50;
        Layout.sizeDelta = size;

        Vector3 position = Layout.localPosition;
        position.x = size.x / 2f;
        Layout.localPosition = position;
    }
}
