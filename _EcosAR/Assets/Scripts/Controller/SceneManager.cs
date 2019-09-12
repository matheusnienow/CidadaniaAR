using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private Vector3 _acceleration;

    void Update()
    {
        ResetSceneIfShaken();
    }

    void ResetSceneIfShaken() {
        _acceleration = Input.acceleration;

        if (_acceleration.sqrMagnitude > 5f) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}