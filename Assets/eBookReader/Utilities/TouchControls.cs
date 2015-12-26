using UnityEngine;
using System.Collections;

public class TouchControls : MonoBehaviour
{
    private float panx = 0;

    private float panz = 0;

    private float maxzoom = 1.5f;

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

    private Vector3 firstOriginalPosition = Vector3.zero;

    private Vector3 secondOriginalPosition = Vector3.zero;

    public Collider NextPage = null;

    public Collider PreviousPage = null;

    public MegaBookBuilder Book = null;

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

        float originalDistance = Vector3.Distance(firstOriginalPosition, secondOriginalPosition);
        float newDistance = Vector3.Distance(firstTouch.position, secondTouch.position);

        if (originalDistance > newDistance)
        {
            zoom = Mathf.Clamp(zoom - 2 * Time.deltaTime, 0, maxzoom);
        }
        else if (originalDistance < newDistance)
        {
            zoom = Mathf.Clamp(zoom + 2 * Time.deltaTime, 0, maxzoom);
        }

        float zoompercent = zoom / maxzoom;
        panx = Mathf.Clamp(panx, -0.5f * zoompercent, 0.5f * zoompercent);
        panz = Mathf.Clamp(panz, -0.5f * zoompercent, 0.5f * zoompercent);

        firstOriginalPosition = firstTouch.position;
        secondOriginalPosition = secondTouch.position;
    }

    private void DoPanCamera()
    {
        if (firstTouchId == -1)
        {
            myTouchState = TouchState.None;

            return;
        }

        Touch touch = Input.GetTouch(firstTouchId);

        float zoompercent = zoom / maxzoom;
        panx = Mathf.Clamp(panx - touch.deltaPosition.x * Time.deltaTime * 0.25f, -0.5f * zoompercent, 0.5f * zoompercent);
        panz = Mathf.Clamp(panz - touch.deltaPosition.y * Time.deltaTime * 0.25f, -0.5f * zoompercent, 0.5f * zoompercent);
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
        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 2)
            {
                Touch firstTouch = Input.touches[0];
                firstTouchId = firstTouch.fingerId;

                Touch secondTouch = Input.touches[1];
                secondTouchId = secondTouch.fingerId;

                if (myTouchState != TouchState.ZoomCamera)
                {
                    firstOriginalPosition = firstTouch.position;
                    secondOriginalPosition = secondTouch.position;
                }

                myTouchState = TouchState.ZoomCamera;
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
                        myTouchState = TouchState.NextPage;
                        currentPage = Mathf.Round(Book.page);
                        targetPage = Mathf.Clamp(currentPage + 1, -1, Book.NumPages);
                    }
                    else if (PreviousPage.Raycast(ray, out hit, 10000))
                    {
                        myTouchState = TouchState.PrevPage;
                        currentPage = Mathf.Round(Book.page);
                        targetPage = Mathf.Clamp(currentPage - 1, -1, Book.NumPages);
                    }
                    else
                    {
                        myTouchState = TouchState.PanCamera;
                    }
                }
                else if (myTouchState == TouchState.ZoomCamera)
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
