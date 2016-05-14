using UnityEngine;
using System.Collections;

public class ARManager : MonoBehaviour
{
    private static ARManager instance = null;

    public static ARManager Instance
    {
        get
        {
            return instance;
        }
    }

    private static WebCamTexture webCam = null;

    private RenderTexture targetTexture = null;

    public GameObject TestARObject = null;

    void Awake()
    {
        instance = this;
    }

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

    public void StartAR(GameObject arPrefab)
    {
        StartCoroutine(DoStartAR(arPrefab));
    }

    private IEnumerator DoStartAR(GameObject arPrefab)
    {
        MainCameraManager.Instance.MoveCameraToInitialARPostion();

        yield return new WaitForSeconds(1f);

        SceneManager.Instance.LevelBackground.SetActive(false);
        SceneManager.Instance.CameraQuad.gameObject.SetActive(true);
        SceneManager.Instance.CameraQuad.texture = targetTexture;

        webCam.Play();

        TestARObject.SetActive(true);

        LeanTween.scale(TestARObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeInBack);

        yield return new WaitForSeconds(0.5f);

        BookGenerator.Instance.Book.transform.parent.gameObject.SetActive(false);
        BookGenerator.Instance.PageObjectsRoot.gameObject.SetActive(false);

        yield break;
    }
}
