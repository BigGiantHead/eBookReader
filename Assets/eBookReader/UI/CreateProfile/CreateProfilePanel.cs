using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateProfilePanel : MonoBehaviour
{
    private float targetScroll = 0;

    public static CreateProfilePanel instance = null;

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
        LeftArrow.SetActive(false);
    }

    void Update()
    {
        ContentScroll.horizontalNormalizedPosition = Mathf.Lerp(ContentScroll.horizontalNormalizedPosition, targetScroll, Time.deltaTime * 10f);
    }

    private void OnShowStart()
    {
        Avatar = null;
        SelectedAvatarImage.overrideSprite = null;
        Name = "";
    }

    private void OnShowEnd()
    {
    }

    public void NextWindow()
    {
        targetScroll = Mathf.Clamp01(targetScroll + ContentScrolStep);
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
        if (targetScroll == 0)
        {
            LeftArrow.SetActive(false);
        }
        else
        {
            LeftArrow.SetActive(true);
        }
    }
}
