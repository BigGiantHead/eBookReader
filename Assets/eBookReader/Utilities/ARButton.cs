using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ARButton : MonoBehaviour
{
    private static WebCamTexture webCam = null;

    private RenderTexture targetTexture = null;

    public RectTransform CanvasRect = null;

    public GameObject ARPrefab = null;

    // Use this for initialization
    void Start()
    {
        if (webCam == null)
        {
            webCam = new WebCamTexture();
            targetTexture = new RenderTexture(Camera.main.pixelWidth, Camera.main.pixelHeight, 0);
        }
    }

    void Update()
    {
        if (webCam.isPlaying)
        {
            Graphics.Blit(webCam, targetTexture);
        }
    }

    public void OnClick()
    {
        StartCoroutine(DoOnClick());
    }

    private IEnumerator DoOnClick()
    {
        MainCameraManager.Instance.MoveCameraToInitialARPostion();

        yield return new WaitForSeconds(1f);

        SceneManager.Instance.LevelBackground.SetActive(false);
        SceneManager.Instance.CameraQuad.gameObject.SetActive(true);
        SceneManager.Instance.CameraQuad.texture = targetTexture;

        webCam.Play();

        yield break;
    }
}
