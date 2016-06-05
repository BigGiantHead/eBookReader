using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AvatarRenderer : SingletonBehaviour<AvatarRenderer>
{
    public class RenderingData
    {
        public System.Guid Id = System.Guid.NewGuid();

        public RenderTexture Texture = null;

        public Avatar Data = null;
    }

    private Kid_Customizer active = null;

    private List<RenderingData> requestedTextures = new List<RenderingData>(0);

    public Kid_Customizer[] Customizers = null;

    public Camera MyCamera = null;

    public int MaxTextureWidth = 512;

    public int MaxTextureHeight = 512;

    void Start()
    {
    }

    void FixedUpdate()
    {
        for (int i = 0; i < requestedTextures.Count; ++i)
        {
            RenderingData rendererData = requestedTextures[i];

            BindDataToAvatar(rendererData.Data);

            MyCamera.targetTexture = rendererData.Texture;

            MyCamera.Render();
        }

        MyCamera.targetTexture = null;
    }

    public RenderingData StartAvatarRendering(Avatar data, int width, int height)
    {
        RenderTexture texture = new RenderTexture(Mathf.Min(width, MaxTextureWidth), Mathf.Min(height, MaxTextureHeight), 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
        texture.filterMode = FilterMode.Bilinear;

        RenderingData arData = new RenderingData()
        {
            Texture = texture,
            Data = data
        };

        requestedTextures.Add(arData);

        MyCamera.enabled = requestedTextures.Count > 0;

        return arData;
    }

    public void StopAvatarRendering(RenderingData arData)
    {
        Destroy(arData.Texture);
        requestedTextures.Remove(arData);

        MyCamera.enabled = requestedTextures.Count > 0;
    }
    
    public Kid_Customizer GetCustomizerAtIndex(int index)
    {
        if (index >= Customizers.Length || index < 0)
        {
            return null;
        }

        return Customizers[index];
    }

    private void BindDataToAvatar(Avatar data)
    {
        Kid_Customizer active = null;

        int index = (int)data.Gender - 1;
        if (index >= Customizers.Length || index < 0)
        {
            return;
        }

        for (int i = 0; i < Customizers.Length; ++i)
        {
            if (i == index)
            {
                active = Customizers[index];
                Customizers[i].gameObject.transform.localPosition = new Vector3(0, 0, 1);
            }
            else
            {
                Customizers[i].gameObject.transform.localPosition = new Vector3(0, 0, 0);
            }
        }

        active.ChangeHairModel(data.HairIndex);
        active.ChangeBlouseModel(data.BlouseIndex);
        active.ChangePantsModel(data.PantsIndex);
        active.ChangeShoesModel(data.ShoesIndex);

        //TODO: add logo implementation
        //active.ChangeLogoModel(data.LogoIndex);
    }
}
