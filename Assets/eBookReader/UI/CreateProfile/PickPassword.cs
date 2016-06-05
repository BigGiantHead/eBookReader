using UnityEngine;
using System.Collections;
using System.Linq;

public class PickPassword : SingletonBehaviour<PickPassword>
{
    public PasswordFruitPosition[] PasswordPositions = null;

    public PasswordFruit[] PasswordFruits = null;

    public Fruitword Password
    {
        set
        {
            Fruitword fw = value;

            PasswordFruit firstFruit = PasswordFruits.Where(pf => pf.Type == fw.FirstFruit).FirstOrDefault();
            PasswordFruit secondFruit = PasswordFruits.Where(pf => pf.Type == fw.SecondFruit).FirstOrDefault();
            PasswordFruit thirdFruit = PasswordFruits.Where(pf => pf.Type == fw.ThirdFruit).FirstOrDefault();
            PasswordFruit fourthFruit = PasswordFruits.Where(pf => pf.Type == fw.FourthFruit).FirstOrDefault();

            SetFruitAtPosition(firstFruit, PasswordPositions[0]);
            SetFruitAtPosition(secondFruit, PasswordPositions[1]);
            SetFruitAtPosition(thirdFruit, PasswordPositions[2]);
            SetFruitAtPosition(fourthFruit, PasswordPositions[3]);

            ItemBeingDragged = null;
        }
        get
        {
            Fruitword fw = new Fruitword();

            fw.FirstFruit = PasswordPositions[0].MyFruitType;
            fw.SecondFruit = PasswordPositions[1].MyFruitType;
            fw.ThirdFruit = PasswordPositions[2].MyFruitType;
            fw.FourthFruit = PasswordPositions[3].MyFruitType;

            return fw;
        }
    }

    public bool IsComplete
    {
        get
        {
            for (int i = 0; i < PasswordPositions.Length; ++i)
            {
                if (PasswordPositions[i].MyItem == null)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public PasswordFruit ItemBeingDragged;

    // Use this for initialization
    void Start()
    {

    }

    public void ResetPassword()
    {
        for (int i = 0; i < PasswordPositions.Length; ++i)
        {
            if (PasswordPositions[i].MyItem != null)
            {
                PasswordPositions[i].ResetItem();
            }
        }
    }

    private void SetFruitAtPosition(PasswordFruit fruit, PasswordFruitPosition position)
    {
        if (fruit != null)
        {
            fruit.OnBeginDrag(null);
            position.OnDrop(null);
        }
        else if (PasswordPositions[0].MyItem != null)
        {
            position.ResetItem();
        }
    }
}
