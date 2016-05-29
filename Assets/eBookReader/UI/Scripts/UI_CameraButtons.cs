using UnityEngine;
using System.Collections;

public class UI_CameraButtons : MonoBehaviour
{
    private static UI_CameraButtons instance = null;

    public static UI_CameraButtons Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {
    }

    void OnDestroy()
    {
        instance = null;
    }
}
