using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    private static SceneManager instance = null;

    public static SceneManager Instance
    {
        get
        {
            return instance;
        }
    }

    public RawImage CameraQuad = null;

    public GameObject LevelBackground = null;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
    }
}
