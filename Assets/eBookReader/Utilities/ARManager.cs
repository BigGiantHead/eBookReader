using UnityEngine;
using System.Collections;
using System.Linq;

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

    private WebCamTexture webCam = null;

    private WebCamDevice webCamDevice;

    private RenderTexture targetTexture = null;

    public GameObject TestARObject = null;

    public GameObject SelfieObject = null;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    IEnumerator Start()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            targetTexture = new RenderTexture(Camera.main.pixelWidth, Camera.main.pixelHeight, 0);
        }

        yield break;
    }

    void Update()
    {
        if (webCam != null && webCam.isPlaying && webCam.didUpdateThisFrame)
        {
            Graphics.Blit(webCam, targetTexture);

            float aspect = (float)webCam.width / (float)webCam.height;
            float width = Screen.width;
            float height = width / aspect;

            MainCameraManager.Instance.CameraQuad.transform.localScale = new Vector3(webCamDevice.isFrontFacing ? -1 : 1, height / Screen.height, 1);
        }
    }

    public void SelectNextCamera()
    {
        StopAllCoroutines();
        StartCoroutine(DoSelectNextCamera());
    }

    public void CaptureScreen()
    {
        StopAllCoroutines();
        StartCoroutine(DoCaptureScreen());
    }

    public void TurnOffAR()
    {
        UI_CameraButtons.Instance.gameObject.SetActive(false);

        StopAllCoroutines();
        StartCoroutine(DoTurnOffAR());
    }

    public void StartSelfieAR()
    {
        if (webCam != null)
        {
            TurnOffAR();
        }
        else
        {
            UI_CameraButtons.Instance.gameObject.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(DoStartSelfieAR());
        }
    }

    public void StartAR(GameObject arPrefab)
    {
        if (webCam != null)
        {
            TurnOffAR();
        }
        else
        {
            UI_CameraButtons.Instance.gameObject.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(DoStartAR(arPrefab));
        }
    }

    private IEnumerator DoSelectNextCamera()
    {
        int camIndex = System.Array.FindIndex<WebCamDevice>(WebCamTexture.devices, d => d.name == webCamDevice.name) + 1;
        if (camIndex >= WebCamTexture.devices.Length)
        {
            camIndex = 0;
        }

        if (webCam != null)
        {
            webCam.Stop();
            webCam = null;
        }

        webCamDevice = WebCamTexture.devices[camIndex];
        webCam = new WebCamTexture(webCamDevice.name);
        webCam.wrapMode = TextureWrapMode.Repeat;

        if (webCamDevice.isFrontFacing)
        {
            MainCameraManager.Instance.CameraQuad.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            MainCameraManager.Instance.CameraQuad.transform.localScale = new Vector3(1, 1, 1);
        }

        webCam.Play();

        yield break;
    }

    private IEnumerator DoCaptureScreen()
    {
        UI.Instance.gameObject.SetActive(false);

        yield return null;

        string path = System.Guid.NewGuid() + ".jpg";

#if !UNITY_ANDROID || UNITY_EDITOR
        Application.CaptureScreenshot(path);
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidCamera.Instance.SaveScreenshotToGallery(path);
#endif

        yield return null;

        UI.Instance.gameObject.SetActive(true);

    }

    private IEnumerator DoTurnOffAR()
    {
        if (webCam != null)
        {
            webCam.Stop();
            webCam = null;
        }

        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        MainCameraManager.Instance.CameraQuad.gameObject.SetActive(false);

        MainCameraManager.Instance.MoveCameraToInitialPostion();

        yield return new WaitForSeconds(1f);

        eBookReader_SceneManager.Instance.LevelBackground.SetActive(true);
        eBookReader_SceneManager.Instance.ARShadowProjector.SetActive(true);

        SelfieObject.SetActive(false);
        TestARObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        if (!string.IsNullOrEmpty(BookGenerator.Instance.CurrentBook))
        {
            BookGenerator.Instance.Book.transform.parent.gameObject.SetActive(true);
        }

        BookGenerator.Instance.PageObjectsRoot.gameObject.SetActive(true);

        yield break;
    }

    private IEnumerator DoStartSelfieAR()
    {
        if (WebCamTexture.devices.Length <= 0)
        {
            yield break;
        }

        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;


        WebCamDevice device = WebCamTexture.devices[0];

        if (!device.isFrontFacing)
        {
            for (int i = 1; i < WebCamTexture.devices.Length; ++i)
            {
                if (WebCamTexture.devices[i].isFrontFacing)
                {
                    device = WebCamTexture.devices[i];
                }
            }
        }

        webCam = new WebCamTexture(device.name);
        webCam.wrapMode = TextureWrapMode.Repeat;
        webCamDevice = device;

        MainCameraManager.Instance.MoveCameraToSelfiePosition();

        yield return new WaitForSeconds(1f);

        eBookReader_SceneManager.Instance.LevelBackground.SetActive(false);
        eBookReader_SceneManager.Instance.ARShadowProjector.SetActive(false);

        MainCameraManager.Instance.CameraQuad.gameObject.SetActive(true);
        MainCameraManager.Instance.CameraQuad.texture = targetTexture;

        if (webCamDevice.isFrontFacing)
        {
            MainCameraManager.Instance.CameraQuad.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            MainCameraManager.Instance.CameraQuad.transform.localScale = new Vector3(1, 1, 1);
        }

        webCam.Play();

        SelfieObject.SetActive(true);

        LeanTween.scale(TestARObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeInBack);

        yield return new WaitForSeconds(0.5f);

        BookGenerator.Instance.Book.transform.parent.gameObject.SetActive(false);
        BookGenerator.Instance.PageObjectsRoot.gameObject.SetActive(false);

        yield break;
    }

    private IEnumerator DoStartAR(GameObject arPrefab)
    {
        if (WebCamTexture.devices.Length <= 0)
        {
            yield break;
        }

        WebCamDevice device = WebCamTexture.devices[0];

        webCam = new WebCamTexture(device.name);
        webCam.wrapMode = TextureWrapMode.Repeat;
        webCamDevice = device;

        MainCameraManager.Instance.MoveCameraToInitialARPostion();

        yield return new WaitForSeconds(1f);

        eBookReader_SceneManager.Instance.LevelBackground.SetActive(false);
        eBookReader_SceneManager.Instance.ARShadowProjector.SetActive(true);

        MainCameraManager.Instance.CameraQuad.gameObject.SetActive(true);
        MainCameraManager.Instance.CameraQuad.texture = targetTexture;

        if (webCamDevice.isFrontFacing)
        {
            MainCameraManager.Instance.CameraQuad.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            MainCameraManager.Instance.CameraQuad.transform.localScale = new Vector3(1, 1, 1);
        }

        webCam.Play();

        TestARObject.SetActive(true);

        LeanTween.scale(TestARObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeInBack);

        yield return new WaitForSeconds(0.5f);

        BookGenerator.Instance.Book.transform.parent.gameObject.SetActive(false);
        BookGenerator.Instance.PageObjectsRoot.gameObject.SetActive(false);

        yield break;
    }
}
