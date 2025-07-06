using UnityEngine;

public class Popup : MonoBehaviour
{
    public BuildingBonus buildingBonus;
    public bool isClickable = false;

    private void OnMouseUpAsButton()
    {
        if (!isClickable)
        {
            Debug.LogWarning("Popup is not clickable at the moment.");
            return;
        }
        EconomyManager.Instance.AddRessource(Mathf.RoundToInt(buildingBonus.Population), Mathf.RoundToInt(buildingBonus.Money), Mathf.RoundToInt(buildingBonus.Materials));
        isClickable = false;
    }
}
