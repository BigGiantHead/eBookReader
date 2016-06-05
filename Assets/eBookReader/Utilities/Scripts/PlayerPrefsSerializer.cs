using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerPrefsSerializer
{
    public static BinaryFormatter bf = new BinaryFormatter();

    public static void Save(string prefKey, object serializableObject)
    {
        if (serializableObject == null)
        {
            if (PlayerPrefs.HasKey(prefKey))
            {
                PlayerPrefs.DeleteKey(prefKey);
            }
        }
        else
        {
            MemoryStream memoryStream = new MemoryStream();
            bf.Serialize(memoryStream, serializableObject);

            string tmp = Convert.ToBase64String(memoryStream.ToArray());

            PlayerPrefs.SetString(prefKey, tmp);
        }
    }

    public static T Load<T>(string prefKey)
    {
        if (!PlayerPrefs.HasKey(prefKey))
            return default(T);

        string serializedData = PlayerPrefs.GetString(prefKey);
        MemoryStream dataStream = new MemoryStream(Convert.FromBase64String(serializedData));

        T deserializedObject = (T)bf.Deserialize(dataStream);

        return deserializedObject;
    }
}