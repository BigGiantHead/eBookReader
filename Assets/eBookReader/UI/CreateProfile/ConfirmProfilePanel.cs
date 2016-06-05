using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConfirmProfilePanel : MonoBehaviour
{
    public Image[] FuritwordImages = null;

    public Text[] Letters = null;

    public AvatarRenderingRequester Avatar = null;

    public void Bind()
    {
        if (CreateProfilePanel.Instance != null && CreateProfilePanel.Instance.Profile != null)
        {
            //avatar
            Avatar.StartAvatar(CreateProfilePanel.Instance.Profile.Avatar);

            //bind password
            Fruitword password = CreateProfilePanel.Instance.PasswordPanel.Password;

            string fruit = "Fruits/" + password.FirstFruit.ToString();
            FuritwordImages[0].overrideSprite = Resources.Load<Sprite>(fruit);

            fruit = "Fruits/" + password.SecondFruit.ToString();
            FuritwordImages[1].overrideSprite = Resources.Load<Sprite>(fruit);

            fruit = "Fruits/" + password.ThirdFruit.ToString();
            FuritwordImages[2].overrideSprite = Resources.Load<Sprite>(fruit);

            fruit = "Fruits/" + password.FourthFruit.ToString();
            FuritwordImages[3].overrideSprite = Resources.Load<Sprite>(fruit);

            //bind name
            string name = CreateProfilePanel.Instance.NamePanel.UserName;

            if (string.IsNullOrEmpty(name))
            {
                for (int i = 0; i < Letters.Length; ++i)
                {
                    Letters[i].text = "";
                }
            }
            else
            {
                for (int i = 0; i < Letters.Length; ++i)
                {
                    if (name.Length > i)
                    {
                        Letters[i].text = name[i].ToString().ToUpperInvariant();
                    }
                    else
                    {
                        Letters[i].text = "";
                    }
                }
            }
        }
              
    }

    public void ClearWindow()
    {
        Avatar.StopAvatar();
    }
}
