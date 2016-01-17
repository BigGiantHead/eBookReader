using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class ProfileElement : MonoBehaviour
{
    [HideInInspector]
    public ProfileData MyData = null;

    public Image Avatar = null;

    public Text UserName = null;
}
