using UnityEngine;

public class Popup : MonoBehaviour
{
    public BuildingBonus buildingBonus;
    public Transform popupPlace;
    public bool isClickable = false;

    private void OnMouseUpAsButton()
    {
        if (!isClickable)
        {
            Debug.LogWarning("Popup is not clickable at the moment.");
            return;
        }
        Destroy(popupPlace.GetChild(0).gameObject);
        EconomyManager.Instance.AddRessource(Mathf.RoundToInt(buildingBonus.Population), Mathf.RoundToInt(buildingBonus.Money), Mathf.RoundToInt(buildingBonus.Materials), Mathf.RoundToInt(buildingBonus.Food));
        isClickable = false;
    }

    private void LateUpdate()
    {
        if(isClickable)
        {
            popupPlace.LookAt(Camera.main.transform);
            popupPlace.rotation = Quaternion.Euler(0, popupPlace.rotation.eulerAngles.y, 0);
        }
    }
}
