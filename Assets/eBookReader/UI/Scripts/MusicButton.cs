using UnityEngine;
using System.Collections;

public class MusicButton : MonoBehaviour
{
    public AudioSource MusicSource = null;

    // Use this for initialization
    void Start()
    {
    }

    public void TriggerMusic()
    {
        if (MusicSource.isPlaying)
        {          
            LTDescr ldscr = LeanTween.value(gameObject, MusicSource.volume, 0, MusicSource.volume / 1);
            ldscr.setOnUpdate(OnUpdateVolume);
            ldscr.setOnComplete(() => { MusicSource.Stop(); });
        }
        else
        {
            MusicSource.Stop();
            MusicSource.Play();
            MusicSource.volume = 0;

            LTDescr ldscr = LeanTween.value(gameObject, 0, 1, 1);
            ldscr.setOnUpdate(OnUpdateVolume);
        }
    }

    private void OnUpdateVolume(float volume)
    {
        MusicSource.volume = volume;
    }
}
