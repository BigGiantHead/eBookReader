using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainCameraManager : MonoBehaviour
{
    private static MainCameraManager instance = null;

    public static MainCameraManager Instance
    {
        get
        {
            return instance;
        }
    }

    public Transform InitialCameraPosition = null;

    public Transform InitialARCameraPosition = null;

    public Transform SelfiePosition = null;

    public RawImage CameraQuad = null;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        transform.position = InitialCameraPosition.position;
        transform.rotation = InitialCameraPosition.rotation;
    }

    public void MoveCameraToInitialPostion()
    {
        TouchControls.Instance.AcceptInput = true;

        StopAllCoroutines();
        StartCoroutine(DoMoveCamera(InitialCameraPosition.position, InitialCameraPosition.rotation));
    }

    public void MoveCameraToInitialARPostion()
    {
        TouchControls.Instance.AcceptInput = false;

        StopAllCoroutines();
        StartCoroutine(DoMoveCamera(InitialARCameraPosition.position, InitialARCameraPosition.rotation));
    }

    public void MoveCameraToSelfiePosition()
    {
        TouchControls.Instance.AcceptInput = false;

        StopAllCoroutines();
        StartCoroutine(DoMoveCamera(SelfiePosition.position, SelfiePosition.rotation));
    }

    private IEnumerator DoMoveCamera(Vector3 targetPosition, Quaternion targetRotation)
    {
        float percent = 0;
        float duration = 0.5f;

        Vector3 currentPos = transform.position;
        Quaternion currentRotation = transform.rotation;

        do
        {
            transform.position = Vector3.Lerp(currentPos, targetPosition, percent);
            transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, percent);

            percent += (Time.deltaTime / duration);

            yield return null;
        }
        while (percent < 1);

        transform.position = targetPosition;
        transform.rotation = targetRotation;

        yield break;
    }
}
