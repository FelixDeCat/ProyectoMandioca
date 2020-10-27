using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosDecals : MonoBehaviour
{
    public Renderer myRenderer;
    public Transform[] positions;
    public Vector4[] vectorPositions;
    public GameObject[] objects;
    public MaterialPropertyBlock materialProperty;

    public GameObject[] sphereCorruption;


    void Start()
    {
        myRenderer = GetComponent<Renderer>();
        materialProperty = new MaterialPropertyBlock();
        objects = GameObject.FindGameObjectsWithTag("ShaderEffect");
        positions = new Transform[objects.Length];
        vectorPositions = new Vector4[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            positions[i] = objects[i].transform;
        }
    }

    private void Update()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if(vectorPositions[i] != null && positions[i] != null)
            vectorPositions[i] = new Vector4(positions[i].position.x, positions[i].position.y, positions[i].position.z, 0);
        }

      

        if (vectorPositions.Length > 0)
        {
            materialProperty.SetVectorArray("positionsArray", vectorPositions);
        }
        myRenderer.SetPropertyBlock(materialProperty);
    }
}
