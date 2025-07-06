using UnityEngine;
using itsmakingthings_daynightcycle;
using System;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // References to various components in the game
    [Header("Game Components")]
    protected GovernorAI governorAI;
    protected CitizenAI citizenAI;
    protected EconomyManager economyManager;
    protected DayNightCycle dayNightCycle;
    public Transform BuildingPool;

    [Header("Buildable Buildings")]
    public List<Building> buildings;
    public List<Popup> popups;
    public GameObject popupPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        dayNightCycle = GetComponent<DayNightCycle>();
        economyManager = GetComponent<EconomyManager>();
        governorAI = GetComponent<GovernorAI>();
        citizenAI = GetComponent<CitizenAI>();
    }

    public void BuildBuilding(Building b, Transform where)
    {
        var a = Instantiate(b.buildingPrefab, where.position, Quaternion.identity);
        a.transform.SetParent(BuildingPool, true);
        buildings.Add(b);
        popups.Add(new Popup
        {
            buildingBonus = new BuildingBonus
            {
                Population = b.cost.population / 2,
                Money = b.cost.money / 2,
                Materials = b.cost.materials / 2,
                chances = 0.2f,
            },
        });
        economyManager.ReduceRessource(b.cost.population, b.cost.money, b.cost.materials);
        economyManager.SetIncomeGenerator();
    }

    public void RandomClickable()
    {
        foreach (var c in popups)
        {
            if (c.buildingBonus.chances > UnityEngine.Random.value)
            {
                Instantiate(popupPrefab, c.transform.position, Quaternion.identity, BuildingPool);
                c.isClickable = true;
            }
            else
            {
                c.isClickable = false;
            }
        }
    }

    public void RegisterTile(Buildable b)
    {
        governorAI.RegisterTile(b, b.transform.position);
    }
}

[System.Serializable]
public class Building
{
    public string name;
    public BuildingType type;
    public int level;
    public BuildingCost cost;
    public int upkeep;
    public int production;
    public GameObject buildingPrefab;
}

[System.Serializable]
public struct BuildingCost
{
    public int money;
    public int materials;
    public int population;
}

public enum BuildingType
{
    Undefined = 0,
    Residential,
    Commercial,
    Industrial
}