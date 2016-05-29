using UnityEngine;
using System.Collections;

public class Doll_FaceController : MonoBehaviour
{
    public Doll_MouthLogic Mouth = null;

    public Animator LeftEye = null;

    public Animator RightEye = null;

    // Use this for initialization
    void Start()
    {
        //StartCoroutine(DoChangeLookDirection());
        //StartCoroutine(DoChangeFace());
    }

    public void ChangeFace(int index)
    {
        switch(index)
        {
            //unsatisfied
            case 0:
                LeftEye.SetTrigger("closeUpperLid");
                RightEye.SetTrigger("closeUpperLid");
                LeftEye.SetTrigger("browAngry");
                RightEye.SetTrigger("browAngry");
                Mouth.ChangeMouth(7);
                break;
            //happy
            case 1:
                LeftEye.SetTrigger("returnToNormal");
                RightEye.SetTrigger("returnToNormal");
                LeftEye.SetTrigger("browExcited");
                RightEye.SetTrigger("browExcited");
                Mouth.ChangeMouth(19);
                break;
            //silly
            case 2:
                LeftEye.SetTrigger("returnToNormal");
                RightEye.SetTrigger("returnToNormal");
                LeftEye.SetTrigger("browExcited");
                RightEye.SetTrigger("browExcited");
                Mouth.ChangeMouth(4);
                break;

            default:
            //crying
            case 3:
                LeftEye.SetTrigger("lowerLidShake");
                RightEye.SetTrigger("lowerLidShake");
                LeftEye.SetTrigger("browSad");
                RightEye.SetTrigger("browSad");
                Mouth.ChangeMouth(5);
                break;
        }

        CancelInvoke("ChangeToNormal");
        Invoke("ChangeToNormal", 30);
    }

    public void ChangeRandomFace()
    {
        ChangeFace(Random.Range(0, 4));
    }

    private void ChangeToNormal()
    {
        LeftEye.SetTrigger("returnToNormal");
        RightEye.SetTrigger("returnToNormal");
        Mouth.ChangeMouth(16);
    }
}
