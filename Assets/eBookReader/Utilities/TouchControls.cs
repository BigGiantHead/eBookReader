using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class TouchControls : MonoBehaviour
{
    private float firstTouchBeganTime = 0;

    private float panx = 0;

    private float panz = 0;

    private float zoom = 0;

    private enum TouchState
    {
        None,

        PanCamera,

        NextPage,

        PrevPage,

        ZoomCamera
    }

    private float currentPage = -1;

    private float targetPage = -1;

    private TouchState myTouchState = TouchState.None;

    private int firstTouchId = -1;

    private int secondTouchId = -1;

    private Vector3 originalCameraPosition = Vector3.zero;

    private Vector2 firstOriginalPosition = Vector2.zero;

    private Vector2 secondOriginalPosition = Vector2.zero;

    public Collider NextPage = null;

    public Collider PreviousPage = null;

    public MegaBookBuilder Book = null;

    public float MaxPanOffsetX = 0.5f;

    public float MaxPanOffsetZ = 0.5f;

    public float MaxZoomOffset = 1.5f;

    // Use this for initialization
    void Start()
    {
        originalCameraPosition = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTouchState();

        switch (myTouchState)
        {
            case TouchState.PanCamera:
                DoPanCamera();
                break;
            case TouchState.NextPage:
                DoNextPage();
                break;
            case TouchState.PrevPage:
                DoPrevPage();
                break;
            case TouchState.ZoomCamera:
                DoZoomCamera();
                break;
        }

        if (myTouchState != TouchState.NextPage && myTouchState != TouchState.PrevPage)
        {
            Book.page = Mathf.Lerp(Book.page, Mathf.Round(Book.page), Time.deltaTime * 10);
        }

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, originalCameraPosition + Camera.main.transform.forward * zoom + new Vector3(panx, 0, panz), Time.deltaTime * 10);
    }

    private void DoZoomCamera()
    {
        Touch firstTouch = Input.GetTouch(firstTouchId);
        Touch secondTouch = Input.GetTouch(secondTouchId);

        float originalDistance = Vector3.Distance(firstOriginalPosition - firstTouch.deltaPosition, secondOriginalPosition - secondTouch.deltaPosition);
        float newDistance = Vector3.Distance(firstOriginalPosition, secondOriginalPosition);

        if (originalDistance > newDistance)
        {
            zoom = Mathf.Clamp(zoom - 2 * Time.deltaTime, 0, MaxZoomOffset);
        }
        else if (originalDistance < newDistance)
        {
            zoom = Mathf.Clamp(zoom + 2 * Time.deltaTime, 0, MaxZoomOffset);
        }

        float zoompercent = zoom / MaxZoomOffset;
        panx = Mathf.Clamp(panx, -MaxPanOffsetX * zoompercent, MaxPanOffsetX * zoompercent);
        panz = Mathf.Clamp(panz, -MaxPanOffsetZ * zoompercent, MaxPanOffsetZ * zoompercent);
    }

    private void DoPanCamera()
    {
        if (firstTouchId == -1)
        {
            myTouchState = TouchState.None;

            return;
        }

        try
        {
            Touch touch = Input.GetTouch(firstTouchId);

            float zoompercent = zoom / MaxZoomOffset;
            panx = Mathf.Clamp(panx - touch.deltaPosition.x * Time.deltaTime * 0.25f, -MaxPanOffsetX * zoompercent, MaxPanOffsetX * zoompercent);
            panz = Mathf.Clamp(panz - touch.deltaPosition.y * Time.deltaTime * 0.25f, -MaxPanOffsetZ * zoompercent, MaxPanOffsetZ * zoompercent);
        }
        catch (ArgumentException)
        {
            Debug.Log("Touch does not exist " + firstTouchId);
        }
    }

    private void DoNextPage()
    {
        if (firstTouchId == -1)
        {
            myTouchState = TouchState.None;

            return;
        }

        Touch touch = Input.GetTouch(firstTouchId);
        float delta = touch.deltaPosition.x;
        Book.page = Mathf.Clamp(Book.page - delta * Time.deltaTime * 0.5f, currentPage, targetPage);
    }

    private void DoPrevPage()
    {
        if (firstTouchId == -1)
        {
            myTouchState = TouchState.None;

            return;
        }

        Touch touch = Input.GetTouch(firstTouchId);
        float delta = touch.deltaPosition.x;
        Book.page = Mathf.Clamp(Book.page - delta * Time.deltaTime * 0.5f, targetPage, currentPage);
    }

    private void UpdateTouchState()
    {
        if (Input.touchCount > 0 && EventSystem.current.currentSelectedGameObject == null)
        {
            if (Input.touchCount == 2)
            {
                Touch firstTouch = Input.touches[0];
                firstTouchId = firstTouch.fingerId;

                Touch secondTouch = Input.touches[1];
                secondTouchId = secondTouch.fingerId;

                if (firstTouch.phase == TouchPhase.Began || secondTouch.phase == TouchPhase.Began)
                {
                    myTouchState = TouchState.ZoomCamera;

                    firstOriginalPosition = firstTouch.position;
                    secondOriginalPosition = secondTouch.position;
                }
            }
            else
            {
                secondTouchId = -1;

                Touch firstTouch = Input.touches[0];
                firstTouchId = firstTouch.fingerId;

                if (firstTouch.phase == TouchPhase.Began)
                {
                    firstTouchBeganTime = Time.timeSinceLevelLoad;

                    Ray ray = Camera.main.ScreenPointToRay(firstTouch.position);
                    RaycastHit hit;

                    if (NextPage.Raycast(ray, out hit, 10000))
                    {
                        myTouchState = TouchState.NextPage;
                        currentPage = Mathf.Round(Book.page);
                        targetPage = Mathf.Clamp(currentPage + 1, -1, Book.NumPages + 1);
                    }
                    else if (PreviousPage.Raycast(ray, out hit, 10000))
                    {
                        myTouchState = TouchState.PrevPage;
                        currentPage = Mathf.Round(Book.page);
                        targetPage = Mathf.Clamp(currentPage - 1, -1, Book.NumPages + 1);
                    }
                }
                else if (firstTouch.phase == TouchPhase.Moved && myTouchState != TouchState.PanCamera)
                {
                    if (Time.timeSinceLevelLoad - firstTouchBeganTime > 0.25f)
                    {
                        myTouchState = TouchState.PanCamera;
                    }
                    firstTouchBeganTime = Time.timeSinceLevelLoad;
                }
                else if (myTouchState != TouchState.NextPage && myTouchState != TouchState.PrevPage)
                {
                    myTouchState = TouchState.PanCamera;
                }
            }
        }
        else
        {
            myTouchState = TouchState.None;
            firstTouchId = -1;
            secondTouchId = -1;
        }
    }
}
