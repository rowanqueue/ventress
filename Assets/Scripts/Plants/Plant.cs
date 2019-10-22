using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlantType
{
    None,Spread,Grass,Shrub,Tree,Special
}

public class Plant : MonoBehaviour
{
    public string desc;
    public PlantType type;
    public List<string> prefabs = new List<string>() {"None", "Spread", "Grass", "Shrub", "Tree", "Special" };
    public List<Plant> neighbors = new List<Plant>();

    GameObject child;
    int age;
    float growthStage;

    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (type != PlantType.None && !child)
        {
            string modelName = prefabs[(int)type];
            child = Instantiate(Resources.Load(modelName),transform) as GameObject;
        }

    }
}
