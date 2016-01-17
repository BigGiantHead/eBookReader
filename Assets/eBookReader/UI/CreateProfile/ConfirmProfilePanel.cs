using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConfirmProfilePanel : MonoBehaviour
{
    public Image[] FuritwordImages = null;

    public Image Avatar = null;

    public void Bind()
    {
       if (CreateProfilePanel.Instance != null && CreateProfilePanel.Instance.NewProfile != null)
        {
            Avatar.overrideSprite = Resources.Load<Sprite>("Avatars/" + CreateProfilePanel.Instance.NewProfile.Avatar);

            string fruit = "Fruits/" + CreateProfilePanel.Instance.NewProfile.Password.FirstFruit.ToString();
            FuritwordImages[0].overrideSprite = Resources.Load<Sprite>(fruit);

            fruit = "Fruits/" + CreateProfilePanel.Instance.NewProfile.Password.SecondFruit.ToString();
            FuritwordImages[1].overrideSprite = Resources.Load<Sprite>(fruit);

            fruit = "Fruits/" + CreateProfilePanel.Instance.NewProfile.Password.ThirdFruit.ToString();
            FuritwordImages[2].overrideSprite = Resources.Load<Sprite>(fruit);

            fruit = "Fruits/" + CreateProfilePanel.Instance.NewProfile.Password.FourthFruit.ToString();
            FuritwordImages[3].overrideSprite = Resources.Load<Sprite>(fruit);
        }
    }
}
