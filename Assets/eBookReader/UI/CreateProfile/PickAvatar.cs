using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PickAvatar : MonoBehaviour
{
    private Avatar currentAvatar = null;

    public enum Gender : int
    {
        None = 0,

        Male = 1,

        Female = 2
    }

    [Header("Customization")]
    public RectTransform Hair = null;

    public RectTransform TShirt = null;

    public RectTransform Pants = null;

    public RectTransform Shoes = null;

    public CheckButtonGroup GenderChecks = null;

    public AvatarRenderingRequester AvatarDisplay = null;

    public GameObject ItemPrefab = null;

    void Awake()
    {
        GenderChecks.OnSelectionChanged.AddListener((int value) => 
        {
            currentAvatar.Gender = (Gender)value;
            SetToCurrentGender();
        });
    }

    // Use this for initialization
    void Start()
    {
    }

    public void RebindWithAvatar(Avatar avatar)
    {
        currentAvatar = avatar;
        AvatarDisplay.StartAvatar(currentAvatar);
        GenderChecks.ChangeSelection((int)currentAvatar.Gender - 1);
    }

    public void ClearWindow()
    {
        AvatarDisplay.StopAvatar();
    }

    private void PopulateKid()
    {
        Kid_Customizer currentCustomizer = AvatarRenderer.Instance.GetCustomizerAtIndex((int)currentAvatar.Gender - 1);

        //populate hair
        Hair.ClearChildren();
        for (int i = 0; i < currentCustomizer.HairSprites.Length; ++i)
        {
            GameObject item = Instantiate(ItemPrefab);
            item.transform.SetParent(Hair);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
            item.transform.localScale = Vector3.one;
            item.SetActive(true);

            int index = i;
            AvatarItem avatarItem = item.GetComponent<AvatarItem>();
            avatarItem.Index = index;

            avatarItem.MyButton.onClick.AddListener(() =>
            {
                currentAvatar.HairIndex = index;
                Hair.BroadcastMessage("Select", index, SendMessageOptions.DontRequireReceiver);
            });

            avatarItem.ElementImage.sprite = currentCustomizer.HairSprites[i];
        }

        //populate t-shirt
        TShirt.ClearChildren();
        for (int i = 0; i < currentCustomizer.BlouseSprites.Length; ++i)
        {
            GameObject item = Instantiate(ItemPrefab);
            item.transform.SetParent(TShirt);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
            item.transform.localScale = Vector3.one;
            item.SetActive(true);

            int index = i;
            AvatarItem avatarItem = item.GetComponent<AvatarItem>();
            avatarItem.Index = index;

            avatarItem.MyButton.onClick.AddListener(() =>
            {
                currentAvatar.BlouseIndex = index;
                TShirt.BroadcastMessage("Select", index, SendMessageOptions.DontRequireReceiver);
            });
            avatarItem.ElementImage.sprite = currentCustomizer.BlouseSprites[i];
        }

        //populate pants
        Pants.ClearChildren();
        for (int i = 0; i < currentCustomizer.PantsSprites.Length; ++i)
        {
            GameObject item = Instantiate(ItemPrefab);
            item.transform.SetParent(Pants);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
            item.transform.localScale = Vector3.one;
            item.SetActive(true);

            int index = i;
            AvatarItem avatarItem = item.GetComponent<AvatarItem>();
            avatarItem.Index = index;

            avatarItem.MyButton.onClick.AddListener(() =>
            {
                currentAvatar.PantsIndex = index;
                Pants.BroadcastMessage("Select", index, SendMessageOptions.DontRequireReceiver);
            });

            avatarItem.ElementImage.sprite = currentCustomizer.PantsSprites[i];
        }

        //populate shoes
        Shoes.ClearChildren();
        for (int i = 0; i < currentCustomizer.ShoeSprites.Length; ++i)
        {
            GameObject item = Instantiate(ItemPrefab);
            item.transform.SetParent(Shoes);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
            item.transform.localScale = Vector3.one;
            item.SetActive(true);

            int index = i;
            AvatarItem avatarItem = item.GetComponent<AvatarItem>();
            avatarItem.Index = index;

            avatarItem.MyButton.onClick.AddListener(() =>
            {
                currentAvatar.ShoesIndex = index;
                Shoes.BroadcastMessage("Select", index, SendMessageOptions.DontRequireReceiver);
            });

            avatarItem.ElementImage.sprite = currentCustomizer.ShoeSprites[i];
        }

        //TODO: implement logo change

        Hair.BroadcastMessage("Select", currentAvatar.HairIndex, SendMessageOptions.DontRequireReceiver);
        TShirt.BroadcastMessage("Select", currentAvatar.BlouseIndex, SendMessageOptions.DontRequireReceiver);
        Pants.BroadcastMessage("Select", currentAvatar.PantsIndex, SendMessageOptions.DontRequireReceiver);
        Shoes.BroadcastMessage("Select", currentAvatar.ShoesIndex, SendMessageOptions.DontRequireReceiver);
    }

    private void SetToCurrentGender()
    {
        if (currentAvatar == null)
            return;

        PopulateKid();
    }
}
