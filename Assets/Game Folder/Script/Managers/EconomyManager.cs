using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    [Header("Economy Settings")]
    [SerializeField] int Population;
    [SerializeField] int Money;
    [SerializeField] int Materials;
    [SerializeField] int Food;
    [SerializeField] IncomeGenerator incomeGenerator;

    [Header("Income Setting")]
    [SerializeField] float timeToGenerateIncome = 5f;

    [Header("Policy Settings")]
    [SerializeField] PolicyModifier CurrentPolicyModifier = new PolicyModifier
    {
        fPopulationMod = 1f,
        fMoneyMod = 1f,
        fMaterialsMod = 1f,
        fFoodMod = 1f
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

    public void GetRessource(out int population, out int money, out int materials, out int food)
    {
        population = Population;
        money = Money;
        materials = Materials;
        food = Food;
    }

    public void ReduceRessource(int population, int money, int materials, int food)
    {
        Population -= population;
        Money -= money;
        Materials -= materials;
        Food -= food;
    }

    public void AddRessource(int population, int money, int materials, int food)
    {
        Population += population;
        Money += money;
        Materials += materials;
        Food += food;
    }


    #region POLICY MANAGEMENT
    protected void PolicyProcessing()
    {
        GetRandomPolicy(PolicyType.Tax, out string taxName, out string taxDesc, out PolicyModifier taxMod);
        AddPolicyModifier(taxMod);
    }

    public void AddPolicyModifier(PolicyModifier mod)
    {
        CurrentPolicyModifier.fPopulationMod += mod.fPopulationMod;
        CurrentPolicyModifier.fMoneyMod += mod.fMoneyMod;
        CurrentPolicyModifier.fMaterialsMod += mod.fMaterialsMod;
        CurrentPolicyModifier.fFoodMod += mod.fFoodMod;
        Debug.Log($"Policy Modifier Applied: Population = {CurrentPolicyModifier.fPopulationMod}, Money = {CurrentPolicyModifier.fMoneyMod}, Materials = {CurrentPolicyModifier.fMaterialsMod}, Workers = {CurrentPolicyModifier.fFoodMod}");
    }

    public void GetRandomPolicy(PolicyType type, out string name, out string desc, out PolicyModifier mod)
    {
        string names = "No Policy";
        string descriptions = "No Policy Description";
        PolicyModifier policyModifier = new PolicyModifier
        {
            fPopulationMod = 1f,
            fMoneyMod = 1f,
            fMaterialsMod = 1f,
            fFoodMod = 1f
        };
        
        switch(type)
        {
            case PolicyType.Tax:
                names = policies.taxPolicy.TaxDescription[Random.Range(0, policies.taxPolicy.TaxDescription.Length - 1)].Name;
                descriptions = policies.taxPolicy.TaxDescription[Random.Range(0, policies.taxPolicy.TaxDescription.Length - 1)].Description;
                policyModifier = policies.taxPolicy.TaxModifiers[Random.Range(0, policies.taxPolicy.TaxModifiers.Length - 1)];
                break;
            case PolicyType.Subsidy:
                names = policies.foodPolicy.FoodDescription[Random.Range(0, policies.foodPolicy.FoodDescription.Length - 1)].Name;
                descriptions = policies.foodPolicy.FoodDescription[Random.Range(0, policies.foodPolicy.FoodDescription.Length - 1)].Description;
                policyModifier = policies.foodPolicy.FoodModifiers[Random.Range(0, policies.foodPolicy.FoodModifiers.Length - 1)];
                break;
            case PolicyType.Regulation:
                names = policies.materialPolicy.MaterialDescription[Random.Range(0, policies.materialPolicy.MaterialDescription.Length - 1)].Name;
                descriptions = policies.materialPolicy.MaterialDescription[Random.Range(0, policies.materialPolicy.MaterialDescription.Length - 1)].Description;
                policyModifier = policies.materialPolicy.MaterialModifiers[Random.Range(0, policies.materialPolicy.MaterialModifiers.Length - 1)];
                break;
            default:
                Debug.LogWarning("No valid policy type provided. Defaulting to None.");
                break;
        }

        Debug.Log($"Random Policy Selected: {names} - {descriptions} with modifiers: Population = {policyModifier.fPopulationMod}, Money = {policyModifier.fMoneyMod}, Materials = {policyModifier.fMaterialsMod}, Workers = {policyModifier.fFoodMod}");
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
                    incomeGenerator.iFood += Mathf.FloorToInt(building.production * CurrentPolicyModifier.fFoodMod);
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
    public int iFood;
}

[System.Serializable]
public class PolicyModifier
{
    public float fPopulationMod;
    public float fMoneyMod;
    public float fMaterialsMod;
    public float fFoodMod;
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