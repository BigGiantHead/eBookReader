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

    public Texture2D BackCover = null;

    public Color BookColor = Color.white;

    public UIMesh Cover = null;

    public UIMesh Side = null;

    public UIMesh Back = null;

    public Transform BookModel = null;

    public Text Title = null;

    public Text Description = null;

    public string BookBundle = null;

    // Use this for initialization
    void Start()
    {
        Cover.Texture = FrontCover;
        Cover.color = Color.white;

        Back.Texture = BackCover;
        Back.color = Color.white;

        Side.color = BookColor;

        startRot = targetRot = Quaternion.Euler(Random.Range(-20f, 20f), Random.Range(-20f, 20f), 0);
    }

    void Update()
    {
        //BookModel.localRotation = Quaternion.Lerp(BookModel.localRotation, targetRot, Time.deltaTime * 20);
        BookModel.Rotate(Vector3.up, Time.deltaTime * 20f);
    }

    public void ReadBook()
    {
        BookGenerator.Instance.LoadBookFromBundle(BookBundle);

        PickBook.Instance.MyPanel.Hide();
        CurrentProfileElement.Instance.MyPanel.Show();
    }
}
