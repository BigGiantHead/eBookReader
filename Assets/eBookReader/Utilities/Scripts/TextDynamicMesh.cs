using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TextDynamicMesh : MegaBookDynamicMesh
{
    public MeshFilter TextGeneratorMeshFilter = null;

    public TypogenicText TextGenerator = null;

    public override void BuildMesh(int page, bool front)
    {
        TextGenerator.Text = "Page " + page + " " + front + " dasdas dasd asd asd asd asd ASD as dasd adadasd sdzfsdf sdfg sdfgsdf g dsf gsdfg sdf gsdfg sdfgsd";
        TextGenerator.RebuildMesh();

        mesh = TextGeneratorMeshFilter.sharedMesh;
    }
}
