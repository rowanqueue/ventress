using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlantType
{
    Spread,Grass,Shrub,Tree,Special
}

public class Plant : MonoBehaviour
{
    public string desc;
    public PlantType type;

    int age;
    float growthStage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
