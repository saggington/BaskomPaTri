using UnityEngine;

public class PauseManager : MonoBehaviour
{
    
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
    }
}
