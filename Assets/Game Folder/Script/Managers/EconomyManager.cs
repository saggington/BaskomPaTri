using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    [Header("Economy Settings")]
    [SerializeField] int Population;
    [SerializeField] int Money;
    [SerializeField] int Materials;
    [SerializeField] int Workers;
    [SerializeField] IncomeGenerator incomeGenerator;

    [Header("Tax and Policy")]
    [SerializeField] PolicyType CurrentPolicyType = PolicyType.None;
    [SerializeField] PolicyModifier CurrentPolicyModifier;

    [Header("Income Setting")]
    [SerializeField] float timeToGenerateIncome = 5f;

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

    private void Update()
    {
        PolicyProcessing();
    }

    private void Start()
    {
        InvokeRepeating(nameof(GenerateIncome), timeToGenerateIncome, timeToGenerateIncome);
    }

    public void GetRessource(out int population, out int money, out int materials, out int workers)
    {
        population = Population;
        money = Money;
        materials = Materials;
        workers = Workers;
    }

    public void ReduceRessource(int population, int money, int materials, int workers)
    {
        Population -= population;
        Money -= money;
        Materials -= materials;
        Workers -= workers;
    }

    public PolicyType GetCurrentPolicy()
    {
        return CurrentPolicyType;
    }

    #region POLICY MANAGEMENT
    protected void PolicyProcessing()
    {
        switch(CurrentPolicyType)
        {
            case PolicyType.Tax:
                ApplyTaxPolicy();
                break;
            case PolicyType.Subsidy:
                ApplySubsidyPolicy();
                break;
            case PolicyType.Regulation:
                ApplyRegulationPolicy();
                break;
                case PolicyType.None:
                CurrentPolicyModifier = new PolicyModifier
                {
                    policyType = PolicyType.None,
                    fPopulationMod = 1f,
                    fMoneyMod = 1f,
                    fMaterialsMod = 1f,
                    fWorkersMod = 1f
                };
                break;
        }
    }

    private void ApplyTaxPolicy()
    {
        CurrentPolicyModifier = new PolicyModifier
        {
            policyType = PolicyType.Tax,
            fPopulationMod = -0.5f,
            fMoneyMod = 2f,
            fMaterialsMod = 0.5f,
            fWorkersMod = 1f
        };
    }

    private void ApplySubsidyPolicy()
    {
        CurrentPolicyModifier = new PolicyModifier
        {
            policyType = PolicyType.Subsidy,
            fPopulationMod = 2f,
            fMoneyMod = -1f,
            fMaterialsMod = 1f,
            fWorkersMod = 0.5f
        };
    }

    private void ApplyRegulationPolicy()
    {
        CurrentPolicyModifier = new PolicyModifier
        {
            policyType = PolicyType.Regulation,
            fPopulationMod = 0.5f,
            fMoneyMod = 1f,
            fMaterialsMod = 2f,
            fWorkersMod = 1.5f
        };
    }
    #endregion

    #region INCOME GENERATION

    public void GenerateIncome()
    {
        Population += Mathf.CeilToInt(incomeGenerator.iPopulation * CurrentPolicyModifier.fPopulationMod);
        Money += Mathf.RoundToInt((incomeGenerator.iMoney * CurrentPolicyModifier.fMoneyMod) - GetUpkeepCost());
        Materials += Mathf.RoundToInt(incomeGenerator.iMaterials * CurrentPolicyModifier.fMaterialsMod);
        Workers += Mathf.RoundToInt(incomeGenerator.iWorkers * CurrentPolicyModifier.fWorkersMod);
        Debug.Log($"Income Generated: Population = {Population}, Money = {Money}, Materials = {Materials}, Workers = {Workers}");
    }

    public float GetUpkeepCost()
    {
        float upkeepCost = 0f;
        foreach(Building building in GameManager.Instance.buildings)
        {
            upkeepCost += building.upkeep;
        }
        return upkeepCost;
    }

    public void SetIncomeGenerator()
    {
        foreach(Building building in GameManager.Instance.buildings)
        {
            switch(building.type)
            {                
                case BuildingType.Commercial:
                    incomeGenerator.iMoney += building.production;
                    break;
                case BuildingType.Residential:
                    incomeGenerator.iPopulation += building.production;
                    incomeGenerator.iWorkers += Mathf.FloorToInt(building.production * CurrentPolicyModifier.fWorkersMod);
                    break;
                case BuildingType.Industrial:
                    incomeGenerator.iMaterials += building.production;
                    break;
            }
        }
    }

    #endregion
}


public enum PolicyType
{
    None = 0,
    Tax = 1,
    Subsidy = 2,
    Regulation = 3
}

[System.Serializable]
public class IncomeGenerator
{
    public PolicyType policyType;
    public int iPopulation;
    public int iMoney;
    public int iMaterials;
    public int iWorkers;
}

[System.Serializable]
public class PolicyModifier
{
    public PolicyType policyType;
    public float fPopulationMod;
    public float fMoneyMod;
    public float fMaterialsMod;
    public float fWorkersMod;
}