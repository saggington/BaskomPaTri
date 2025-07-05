using UnityEngine;

public class GovernorAI : MonoBehaviour
{
    public Building[] BuildableBuilding;

    public Building FindBuilding(string name)
    {
        foreach (Building building in BuildableBuilding)
        {
            if (building.name.Equals(name, System.StringComparison.OrdinalIgnoreCase))
            {
                return building;
            }
        }
        return null;
    }

}
