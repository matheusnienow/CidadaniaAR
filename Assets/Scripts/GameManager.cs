using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject endPanel;
    public Camera cam;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        RegisterOnPlayer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RegisterOnPlayer()
    {
        var script = player.GetComponent<PlayerMovementController>();
        PlayerMovementController.onDestinationReached += OnLevelCompleted;
    }

    private void OnLevelCompleted()
    {
        endPanel.SetActive(true);
        cam.GetComponent<CameraScript>().enabled = false;
        text.text = "LEVEL COMPLETED";
    }
}
