using UnityEngine;
using System.Collections;
using System;

public class GenderCheck : CheckButton
{
    public GameObject TickImage = null;

    public override void Deselect()
    {
        TickImage.SetActive(false);
    }

    public override void Select()
    {
        TickImage.SetActive(true);
    }
}
