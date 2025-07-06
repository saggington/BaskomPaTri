using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    [Header("Economy Settings")]
    [SerializeField] int Population;
    [SerializeField] int Money;
    [SerializeField] int Materials;
    [SerializeField] IncomeGenerator incomeGenerator;

    [Header("Income Setting")]
    [SerializeField] float timeToGenerateIncome = 5f;

    [Header("Policy Settings")]
    [SerializeField] PolicyModifier CurrentPolicyModifier = new PolicyModifier
    {
        fPopulationMod = 1f,
        fMoneyMod = 1f,
        fMaterialsMod = 1f,
        fWorkersMod = 1f
    };

    [SerializeField] PolicySO policies;

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
        InvokeRepeating(nameof(GenerateIncome), timeToGenerateIncome, timeToGenerateIncome);
        PolicyProcessing();
    }

    public void GetRessource(out int population, out int money, out int materials)
    {
        population = Population;
        money = Money;
        materials = Materials;
    }

    public void ReduceRessource(int population, int money, int materials)
    {
        Population -= population;
        Money -= money;
        Materials -= materials;
    }

    public void AddRessource(int population, int money, int materials)
    {
        Population += population;
        Money += money;
        Materials += materials;
    }


    #region POLICY MANAGEMENT
    protected void PolicyProcessing()
    {
        GetRandomPolicy(PolicyType.Tax, out string taxName, out string taxDesc, out PolicyModifier taxMod);
        AddPolicyModifier(taxMod);
    }

    protected void AddPolicyModifier(PolicyModifier mod)
    {
        CurrentPolicyModifier.fPopulationMod += mod.fPopulationMod;
        CurrentPolicyModifier.fMoneyMod += mod.fMoneyMod;
        CurrentPolicyModifier.fMaterialsMod += mod.fMaterialsMod;
        CurrentPolicyModifier.fWorkersMod += mod.fWorkersMod;
        Debug.Log($"Policy Modifier Applied: Population = {CurrentPolicyModifier.fPopulationMod}, Money = {CurrentPolicyModifier.fMoneyMod}, Materials = {CurrentPolicyModifier.fMaterialsMod}, Workers = {CurrentPolicyModifier.fWorkersMod}");
    }

    protected void GetRandomPolicy(PolicyType type, out string name, out string desc, out PolicyModifier mod)
    {
        string names = "No Policy";
        string descriptions = "No Policy Description";
        PolicyModifier policyModifier = new PolicyModifier
        {
            fPopulationMod = 1f,
            fMoneyMod = 1f,
            fMaterialsMod = 1f,
            fWorkersMod = 1f
        };
        
        switch(type)
        {
            case PolicyType.Tax:
                names = policies.taxPolicy.TaxDescription[Random.Range(0, policies.taxPolicy.TaxDescription.Length)].Name;
                descriptions = policies.taxPolicy.TaxDescription[Random.Range(0, policies.taxPolicy.TaxDescription.Length)].Description;
                policyModifier = policies.taxPolicy.TaxModifiers[Random.Range(0, policies.taxPolicy.TaxModifiers.Length)];
                break;
            case PolicyType.Subsidy:
                names = policies.foodPolicy.FoodDescription[Random.Range(0, policies.foodPolicy.FoodDescription.Length)].Name;
                descriptions = policies.foodPolicy.FoodDescription[Random.Range(0, policies.foodPolicy.FoodDescription.Length)].Description;
                policyModifier = policies.foodPolicy.FoodModifiers[Random.Range(0, policies.foodPolicy.FoodModifiers.Length)];
                break;
            case PolicyType.Regulation:
                names = policies.materialPolicy.MaterialDescription[Random.Range(0, policies.materialPolicy.MaterialDescription.Length)].Name;
                descriptions = policies.materialPolicy.MaterialDescription[Random.Range(0, policies.materialPolicy.MaterialDescription.Length)].Description;
                policyModifier = policies.materialPolicy.MaterialModifiers[Random.Range(0, policies.materialPolicy.MaterialModifiers.Length)];
                break;
            default:
                Debug.LogWarning("No valid policy type provided. Defaulting to None.");
                break;
        }

        Debug.Log($"Random Policy Selected: {names} - {descriptions} with modifiers: Population = {policyModifier.fPopulationMod}, Money = {policyModifier.fMoneyMod}, Materials = {policyModifier.fMaterialsMod}, Workers = {policyModifier.fWorkersMod}");
        name = names;
        desc = descriptions;
        mod = policyModifier;
    }

    #endregion

    #region INCOME GENERATION

    public void GenerateIncome()
    {
        Population += Mathf.CeilToInt(incomeGenerator.iPopulation * CurrentPolicyModifier.fPopulationMod);
        Money += Mathf.RoundToInt((incomeGenerator.iMoney * CurrentPolicyModifier.fMoneyMod));
        Materials += Mathf.RoundToInt(incomeGenerator.iMaterials * CurrentPolicyModifier.fMaterialsMod);
        Debug.Log($"Income Generated: Population = {Population}, Money = {Money}, Materials = {Materials}");
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
    public float fPopulationMod;
    public float fMoneyMod;
    public float fMaterialsMod;
    public float fWorkersMod;
}

[System.Serializable]
public class Policy
{
    public PolicyType policyType;
    public string name;
    public string description;
    public PolicyModifier modifier;
    public Policy(PolicyType type, string name, string description, PolicyModifier modifier)
    {
        this.policyType = type;
        this.name = name;
        this.description = description;
        this.modifier = modifier;
    }
}