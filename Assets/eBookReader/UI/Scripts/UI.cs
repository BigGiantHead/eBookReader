using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour
{
    private static UI instance = null;

    public static UI Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start ()
    {	
	}

    void OnDestroy()
    {
        instance = null;
    }
}
