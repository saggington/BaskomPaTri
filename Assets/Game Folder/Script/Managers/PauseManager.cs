using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
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
    public static void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public static void ContinueGame()
    {
        Time.timeScale = 1f;
    }
}
