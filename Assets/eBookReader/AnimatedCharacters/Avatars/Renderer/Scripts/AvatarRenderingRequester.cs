using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AvatarRenderingRequester : MonoBehaviour
{
    private AvatarRenderer.RenderingData renderingData = null;

    public RawImage TextureReceiver = null;

    void Awake()
    {
        TextureReceiver.enabled = false;
    }

	// Use this for initialization
	void Start ()
    {	
	}

    public void StartAvatar(Avatar data)
    {
        TextureReceiver.enabled = true;
        renderingData = AvatarRenderer.Instance.StartAvatarRendering(data, (int)TextureReceiver.rectTransform.rect.width, (int)TextureReceiver.rectTransform.rect.height);
        TextureReceiver.texture = renderingData.Texture;
    }

    public void StopAvatar()
    {
        TextureReceiver.enabled = false;
        if (renderingData != null && AvatarRenderer.Instance != null)
        {
            AvatarRenderer.Instance.StopAvatarRendering(renderingData);
        }
        TextureReceiver.texture = null;
    }

    void OnDestroy()
    {
        StopAvatar();
    }
}
