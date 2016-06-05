using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class TouchControls : MonoBehaviour
{
    private static TouchControls instance = null;

    public static TouchControls Instance
    {
        get
        {
            return instance;
        }
    }

    private float percentTillPan = 0;

    private float panx = 0;

    private float panz = 0;

    private float zoom = 0;

    public enum TouchState
    {
        None,

        PanCamera,

        NextPage,

        PrevPage,

        ZoomCamera,

        WaitingNextPage,

        WaitingPrevPage
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

    public float PanSpeed = 0.25f;

    public float MaxZoomOffset = 1.5f;

    public float ZoomSpeed = 1;

    public float DelayToPan = 0.25f;

    public float PercentTillPan
    {
        get
        {
            return percentTillPan;
        }
    }

    public TouchState MyTouchState
    {
        get
        {
            return myTouchState;
        }
    }

    public int FirstTouchId
    {
        get
        {
            return firstTouchId;
        }
    }

    public bool AcceptInput = true;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        originalCameraPosition = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!AcceptInput)
            return;

        if (EventSystem.current.IsPointerOverGameObject(firstTouchId))
        {
            myTouchState = TouchState.None;
        }
        else
        {
            UpdateTouchState();

            switch (myTouchState)
            {
                case TouchState.PanCamera:
                    DoPanCamera();
                    break;
                case TouchState.NextPage:
                case TouchState.WaitingNextPage:
                    DoNextPage();
                    break;
                case TouchState.PrevPage:
                case TouchState.WaitingPrevPage:
                    DoPrevPage();
                    break;
                case TouchState.ZoomCamera:
                    DoZoomCamera();
                    break;
            }
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
            zoom = Mathf.Clamp(zoom - ZoomSpeed * Time.deltaTime, 0, MaxZoomOffset);
        }
        else if (originalDistance < newDistance)
        {
            zoom = Mathf.Clamp(zoom + ZoomSpeed * Time.deltaTime, 0, MaxZoomOffset);
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
            panx = Mathf.Clamp(panx - touch.deltaPosition.x * Time.deltaTime * PanSpeed, -MaxPanOffsetX * zoompercent, MaxPanOffsetX * zoompercent);
            panz = Mathf.Clamp(panz - touch.deltaPosition.y * Time.deltaTime * PanSpeed, -MaxPanOffsetZ * zoompercent, MaxPanOffsetZ * zoompercent);
        }
        catch (ArgumentException)
        {
            Debug.Log("Touch does not exist " + firstTouchId);
        }
    }

    [ContextMenu("Next Page")]
    private void DoNextPageForce()
    {
        Book.NextPage();
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

    [ContextMenu("Previous Page")]
    private void DoPrevPageForce()
    {
        Book.PrevPage();
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

                StopCoroutine("WaitForPan");
                percentTillPan = 0;
            }
            else
            {
                secondTouchId = -1;

                Touch firstTouch = Input.touches[0];
                firstTouchId = firstTouch.fingerId;

                if (firstTouch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(firstTouch.position);
                    RaycastHit hit;

                    if (NextPage.Raycast(ray, out hit, 10000))
                    {
                        myTouchState = TouchState.WaitingNextPage;
                        currentPage = Mathf.Round(Book.page);
                        targetPage = Mathf.Clamp(currentPage + 1, -1, Book.NumPages + 1);
                    }
                    else if (PreviousPage.Raycast(ray, out hit, 10000))
                    {
                        myTouchState = TouchState.WaitingPrevPage;
                        currentPage = Mathf.Round(Book.page);
                        targetPage = Mathf.Clamp(currentPage - 1, -1, Book.NumPages + 1);
                    }

                    StartCoroutine("WaitForPan");
                }
                else if (firstTouch.phase == TouchPhase.Moved && myTouchState != TouchState.PanCamera)
                {
                    if (myTouchState == TouchState.WaitingNextPage)
                    {
                        myTouchState = TouchState.NextPage;
                    }
                    else if (myTouchState == TouchState.WaitingPrevPage)
                    {
                        myTouchState = TouchState.PrevPage;
                    }

                    StopCoroutine("WaitForPan");
                    percentTillPan = 0;
                }
                else if (myTouchState != TouchState.NextPage && myTouchState != TouchState.WaitingNextPage && myTouchState != TouchState.PrevPage && myTouchState != TouchState.WaitingPrevPage)
                {
                    myTouchState = TouchState.PanCamera;

                    StopCoroutine("WaitForPan");
                    percentTillPan = 0;
                }
            }
        }
        else
        {
            myTouchState = TouchState.None;
            firstTouchId = -1;
            secondTouchId = -1;

            StopCoroutine("WaitForPan");
            percentTillPan = 0;
        }
    }

    private IEnumerator WaitForPan()
    {
        float time = Time.timeSinceLevelLoad;

        while (Time.timeSinceLevelLoad - time < DelayToPan)
        {
            percentTillPan = (Time.timeSinceLevelLoad - time) / DelayToPan;

            yield return null;
        }

        myTouchState = TouchState.PanCamera;
        percentTillPan = 0;

        yield break;
    }
}
