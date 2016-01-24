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
            Stop();
        }
        else
        {
            Play();
        }
    }

    public void Play()
    {
        ButtonImage.overrideSprite = StopAudioSprite;
        Audio.Play();
        StartCoroutine("DoWaitTillEnd");

    }

    public void Stop()
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

        Stop();

        yield break;
    }

    private void OnEnable()
    {
        Stop();
    }
}
