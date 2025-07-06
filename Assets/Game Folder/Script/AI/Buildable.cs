using UnityEngine;

public class Buildable : MonoBehaviour
{
    public BuildingType buildingType;
    public Building Building;

    private void Start()
    {
        GameManager.Instance.RegisterTile(this);
    }
}

[System.Serializable]
public class BuildingBonus
{
    public float Money;
    public float Population;
    public float Materials;
    public float Food;
    public float chances;
}
