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

    [Header("Buildable Buildings")]
    public List<Building> buildings;

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
    }

    public void BuildBuilding(Building b, Transform where)
    {
        Instantiate(b.buildingPrefab, where.position, Quaternion.identity);
        buildings.Add(b);
        economyManager.SetIncomeGenerator();
    }
}

[System.Serializable]
public class Building
{
    public string name;
    public BuildingType type;
    public int level;
    public int cost;
    public int upkeep;
    public int production;
    public GameObject buildingPrefab;
}

public enum BuildingType
{
    Undefined = 0,
    Road = 1,
    Residential,
    Commercial,
    Industrial
}