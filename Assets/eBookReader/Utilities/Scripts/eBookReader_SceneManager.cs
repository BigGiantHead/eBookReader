using UnityEngine;
using UnityEngine.UI;

public class eBookReader_SceneManager : MonoBehaviour
{
    private static eBookReader_SceneManager instance = null;

    public static eBookReader_SceneManager Instance
    {
        get
        {
            return instance;
        }
    }

    public GameObject LevelBackground = null;

    public GameObject ARShadowProjector = null;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
    }
}
