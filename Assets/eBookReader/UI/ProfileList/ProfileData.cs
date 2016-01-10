using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ProfileData
{
    public Guid ID;

    public string UserName;

    public string Avatar;

    public long LastUsed;

    public ProfileData()
    {
        ID = Guid.NewGuid();
    }
}
