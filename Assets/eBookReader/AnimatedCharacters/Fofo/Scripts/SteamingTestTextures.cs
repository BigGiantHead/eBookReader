using UnityEngine;
using System.Collections;

public class SteamingTestTextures : MonoBehaviour
{
    public Renderer Renderer = null;

    public string Body = "";

    public string Fur = "";

    public string Noise = "";

    // Use this for initialization
    void Start()
    {
        StartCoroutine(DoLoadAssets());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            StartCoroutine(DoLoadAssets());
        }
    }

    public IEnumerator DoLoadAssets()
    {
        WWW texture = new WWW("file://" + Application.streamingAssetsPath + "/" + Body);

        yield return texture;

        Renderer.material.SetTexture("_SkinTex", texture.texture);

        texture = new WWW("file://" + Application.streamingAssetsPath + "/" + Fur);

        yield return texture;

        Renderer.material.SetTexture("_MainTex", texture.texture);

        texture = new WWW("file://" + Application.streamingAssetsPath + "/" + Noise);

        yield return texture;

        Renderer.material.SetTexture("_NoiseTex", texture.texture);

        yield break;
    }
}
