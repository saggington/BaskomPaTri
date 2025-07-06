using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GovernorAI : MonoBehaviour
{
    public Building[] BuildableBuilding;
    [SerializeField] Buildable[] buildableTiles;
    public float BuildChance = 0.5f;
    public float ResidentialChance = 0.5f;
    public float CommercialChance = 0.3f;
    public float IndustrialChance = 0.2f;

    public float ConsiderBuildInterval = 10f; // Time in seconds between build considerations

    private void Start()
    {
        InvokeRepeating(nameof(ConsiderBuild), ConsiderBuildInterval, ConsiderBuildInterval);
    }

    public Building FindBuilding(string name)
    {
        foreach (Building building in BuildableBuilding)
        {
            if (building.name.Equals(name, System.StringComparison.OrdinalIgnoreCase))
            {
                return building;
            }
        }
        return null;
    }

    public void RegisterTile(Buildable type, Vector3 position)
    {
        System.Array.Resize(ref buildableTiles, buildableTiles.Length + 1);
        buildableTiles[buildableTiles.Length - 1] = type;
        buildableTiles = buildableTiles.OrderBy((d) => (d.transform.position - transform.position).sqrMagnitude).ToArray();
    }

    public Building FindBuildingByType(BuildingType type)
    {
        List<Building> buildableBuilding = new List<Building>();
        foreach (Building building in BuildableBuilding)
        {
            if (building.type == type)
            {
                buildableBuilding.Add(building);
            }
        }

        Debug.Log($"Governor AI: Searching for buildings of type {type} - Found {buildableBuilding.Count} candidates.");

        if (buildableBuilding.Count > 0)
        {
            Debug.Log($"Governor AI: Found {buildableBuilding.Count} buildings of type {type}");
            return buildableBuilding[Random.Range(0, buildableBuilding.Count)];
        }

        return null;
    }

    public void ConsiderBuild()
    {
        if (buildableTiles[0] == null)
            CancelInvoke(nameof(ConsiderBuild));
        int population, money, materials = 0;
        EconomyManager.Instance.GetRessource(out population, out money, out materials);
        //Debug.Log($"Governor AI: Consider Build - Population: {population}, Money: {money}, Materials: {materials}, Workers: {workers}");
        if (Random.value < BuildChance)
        {
            if (CommercialChance > Random.value)
            {
                Building building = FindBuildingByType(BuildingType.Commercial);
                if (building != null && money >= building.cost.money && materials >= building.cost.materials && population >= building.cost.population)
                {
                    //Debug.Log($"Governor AI: Building Commercial at {buildableTiles[0].transform.position}");
                    GameManager.Instance.BuildBuilding(building, buildableTiles[0].transform);
                    buildableTiles[0].Building = building;
                    buildableTiles[0].buildingType = building.type;
                    buildableTiles = buildableTiles.Skip(1).ToArray();
                    SpawnChancesRefactor(building.type);
                }
            }
            else if (ResidentialChance > Random.value)
            {
                Building building = FindBuildingByType(BuildingType.Residential);
                if (building != null && money >= building.cost.money && materials >= building.cost.materials && population >= building.cost.population)
                {
                    //Debug.Log($"Governor AI: Building Residential at {buildableTiles[0].transform.position}");
                    GameManager.Instance.BuildBuilding(building, buildableTiles[0].transform);
                    buildableTiles[0].Building = building;
                    buildableTiles[0].buildingType = building.type;
                    buildableTiles = buildableTiles.Skip(1).ToArray();
                    SpawnChancesRefactor(building.type);
                }
            }
            else if (IndustrialChance > Random.value)
            {
                Building building = FindBuildingByType(BuildingType.Industrial);
                if (building != null && money >= building.cost.money && materials >= building.cost.materials && population >= building.cost.population)
                {
                    //Debug.Log($"Governor AI: Building Industrial at {buildableTiles[0].transform.position}");
                    GameManager.Instance.BuildBuilding(building, buildableTiles[0].transform);
                    buildableTiles[0].Building = building;
                    buildableTiles[0].buildingType = building.type;
                    buildableTiles = buildableTiles.Skip(1).ToArray();
                    SpawnChancesRefactor(building.type);
                }
            }
            else             
            {
                Debug.Log("Governor AI: No suitable building type found or insufficient resources.");
            }
        }
    }

    public void SpawnChancesRefactor(BuildingType type)
    {
        switch(type)
        {
            case BuildingType.Commercial:
                CommercialChance = Mathf.Clamp(CommercialChance - 0.1f, 0f, 1f);
                ResidentialChance = Mathf.Clamp(ResidentialChance + 0.05f, 0f, 1f);
                IndustrialChance = Mathf.Clamp(IndustrialChance + 0.05f, 0f, 1f);
                break;
            case BuildingType.Residential:
                CommercialChance = Mathf.Clamp(CommercialChance + 0.05f, 0f, 1f);
                ResidentialChance = Mathf.Clamp(ResidentialChance - 0.1f, 0f, 1f);
                IndustrialChance = Mathf.Clamp(IndustrialChance + 0.05f, 0f, 1f);
                break;
            case BuildingType.Industrial:
                CommercialChance = Mathf.Clamp(CommercialChance + 0.05f, 0f, 1f);
                ResidentialChance = Mathf.Clamp(ResidentialChance + 0.05f, 0f, 1f);
                IndustrialChance = Mathf.Clamp(IndustrialChance - 0.1f, 0f, 1f);
                break;
            default:
                Debug.LogWarning("Governor AI: Invalid building type provided for spawn chances refactor.");
                break;
        }
    }
}
