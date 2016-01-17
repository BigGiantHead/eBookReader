using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuBook : MonoBehaviour
{
    private Quaternion startRot;

    private Quaternion targetRot;

    private Vector3 targetScale = Vector3.one;

    public Texture2D FrontCover = null;

    public Color BookColor = Color.white;

    public UIMesh Cover = null;

    public UIMesh Side = null;

    public UIMesh Back = null;

    public Transform BookModel = null;

    public Text Title = null;

    public Text Description = null;

    void Awake()
    {
        Cover.Texture = FrontCover;
        Side.color = BookColor;
        Back.color = BookColor;
    }

    // Use this for initialization
    void Start()
    {
        startRot = targetRot = Quaternion.Euler(Random.Range(-20f, 20f), Random.Range(-20f, 20f), 0);
    }

    void Update()
    {
        BookModel.localRotation = Quaternion.Lerp(BookModel.localRotation, targetRot, Time.deltaTime * 20);
    }
}
