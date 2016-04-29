using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, 10 * Time.deltaTime);
    }
}
