using System;
using UnityEngine;

public class DemoGlobalValues : MonoBehaviour
{
    public string TimeString
    {
        get { return DateTime.Now.Millisecond.ToString("000"); }
    }
}
