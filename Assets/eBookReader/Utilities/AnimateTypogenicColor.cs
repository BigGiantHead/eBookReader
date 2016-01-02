using UnityEngine;
using System.Collections;

public class AnimateTypogenicColor : MonoBehaviour
{
    private Color colorTopLeft, colorTopRight, colorBottomLeft, colorBottomRight;

    public TypogenicText Text = null;

    public float Duration = 0.25f;

    void Start()
    {
        colorTopLeft = Text.ColorTopLeft;
        colorTopRight = Text.ColorTopRight;
        colorBottomLeft = Text.ColorBottomLeft;
        colorBottomRight = Text.ColorBottomRight;
    }

    void OnEnable()
    {
        StartCoroutine(DoAnimate());
    }

    IEnumerator DoAnimate()
    {
        colorTopLeft.a = 0;
        colorTopRight.a = 0;
        colorBottomLeft.a = 0;
        colorBottomRight.a = 0;

        float time = Time.timeSinceLevelLoad;
        while (Time.timeSinceLevelLoad - time < Duration)
        {
            colorTopRight.a =
            colorBottomLeft.a =
            colorBottomRight.a =
            colorTopLeft.a = Mathf.Lerp(0, 1, (Time.timeSinceLevelLoad - time) / Duration);

            Text.ColorTopLeft = colorTopLeft;
            Text.ColorTopRight = colorTopRight;
            Text.ColorBottomLeft = colorBottomLeft;
            Text.ColorBottomRight = colorBottomRight;

            yield return null;
        }

        colorTopRight.a = colorBottomLeft.a = colorBottomRight.a = colorTopLeft.a = 1;

        Text.ColorTopLeft = colorTopLeft;
        Text.ColorTopRight = colorTopRight;
        Text.ColorBottomLeft = colorBottomLeft;
        Text.ColorBottomRight = colorBottomRight;

        yield break;
    }
}
