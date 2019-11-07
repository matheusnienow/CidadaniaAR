using UnityEngine;
using UnityEngine.UI;

public static class GameManager
{
    public static void OnLevelCompleted(GameObject endPanel)
    {
        if (endPanel != null) endPanel.SetActive(true);
    }
}
