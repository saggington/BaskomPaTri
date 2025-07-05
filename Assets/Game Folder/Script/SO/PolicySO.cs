using UnityEngine;

[CreateAssetMenu(fileName = "PolicySO", menuName = "ScriptableObjects/PolicySO", order = 1)]
public class PolicySO : ScriptableObject
{
    public TaxPolicy taxPolicy;
    public FoodPolicy foodPolicy;
    public MaterialPolicy materialPolicy;
}

[System.Serializable]
public class TaxPolicy
{
    public Desc[] TaxDescription;
    public PolicyModifier[] TaxModifiers;
}

[System.Serializable]
public class FoodPolicy
{
    public Desc[] FoodDescription;
    public PolicyModifier[] FoodModifiers;
}

[System.Serializable]
public class MaterialPolicy
{
    public Desc[] MaterialDescription;
    public PolicyModifier[] MaterialModifiers;
}

[System.Serializable]
public class Desc
{
    public string Name;
    [TextArea] public string Description;
}