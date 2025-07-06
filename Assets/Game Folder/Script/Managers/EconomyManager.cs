using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    [Header("Economy Settings")]
    [SerializeField] int Population;
    [SerializeField] int Food;
    [SerializeField] int Money;
    [SerializeField] int Materials;
    [SerializeField] IncomeGenerator incomeGenerator;

    [Header("Income Setting")]
    [SerializeField] float timeToGenerateIncome = 5f;

<<<<<<< Updated upstream
    [Header("Policy Settings")]
    [SerializeField] PolicyModifier CurrentPolicyModifier = new PolicyModifier
    {
        fPopulationMod = 1f,
        fMoneyMod = 1f,
        fMaterialsMod = 1f,
        fWorkersMod = 1f
    };

    [SerializeField] PolicySO policies;

=======
    [SerializeField] private ResourceUIManager resourceUIManager;
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
    public void GetRessource(out int population, out int money, out int materials)
=======
    public void GetRessource(out int population, out int food, out int money, out int materials, out int workers)
>>>>>>> Stashed changes
    {
        population = Population;
        food = Food;
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
<<<<<<< Updated upstream
        GetRandomPolicy(PolicyType.Tax, out string taxName, out string taxDesc, out PolicyModifier taxMod);
        AddPolicyModifier(taxMod);
=======
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
                    fFoodMod = 1f,
                    fMoneyMod = 1f,
                    fMaterialsMod = 1f,
                    fWorkersMod = 1f
                };
                break;
        }
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
            fPopulationMod = 1f,
            fMoneyMod = 1f,
            fMaterialsMod = 1f,
=======
            policyType = PolicyType.Tax,
            fPopulationMod = -0.5f,
            fFoodMod = -0.5f,
            fMoneyMod = 2f,
            fMaterialsMod = 0.5f,
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
=======
    private void ApplySubsidyPolicy()
    {
        CurrentPolicyModifier = new PolicyModifier
        {
            policyType = PolicyType.Subsidy,
            fPopulationMod = 2f,
            fFoodMod = 1.5f,
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
            fFoodMod = 1f,
            fMoneyMod = 1f,
            fMaterialsMod = 2f,
            fWorkersMod = 1.5f
        };
    }
>>>>>>> Stashed changes
    #endregion

    #region INCOME GENERATION

    public void GenerateIncome()
    {
        Population += Mathf.CeilToInt(incomeGenerator.iPopulation * CurrentPolicyModifier.fPopulationMod);
<<<<<<< Updated upstream
        Money += Mathf.RoundToInt((incomeGenerator.iMoney * CurrentPolicyModifier.fMoneyMod));
        Materials += Mathf.RoundToInt(incomeGenerator.iMaterials * CurrentPolicyModifier.fMaterialsMod);
        Debug.Log($"Income Generated: Population = {Population}, Money = {Money}, Materials = {Materials}");
=======
        Food += Mathf.RoundToInt(incomeGenerator.iFood * CurrentPolicyModifier.fFoodMod);
        Money += Mathf.RoundToInt((incomeGenerator.iMoney * CurrentPolicyModifier.fMoneyMod) - GetUpkeepCost());
        Materials += Mathf.RoundToInt(incomeGenerator.iMaterials * CurrentPolicyModifier.fMaterialsMod);
        Workers += Mathf.RoundToInt(incomeGenerator.iWorkers * CurrentPolicyModifier.fWorkersMod);
        Debug.Log($"Income Generated: Population = {Population}, Money = {Money}, Materials = {Materials}, Workers = {Workers}");

        resourceUIManager.UpdateResource(Population, Food, Materials, Money);
>>>>>>> Stashed changes
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
                    incomeGenerator.iFood += building.production;
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
    public int iFood;
    public int iMoney;
    public int iMaterials;
    public int iWorkers;
}

[System.Serializable]
public class PolicyModifier
{
    public float fPopulationMod;
    public float fFoodMod;
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