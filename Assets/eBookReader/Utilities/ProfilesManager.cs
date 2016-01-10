using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class ProfilesManager : MonoBehaviour
{
    private static ProfilesManager instance = null;

    public static ProfilesManager Instance
    {
        get
        {
            return instance;
        }
    }

    private Guid? currentProfileId = null;

    [HideInInspector]
    public List<ProfileData> Profiles = null;

    public ProfileData CurrentProfile
    {
        get
        {
            if (currentProfileId.HasValue)
            {
                return Profiles.Where(p => p.ID == currentProfileId.Value).FirstOrDefault();
            }

            return null;
        }
        set
        {
            if (value == null)
            {
                currentProfileId = null;
            }
            else
            {
                currentProfileId = value.ID;
            }

            PlayerPrefsSerializer.Save("CurrentProfile", currentProfileId);
        }
    }

    void Awake()
    {
        instance = this;
        currentProfileId = PlayerPrefsSerializer.Load<Guid?>("CurrentProfile");
        Profiles = PlayerPrefsSerializer.Load<List<ProfileData>>("Profiles");
        if (Profiles == null)
        {
            Profiles = new List<ProfileData>(0);
        }
    }

    // Use this for initialization
    void Start()
    {
    }

    void OnDestroy()
    {
        instance = null;
    }

    public void AddProfile(ProfileData data)
    {
        if (!Profiles.Contains(data))
        {
            Profiles.Add(data);
        }

        PlayerPrefsSerializer.Save("Profiles", Profiles);
    }

    public void RemoveProfile(ProfileData data)
    {
        if (Profiles.Contains(data))
        {
            Profiles.Remove(data);
        }

        PlayerPrefsSerializer.Save("Profiles", Profiles);
    }

    public void UpdateProfiles()
    {
        PlayerPrefsSerializer.Save("Profiles", Profiles);
    }
}
