using UnityEngine;
using System.Collections.Generic;

public class Kid_Customizer : MonoBehaviour
{
#if UNITY_EDITOR
    private int index = 0;
#endif

    [Header("Hair Elements")]
    public List<CustomizerElement> HairModels = null;

    public Sprite[] HairSprites = null;

    [Header("Blouse Elements")]
    public List<CustomizerElement> BlouseModels = null;

    public Sprite[] BlouseSprites = null;

    [Header("Pants Elements")]
    public List<CustomizerElement> PantsModels = null;

    public Sprite[] PantsSprites = null;

    [Header("Shoes Elements")]
    public List<CustomizerElement> ShoeModels = null;

    public Sprite[] ShoeSprites = null;

    [Header("Logo Elements")]
    public List<CustomizerElement> LogoModels = null;

    public Sprite[] LogoSprites = null;

    // Use this for initialization
    void Start()
    {
    }


#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            index++;

            ChangeHairModel(index % HairModels.Count);
            ChangeBlouseModel(index % BlouseModels.Count);
            ChangePantsModel(index % PantsModels.Count);
            ChangeShoesModel(index % ShoeModels.Count);
            //ChangeLogoModel(index % LogoModels.Length);
        }
    }
#endif

    public void ChangeHairModel(int index)
    {
        ActivateModel(index, HairModels);
    }

    public void ChangeBlouseModel(int index)
    {
        ActivateModel(index, BlouseModels);
    }

    public void ChangePantsModel(int index)
    {
        ActivateModel(index, PantsModels);
    }

    public void ChangeShoesModel(int index)
    {
        ActivateModel(index, ShoeModels);
    }

    public void ChangeLogoModel(int index)
    {
        ActivateModel(index, LogoModels);
    }

    private void ActivateModel(int index, List<CustomizerElement> elements)
    {
        List<int> activatedElements = new List<int>(0);

        for (int i = 0; i < elements.Count; ++i)
        {
            for (int j = 0; j < elements[i].Elements.Count; ++j)
            {
                if (i == index)
                {
                    elements[i].Elements[j].SetActive(true);
                    activatedElements.Add(elements[i].Elements[j].GetInstanceID());
                }
                else
                {
                    if (!activatedElements.Contains(elements[i].Elements[j].GetInstanceID()))
                    {
                        elements[i].Elements[j].SetActive(false);
                    }
                }
            }
        }
    }
}

[System.Serializable]
public class CustomizerElement
{
    public List<GameObject> Elements = null;
}
