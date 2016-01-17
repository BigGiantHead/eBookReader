using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ProfileData
{
    public Guid ID;

    public string UserName;

    public string Avatar;

    public Fruitword Password;

    public long LastUsed;

    public ProfileData()
    {
        ID = Guid.NewGuid();
        Password = new Fruitword();
    }
}

[Serializable]
public class Fruitword
{
    public FruitType FirstFruit;

    public FruitType SecondFruit;

    public FruitType ThirdFruit;

    public FruitType FourthFruit;
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