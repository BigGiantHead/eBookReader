using UnityEngine;
using System.Collections;

public class EnterPassword : MonoBehaviour
{
    private static EnterPassword instance = null;

    public static EnterPassword Instance
    {
        get
        {
            return instance;
        }
    }

    public ModalPanel MyPanel = null;

    public PasswordFruitPosition[] Password = null;

    public RectTransform PasswordContainer = null;

    public AnimationCurve ShakeCurve = null;

    [HideInInspector]
    public ProfileData TargetData = null;

    void Awake()
    {
        instance = this;
        MyPanel.OnShowStart = ResetPassword;
    }

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < Password.Length; ++i)
        {
            Password[i].PasswordFruitPositionChanged = PasswordUpdated;
        }
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

    private void PasswordUpdated()
    {
        bool isEqual = true;
        bool shake = true;

        for (int i = 0; i < Password.Length; ++i)
        {
            if (Password[i].MyItem == null)
            {
                isEqual = false;
                shake = false;
            }
            else if (Password[i].MyItem.Type != TargetData.Password[i])
            {
                isEqual = false;
            }
        }

        if (isEqual)
        {
            MyPanel.Hide();
            if (TargetData != null)
            {
                TargetData.LastUsed = System.DateTime.Now.Ticks;
            }
            ProfilesManager.Instance.UpdateProfiles();
            ProfilesManager.Instance.CurrentProfile = TargetData;
            CurrentProfileElement.Instance.MyPanel.Show();
        }
        else if (shake)
        {
            StartCoroutine(DoShake());
        }
    }

    private IEnumerator DoShake()
    {
        float duration = 0.25f;
        float time = Time.timeSinceLevelLoad;
        Vector3 startPos = PasswordContainer.localPosition;
        Vector3 pos = startPos;

        while (Time.timeSinceLevelLoad - time < duration)
        {
            float value = ShakeCurve.Evaluate((Time.timeSinceLevelLoad - time) / duration);

            pos.x = startPos.x + value * 10f;
            PasswordContainer.localPosition = pos;

            yield return null;
        }

        PasswordContainer.localPosition = startPos;

        yield break;
    }
}
