using UnityEngine;

public class TouchIndicator : MonoBehaviour
{
    private static TouchIndicator instance = null;

    public static TouchIndicator Instance
    {
        get
        {
            return instance;
        }
    }

    private float alpha = 0;

    public CanvasGroup CanvasGroup = null;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (TouchControls.Instance.FirstTouchId > -1 && Input.touchCount > 0)
        {
            try
            {
                Touch touch = Input.GetTouch(TouchControls.Instance.FirstTouchId);
                transform.position = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0.1f));
            }
            catch
            {
            }
        }

        CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, alpha, Time.deltaTime * 10);

        switch (TouchControls.Instance.MyTouchState)
        {
            case TouchControls.TouchState.PanCamera:
                alpha = 1f;
                break;

            case TouchControls.TouchState.WaitingNextPage:
            case TouchControls.TouchState.WaitingPrevPage:
                alpha = TouchControls.Instance.PercentTillPan;
                break;

            default:
                alpha = 0;
                break;
        }
    }
}
