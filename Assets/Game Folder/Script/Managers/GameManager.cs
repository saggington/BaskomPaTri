using UnityEngine;
using itsmakingthings_daynightcycle;

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
    public Building[] buildings;

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

    private void Update()
    {

    }

    public Building FindBuilding(string name)
    {
        foreach (Building building in buildings)
        {
            if (building.name.Equals(name, System.StringComparison.OrdinalIgnoreCase))
            {
                return building;
            }
        }
        return null;
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

    public Building(string name, int level, int cost, int upkeep, int production)
    {
        this.name = name;
        this.level = level;
        this.cost = cost;
        this.upkeep = upkeep;
        this.production = production;
    }
}

public enum BuildingType
{
    Undefined = 0,
    Residential,
    Commercial,
    Industrial
}