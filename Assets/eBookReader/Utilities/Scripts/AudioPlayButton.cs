using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioPlayButton : MonoBehaviour
{
    public Image ButtonImage = null;

    public Button Button = null;

    public AudioSource Audio = null;

    public Sprite StopAudioSprite = null;

    public RectTransform CanvasRect = null;
    
    // Use this for initialization
    void Start()
    {
    }

    public void OnClick()
    {
        if (Audio.isPlaying)
        {
            StopAudio();
        }
        else
        {
            PlayAudio();
        }
    }

    public void PlayAudio()
    {
        BookGenerator.Instance.PageObjectsRoot.BroadcastMessage("StopAudio");

        ButtonImage.overrideSprite = StopAudioSprite;
        Audio.Play();
        StartCoroutine("DoWaitTillEnd");

    }

    public void StopAudio()
    {
        ButtonImage.overrideSprite = null;
        Audio.Stop();
        StopAllCoroutines();
    }

    private IEnumerator DoWaitTillEnd()
    {
        float time = Time.timeSinceLevelLoad;
        while (Time.timeSinceLevelLoad - time < Audio.clip.length)
        {
            yield return null;
        }

        StopAudio();

        yield break;
    }

    private void OnEnable()
    {
        StopAudio();
    }
}
