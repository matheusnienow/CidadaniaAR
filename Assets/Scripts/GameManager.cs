using UnityEngine;
using UnityEngine.UI;

public static class GameManager
{
    public static void OnLevelCompleted(GameObject endPanel, GameObject ui)
    {
        if (endPanel != null) endPanel.SetActive(true);
        if (ui != null) ui.SetActive(false);
    }
}
