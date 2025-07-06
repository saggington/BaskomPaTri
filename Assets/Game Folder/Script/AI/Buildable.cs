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
