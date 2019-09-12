using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public Slider slider;
    public GameObject loadingMenu;
    public Animator animator;

    private bool _playedAnimation = false;

    public void IniciarButton()
    {
        StartCoroutine(LoadAsynchronously(1));
    }

    public void SairButton()
    {
        Application.Quit();
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        loadingMenu.SetActive(true);

        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex);
        operation.completed += Operation_completed;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progress;

            if (progress == 1 && !_playedAnimation)
            {
                animator.SetTrigger("FadeOut");
                _playedAnimation = true;
            }

            yield return null;
        }
    }

    private void Operation_completed(AsyncOperation obj)
    {
        animator.SetTrigger("FadeIn");
    }
}