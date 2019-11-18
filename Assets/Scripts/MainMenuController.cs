using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject aboutMenu;
    
    public void LoadAboutPanel()
    {
        aboutMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    
    public void ExitAboutPanel()
    {
        aboutMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
