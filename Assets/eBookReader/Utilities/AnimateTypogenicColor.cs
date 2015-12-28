using UnityEngine;
using System.Collections;

public class AnimateTypogenicColor : MonoBehaviour
{
    public TypogenicText Text = null;

    public bool AnimateOnEnable = true;

    public Color From = Color.black;

    public Color To = Color.white;

    public float Duration = 0.25f;

    void OnEnable()
    {
        StartCoroutine(DoAnimate());
    }

    IEnumerator DoAnimate()
    {
        float time = Time.timeSinceLevelLoad;
        while(Time.timeSinceLevelLoad - time < Duration)
        {
            Text.ColorTopLeft = Color.Lerp(From, To, (Time.timeSinceLevelLoad - time) / Duration);

            yield return null;
        }

        yield break;
    }
}
