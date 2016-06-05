using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class CheckButton : MonoBehaviour
{
    [HideInInspector]
    public int Index = -1;

    public int Value = -1;

    public Button MyButton = null;

	// Use this for initialization
	protected virtual void Start ()
    {
	}

    public abstract void Select();

    public abstract void Deselect();
}
