using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateProfilePanel : MonoBehaviour
{
    private ProfileData localProfile = null;

    private CreateProfilePanelWindow currentWindow = CreateProfilePanelWindow.PickAvatar;

    private float targetScroll = 0;

    private static CreateProfilePanel instance = null;

    public static CreateProfilePanel Instance
    {
        get
        {
            return instance;
        }
    }

    public ModalPanel MyPanel = null;

    public Sprite DefaultAvatar = null;

    public GameObject LeftArrow = null;

    public GameObject RightArrow = null;

    public ScrollRect ContentScroll = null;

    public float ContentScrolStep = 1;

    [Header("Panels")]
    public ConfirmProfilePanel ConfirmProfilePanel = null;

    public PickAvatar PickAvatarPanel = null;

    public NamePanel NamePanel = null;

    public PickPassword PasswordPanel = null;

    [HideInInspector]
    public ProfileData Profile = null;

    public enum CreateProfilePanelWindow
    {
        PickAvatar = 0,

        PickName = 1,

        PickPassword = 2,

        ConfirmProfile = 3
    }

    void Awake()
    {
        instance = this;
        MyPanel.OnShowStart = OnShowStart;
        MyPanel.OnShowEnd = OnShowEnd;
        MyPanel.OnHideEnd = OnHideEnd;
    }

    // Use this for initialization
    void Start()
    {
        if (ProfilesManager.Instance.CurrentProfile != null)
        {
            MyPanel.Hide();
            EnterPassword.Instance.TargetData = ProfilesManager.Instance.CurrentProfile;
            EnterPassword.Instance.MyPanel.Show();
        }
    }

    void OnEnable()
    {
    }

    void Update()
    {
        ContentScroll.horizontalNormalizedPosition = Mathf.Lerp(ContentScroll.horizontalNormalizedPosition, targetScroll, Time.deltaTime * 10f);
    }

    private void OnShowStart()
    {
        if (Profile == null)
        {
            Profile = new ProfileData();
        }

        PasswordPanel.ResetPassword();

        currentWindow = CreateProfilePanelWindow.PickAvatar;
        targetScroll = 0;
        ContentScroll.horizontalNormalizedPosition = 0;
        LeftArrow.SetActive(false);
        RightArrow.SetActive(true);

    }

    private void OnShowEnd()
    {
        PickAvatarPanel.RebindWithAvatar(Profile.Avatar);
        NamePanel.UserName = Profile.UserName;
    }

    private void OnHideEnd()
    {
        Profile = null;
        PickAvatarPanel.ClearWindow();
        ConfirmProfilePanel.ClearWindow();
    }

    public void NextWindow()
    {
        bool gotonext = true;

        switch (currentWindow)
        {
            case CreateProfilePanelWindow.PickName:
                gotonext = NamePanel.UserName.Length >= 3;
                if (gotonext)
                {
                    PasswordPanel.ResetPassword();
                    PasswordPanel.Password = Profile.Password;
                }
                break;

            case CreateProfilePanelWindow.PickPassword:
                gotonext = PasswordPanel.IsComplete;

                break;
        }

        if (gotonext)
        {
            targetScroll = Mathf.Clamp01(targetScroll + ContentScrolStep);
            currentWindow = (CreateProfilePanelWindow)Mathf.Clamp((int)currentWindow + 1, 0, 3);
        }

        if (currentWindow == CreateProfilePanelWindow.ConfirmProfile)
        {
            RightArrow.SetActive(false);
            ConfirmProfilePanel.Bind();
        }
        else
        {
            RightArrow.SetActive(true);
        }

        if (targetScroll > 0)
        {
            LeftArrow.SetActive(true);
        }
        else
        {
            LeftArrow.SetActive(false);
        }
    }

    public void PreviousWindow()
    {
        targetScroll = Mathf.Clamp01(targetScroll - ContentScrolStep);
        currentWindow = (CreateProfilePanelWindow)Mathf.Clamp((int)currentWindow - 1, 0, 3);
                
        if (!RightArrow.activeSelf)
        {
            RightArrow.SetActive(true);
        }

        if (targetScroll == 0)
        {
            LeftArrow.SetActive(false);
        }
        else
        {
            LeftArrow.SetActive(true);
        }
    }

    public void CreateNewProfile()
    {
        Profile.UserName = NamePanel.UserName;
        Profile.Password = PasswordPanel.Password;

        ProfilesManager.Instance.AddProfile(Profile);
        ProfilesManager.Instance.CurrentProfile = Profile;

        MyPanel.Hide();

        CurrentProfileElement.Instance.MyPanel.Show();
    }
}
