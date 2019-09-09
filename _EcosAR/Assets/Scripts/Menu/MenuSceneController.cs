using UnityEngine;

public class MenuSceneController : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this);

        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
}