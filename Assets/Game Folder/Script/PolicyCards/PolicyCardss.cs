using UnityEngine;

public class PolicyCardss : MonoBehaviour
{
    public PolicyModifier PolicyModifier;

    public void ApplyPolicy()
    {
        EconomyManager.Instance.AddPolicyModifier(PolicyModifier);
        PauseManager.ContinueGame();
        PolicyManager.Instance.OpenPolicyCardUI(false);
    }
}
