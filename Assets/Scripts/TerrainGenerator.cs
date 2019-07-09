using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    Terrain terrain;
    public float HillFrequency = 10.0f;
    public float LowestHillHeight;
    public float HighestHillHeight;
    // Start is called before the first frame update
    void Start()
    {
        terrain = GetComponent<Terrain>();
        GenerateHeights(10f);
    }

    public void GenerateHeights(float tileSize)
    {
        float hillHeight = (float)((float)HighestHillHeight - (float)LowestHillHeight) / ((float)terrain.terrainData.heightmapResolution / 2);
        float baseHeight = (float)LowestHillHeight / ((float)terrain.terrainData.heightmapResolution / 2);
        float[,] heights = new float[terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution];

        for (int i = 0; i < terrain.terrainData.heightmapResolution; i++)
        {
            for (int k = 0; k < terrain.terrainData.heightmapResolution; k++)
            {
                heights[i, k] = baseHeight + (Mathf.PerlinNoise(((float)i / (float)terrain.terrainData.heightmapResolution) * tileSize, ((float)k / (float)terrain.terrainData.heightmapResolution) * tileSize) * (float)hillHeight);
            }
        }

        terrain.terrainData.SetHeights(0, 0, heights);
        Debug.Log("A");
    }
}
