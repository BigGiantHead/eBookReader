using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasRenderer), typeof(MeshFilter), typeof(Collider))]
public class UIMesh : MaskableGraphic
{
    private Collider myCollider = null;

    public Texture Texture = null;

    protected override void Awake()
    {
        base.Awake();

        myCollider = GetComponent<Collider>();
    }

    protected override void UpdateGeometry()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

        canvasRenderer.Clear();

        canvasRenderer.SetMesh(mesh);
    }

    protected override void UpdateMaterial()
    {
        base.UpdateMaterial();

        canvasRenderer.SetColor(color);

        if (Texture != null)
        {
            canvasRenderer.SetTexture(Texture);
        }
    }

    public override bool Raycast(Vector2 sp, Camera eventCamera)
    {
        if (myCollider == null)
        {
            return false;
        }

        Ray ray = eventCamera.ScreenPointToRay(sp);
        RaycastHit hit;

        bool collision = myCollider.Raycast(ray, out hit, float.MaxValue);

        return collision;
    }
}
