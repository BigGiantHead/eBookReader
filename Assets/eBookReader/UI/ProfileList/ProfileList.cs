using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ProfileList : MonoBehaviour
{
    public GameObject ProfileSample = null;

    public RectTransform Layout = null;

    // Use this for initialization
    void Start()
    {
        {
            GameObject profile = Instantiate(ProfileSample) as GameObject;
            ProfileElement profileElem = profile.GetComponent<ProfileElement>();
            profileElem.Avatar.sprite = Resources.Load<Sprite>("Guest_Avatar");
            profileElem.UserName.text = "New Profile +";
            profileElem.MyData = null;

            profile.transform.SetParent(Layout);
            profile.SetActive(true);
        }

        foreach (ProfileData data in ProfilesManager.Instance.Profiles)
        {
            GameObject profile = Instantiate(ProfileSample) as GameObject;
            ProfileElement profileElem = profile.GetComponent<ProfileElement>();
            profileElem.Avatar.sprite = Resources.Load<Sprite>("Avatars/" + data.Avatar);
            profileElem.UserName.text = data.UserName;
            profileElem.MyData = data;

            profile.transform.SetParent(Layout);
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
