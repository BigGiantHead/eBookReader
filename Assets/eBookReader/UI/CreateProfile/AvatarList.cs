using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AvatarList : MonoBehaviour
{
    private Sprite[] avatars = null;

    public GameObject AvatarSample = null;

    public RectTransform MyList = null;

    // Use this for initialization
    void Start()
    {
        avatars = Resources.LoadAll<Sprite>("Avatars");
        foreach (Sprite avatar in avatars)
        {
            GameObject avatarElement = Instantiate(AvatarSample);
            avatarElement.transform.SetParent(MyList);

            Image avatarImage = avatarElement.GetComponent<Image>();
            avatarImage.sprite = avatar;

            avatarElement.SetActive(true);
        }

        Vector2 size = MyList.sizeDelta;
        size.y = Mathf.RoundToInt(avatars.Length / 4f) * 100 + (Mathf.RoundToInt(avatars.Length / 4f) - 1) * 10;
        MyList.sizeDelta = size;

        Vector3 position = MyList.localPosition;
        position.y = size.y / 2f;
        MyList.localPosition = position;
    }
}
