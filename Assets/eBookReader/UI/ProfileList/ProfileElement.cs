using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class ProfileElement : MonoBehaviour
{
    [HideInInspector]
    public ProfileData MyData = null;

    public Image Avatar = null;

    public Text UserName = null;

    public ModalPanel ProfilesPanel = null;

    public ModalPanel CurrentUserPanel = null;

    public void OnClick()
    {
        if (MyData != null)
        {
            MyData.LastUsed = DateTime.Now.Ticks;
        }
        ProfilesManager.Instance.CurrentProfile = MyData;

        CurrentUserPanel.Show();
        ProfilesPanel.Hide();
    }
}
