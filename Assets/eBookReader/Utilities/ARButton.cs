using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ARButton : MonoBehaviour
{
    public RectTransform CanvasRect = null;

    public GameObject ARPrefab = null;

    // Use this for initialization
    void Start()
    {
    }

    public void OnClick()
    {
        StartCoroutine(DoOnClick());
    }

    private IEnumerator DoOnClick()
    {
        ARManager.Instance.StartAR(ARPrefab);

        yield break;
    }
}
