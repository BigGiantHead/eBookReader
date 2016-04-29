using UnityEngine;
using System.Collections;

public class Doll_MouthLogic : MonoBehaviour
{
    public MeshRenderer MouthRenderer = null;

    public Animator MouthAnimator = null;

    public Texture2D[] Mouths = null;

    // Use this for initialization
    void Start()
    {
    }

    public void ChangeMouth(int index)
    {
        MouthAnimator.SetTrigger("bounce");

        if (Mouths.Length > index)
        {
            MouthRenderer.material.mainTexture = Mouths[index];
        }
    }
}
