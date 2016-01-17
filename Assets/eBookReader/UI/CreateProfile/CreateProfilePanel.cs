using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateProfilePanel : MonoBehaviour
{
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

    public Image SelectedAvatarImage = null;

    public Sprite DefaultAvatar = null;

    [HideInInspector]
    public Sprite Avatar = null;

    [HideInInspector]
    public string Name = null;

    public GameObject LeftArrow = null;

    public GameObject RightArrow = null;

    public ScrollRect ContentScroll = null;

    public float ContentScrolStep = 1;

    [HideInInspector]
    public ProfileData NewProfile = null;

    public PasswordFruitPosition[] Password = null;

    public ConfirmProfilePanel ConfirmProfilePanel = null;

    void Awake()
    {
        instance = this;
        MyPanel.OnShowStart = OnShowStart;
        MyPanel.OnShowEnd = OnShowEnd;
    }

    // Use this for initialization
    void Start()
    {
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
        ResetPassword();
        NewProfile = new ProfileData();
        Avatar = null;
        SelectedAvatarImage.overrideSprite = null;
        Name = "";
        currentWindow = CreateProfilePanelWindow.PickAvatar;
        targetScroll = 0;
        ContentScroll.horizontalNormalizedPosition = 0;
        LeftArrow.SetActive(false);
        RightArrow.SetActive(true);
    }

    private void OnShowEnd()
    {
    }

    public void NextWindow()
    {
        bool gotonext = true;

        switch (currentWindow)
        {
            case CreateProfilePanelWindow.PickAvatar:
                gotonext = !string.IsNullOrEmpty(NewProfile.Avatar);

                break;

            case CreateProfilePanelWindow.PickPassword:
                
                for (int i = 0; i < Password.Length; ++i)
                {
                    if (Password[i].MyItem == null)
                    {
                        gotonext = false;
                        break;
                    }
                }

                if (gotonext)
                {
                    NewProfile.Password.FirstFruit = Password[0].MyItem.Type;
                    NewProfile.Password.SecondFruit = Password[1].MyItem.Type;
                    NewProfile.Password.ThirdFruit = Password[2].MyItem.Type;
                    NewProfile.Password.FourthFruit = Password[3].MyItem.Type;
                }

                break;
        }

        if (gotonext)
        {
            targetScroll = Mathf.Clamp01(targetScroll + ContentScrolStep);
            currentWindow = (CreateProfilePanelWindow)Mathf.Clamp((int)currentWindow + 1, 0, 2);
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
        currentWindow = (CreateProfilePanelWindow)Mathf.Clamp((int)currentWindow - 1, 0, 2);
                
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

    public enum CreateProfilePanelWindow
    {
        PickAvatar = 0,

        PickPassword = 1,

        ConfirmProfile = 2
    }

    public void ResetPassword()
    {
        for (int i = 0; i < Password.Length; ++i)
        {
            if (Password[i].MyItem != null)
            {
                Password[i].ResetItem();
            }
        }
    }

    public void CreateNewProfile()
    {
        ProfilesManager.Instance.AddProfile(NewProfile);
        ProfilesManager.Instance.CurrentProfile = NewProfile;
        MyPanel.Hide();
        CurrentProfileElement.Instance.MyPanel.Show();
    }
}
