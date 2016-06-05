using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Linq;

public class SelfieTouchControls : MonoBehaviour
{
    private Vector3[] initialPositions = null;

    private float firstTouchStartTime = 0;

    private Vector2 firstTouchPreviousPos = Vector2.zero;

    private Vector2 secondTouchPreviousPos = Vector2.zero;

    private Vector3 localActiveCharacterTouchPoint = Vector3.zero;

    private TouchState myTouchState = TouchState.None;

    private int firstTouchId = -1;

    private int secondTouchId = -1;

    private Collider activeCharacter = null;

    public Collider[] CharacterColliders = null;

    public enum TouchState
    {
        None = 0,

        OneFinger = 1,

        TwoFingers = 2
    }

    void Awake()
    {
        initialPositions = new Vector3[CharacterColliders.Length];
        for (int i = 0; i < CharacterColliders.Length; ++i)
        {
            initialPositions[i] = CharacterColliders[i].transform.position;
        }

    }

    // Use this for initialization
    void Start()
    {
    }

    void OnEnable()
    {
        for (int i = 0; i < CharacterColliders.Length; ++i)
        {
            CharacterColliders[i].transform.localScale = Vector3.one;
            CharacterColliders[i].transform.position = initialPositions[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTouchState();

        if (activeCharacter != null)
        {
            if (secondTouchId > -1 && myTouchState == TouchState.TwoFingers)
            {
                Touch firstTouch = Input.touches.Where(t => t.fingerId == firstTouchId).FirstOrDefault();
                Touch secondTouch = Input.touches.Where(t => t.fingerId == secondTouchId).FirstOrDefault();

                Vector2 previousFirstTouch = firstTouch.position - firstTouch.deltaPosition;
                Vector2 previousSecondTouch = secondTouch.position - secondTouch.deltaPosition;

                Vector2 previousDir = (previousFirstTouch - previousSecondTouch).normalized;
                Vector2 dir = (firstTouch.position - secondTouch.position).normalized;

                float previousDistance = Vector2.Distance(previousFirstTouch, previousSecondTouch);
                float distance = Vector2.Distance(firstTouch.position, secondTouch.position);

                float delta = distance - previousDistance;
                float scale = Mathf.Clamp(activeCharacter.transform.localScale.x + delta * Time.deltaTime, 0.5f, 1.5f);

                activeCharacter.transform.localScale = Vector3.one * scale;

                float angle = Utils.AngleSigned(previousDir, dir, Vector3.forward);
                activeCharacter.transform.Rotate(activeCharacter.transform.forward * angle * 10);

            }
            else if (firstTouchId > -1 && myTouchState == TouchState.OneFinger)
            {
                Touch firstTouch = Input.touches.Where(t => t.fingerId == firstTouchId).FirstOrDefault();

                Vector3 firstTouchInViewport = Camera.main.ScreenToViewportPoint(firstTouch.position);
                Vector3 touchedPointInWorld = activeCharacter.transform.TransformPoint(localActiveCharacterTouchPoint);
                Vector3 touchedPointInViewport = Camera.main.WorldToViewportPoint(touchedPointInWorld);

                firstTouchInViewport.z = touchedPointInViewport.z;
                activeCharacter.transform.position = Camera.main.ViewportToWorldPoint(firstTouchInViewport) - (touchedPointInWorld - activeCharacter.transform.position);

                if (firstTouch.phase == TouchPhase.Ended && firstTouch.tapCount == 1 && (Time.timeSinceLevelLoad - firstTouchStartTime) < 0.25f)
                {
                    activeCharacter.BroadcastMessage("ChangeRandomFace");
                }
            }
        }
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

                if (firstTouch.phase == TouchPhase.Began || secondTouch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(firstTouch.position);
                    RaycastHit hit;

                    activeCharacter = TouchedCollider(ray, out hit);

                    if (activeCharacter != null)
                    {
                        localActiveCharacterTouchPoint = activeCharacter.transform.InverseTransformPoint(hit.point);
                        firstTouchPreviousPos = firstTouch.position;
                        secondTouchPreviousPos = secondTouch.position;

                        myTouchState = TouchState.TwoFingers;
                    }
                }
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

                    activeCharacter = TouchedCollider(ray, out hit);

                    if (activeCharacter != null && myTouchState != TouchState.TwoFingers)
                    {
                        localActiveCharacterTouchPoint = activeCharacter.transform.InverseTransformPoint(hit.point);
                        myTouchState = TouchState.OneFinger;
                        firstTouchPreviousPos = firstTouch.position;
                        firstTouchStartTime = Time.timeSinceLevelLoad;
                    }
                }
            }
        }
        else
        {
            myTouchState = TouchState.None;
            activeCharacter = null;
            firstTouchId = -1;
            secondTouchId = -1;
        }
    }

    private Collider TouchedCollider(Ray ray, out RaycastHit hit)
    {
        hit = new RaycastHit();

        for (int i = 0; i < CharacterColliders.Length; ++i)
        {
            if (CharacterColliders[i].Raycast(ray, out hit, float.MaxValue))
            {
                return CharacterColliders[i];
            }
        }

        return null;
    }
}
