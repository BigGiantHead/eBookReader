using UnityEngine;
using System.Collections;

public class CheckButtonGroup : MonoBehaviour
{
    public CheckButton[] Checks = null;

    public int InitialySelected = -1;

    public OnSelectedIndexChanged OnSelectionChanged = new OnSelectedIndexChanged();

    // Use this for initialization
    void Start()
    {
        if (Checks != null)
        {
            for (int i = 0; i < Checks.Length; ++i)
            {
                int index = i;
                Checks[i].MyButton.onClick.AddListener(() => 
                {
                    ChangeSelection(index);
                });
            }
        }
    }

    //void OnEnable()
    //{
    //    if (InitialySelected >= 0 && Checks != null)
    //    {
    //        ChangeSelection(InitialySelected);
    //    }
    //}

    public void ChangeSelection(int index)
    {
        int value = -1;
        for (int i = 0; i < Checks.Length; ++i)
        {
            if (i == index)
            {
                Checks[i].Select();
                value = Checks[i].Value;
            }
            else
            {
                Checks[i].Deselect();
            }
        }

        OnSelectionChanged.Invoke(value);
    }
}

public class OnSelectedIndexChanged : UnityEngine.Events.UnityEvent<int>
{

}
