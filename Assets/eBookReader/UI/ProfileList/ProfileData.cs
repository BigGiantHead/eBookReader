using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ProfileData
{
    public Guid ID;

    public string UserName;

    public Avatar Avatar;

    public Fruitword Password;

    public long LastUsed;

    public ProfileData()
    {
        ID = Guid.NewGuid();
        Password = new Fruitword();
        Avatar = new Avatar();
    }
}

[Serializable]
public class Avatar
{
    public PickAvatar.Gender Gender = PickAvatar.Gender.Male;

    public int HairIndex = 0;

    public int BlouseIndex = 0;

    public int PantsIndex = 0;

    public int ShoesIndex = 0;
}

[Serializable]
public class Fruitword
{
    public FruitType FirstFruit;

    public FruitType SecondFruit;

    public FruitType ThirdFruit;

    public FruitType FourthFruit;

    public FruitType this[int i]
    {
        get
        {
            switch (i)
            {
                case 0:
                    return FirstFruit;
                case 1:
                    return SecondFruit;
                case 2:
                    return ThirdFruit;
                case 3:
                    return FourthFruit;
            }

            return FruitType.None;
        }
        set
        {
            switch (i)
            {
                case 0:
                    FirstFruit = value;
                    break;

                case 1:
                    SecondFruit = value;
                    break;

                case 2:
                    ThirdFruit = value;
                    break;

                case 3:
                    FourthFruit = value;
                    break;
            }
        }
    }
}


public enum FruitType
{
    None = 0,

    Apple = 1,

    Banana = 2,

    Cherry = 3,

    Grapes = 4,

    Kiwi = 5,

    Lemon = 6,

    Orange = 7,

    Peach = 8,

    Pear = 9,

    Plum = 10,

    Strawberry = 11,

    Watermelon = 12,

    Empty = 13
}