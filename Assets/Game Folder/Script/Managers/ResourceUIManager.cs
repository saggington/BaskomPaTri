using TMPro;
using UnityEngine;

public class ResourceUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _population;


    public void UpdateResource(int population, int food, int BM, int money)
    {
        Debug.Log("UPDATED");
        _population.text = "Population: " + population.ToString();
    }
}
