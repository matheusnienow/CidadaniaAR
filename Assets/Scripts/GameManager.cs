using UnityEngine;
using UnityEngine.UI;

public static class GameManager
{
    public static void OnLevelCompleted(GameObject endPanel)
    {
        endPanel.SetActive(true);
        endPanel.GetComponentInChildren<Text>().text = "NÍVEL FINALIZADO!";
    }
}
