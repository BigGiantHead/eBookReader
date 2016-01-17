using UnityEngine;
using System.Collections;
using System;

public class ModalPanel : MonoBehaviour
{
    public GameObject Content = null;

    public bool InitialyHidden = true;

    public Action OnHideStart = null;

    public Action OnHideEnd = null;

    public Action OnShowStart = null;

    public Action OnShowEnd = null;

    void Awake()
    {
        if (Content != null)
        {
            Content.SetActive(false);
        }
    }

    // Use this for initialization
    void Start()
    {
        if (InitialyHidden)
        {
            HideImmidiate();
        }
        else
        {
            Show();
        }
    }

    public void Show()
    {
        if (OnShowStart != null)
        {
            OnShowStart();
        }

        gameObject.SetActive(true);

        LTDescr scale = LeanTween.scale(gameObject, Vector3.one, 1f);
        scale.setEase(LeanTweenType.easeOutExpo);
        scale.onComplete = OnShowComplete;
    }


    public void HideImmidiate()
    {
        transform.localScale = Vector3.one * 0.001f;
        if (Content != null)
        {
            Content.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    public void Hide()
    { 
        if (OnHideStart != null)
        {
            OnHideStart();
        }

        LTDescr scale = LeanTween.scale(gameObject, Vector3.one * 0.001f, 0.25f);
        scale.setEase(LeanTweenType.easeInSine);
        scale.onComplete = OnHideComplete;
    }

    private void OnHideComplete()
    {
        gameObject.SetActive(false);

        if (Content != null)
        {
            Content.SetActive(false);
        }

        if (OnHideEnd != null)
        {
            OnHideEnd();
        }
    }

    private void OnShowComplete()
    {
        if (Content != null)
        {
            Content.SetActive(true);
        }

        if (OnShowEnd != null)
        {
            OnShowEnd();
        }
    }
}
