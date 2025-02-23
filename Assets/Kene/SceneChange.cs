using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public Animator animator;

    private int levelIndexToLoad;

    public void FadeToLevel(int levelIndex) 
    {
        levelIndexToLoad = levelIndex;
        animator.SetTrigger("fade");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelIndexToLoad);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
