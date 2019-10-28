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
    public List<LineRenderer> lines = new List<LineRenderer>();//shows the plants this plant supports
    public List<Plant> supportedBy = new List<Plant>();
    public List<Plant> dependents = new List<Plant>();

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

    }
    public void SetType(PlantType newType)
    {
        type = newType;
        if (type != PlantType.None && !child)
        {
            string modelName = prefabs[(int)type];
            child = Instantiate(Resources.Load(modelName), transform) as GameObject;
        }
        UpdateDependecies();
    }
    public void UpdateDependecies()
    {
        supportedBy.Clear();
        dependents.Clear();
        switch (type)
        {
            case PlantType.None:
                foreach(LineRenderer line in lines)
                {
                    line.enabled = false;
                }
                break;
            case PlantType.Spread:
                for(int i = 0; i < neighbors.Count; i++)
                {
                    if(neighbors[i].type == PlantType.Spread || neighbors[i].type == PlantType.None)
                    {
                        lines[i].enabled = false;
                    }
                    else
                    {
                        lines[i].enabled = true;
                        dependents.Add(neighbors[i]);
                    }
                }
                break;
            case PlantType.Grass:
                for (int i = 0; i < neighbors.Count; i++)
                {
                    if (neighbors[i].type == PlantType.Grass || neighbors[i].type == PlantType.None)
                    {
                        lines[i].enabled = false;
                    }else if(neighbors[i].type == PlantType.Spread)
                    {
                        supportedBy.Add(neighbors[i]);
                    }
                    else
                    {
                        lines[i].enabled = true;
                        dependents.Add(neighbors[i]);
                    }
                }
                break;
            case PlantType.Shrub:
                for (int i = 0; i < neighbors.Count; i++)
                {
                    if (neighbors[i].type == PlantType.Shrub || neighbors[i].type == PlantType.None)
                    {
                        lines[i].enabled = false;
                    }else if(neighbors[i].type == PlantType.Grass || neighbors[i].type == PlantType.Spread)
                    {
                        supportedBy.Add(neighbors[i]);
                    }
                    else
                    {
                        lines[i].enabled = true;
                        dependents.Add(neighbors[i]);
                    }
                }
                break;
            case PlantType.Tree:
                for (int i = 0; i < neighbors.Count; i++)
                {
                    if (neighbors[i].type == PlantType.Tree || neighbors[i].type == PlantType.None)
                    {
                        lines[i].enabled = false;
                    }else if (neighbors[i].type == PlantType.Shrub || neighbors[i].type == PlantType.Grass || neighbors[i].type == PlantType.Spread)
                    {
                        supportedBy.Add(neighbors[i]);
                    }
                    else
                    {
                        lines[i].enabled = true;
                        dependents.Add(neighbors[i]);
                    }
                }
                break;
        }
    }
}
