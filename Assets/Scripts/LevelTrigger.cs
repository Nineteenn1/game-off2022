using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour
{
    public Animator levelTransition;

    public float levelTransitionTime = 1.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LoadNextLevel();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene()
            .buildIndex + 1));
    }

    IEnumerator LoadLevel(int level)
    {
        levelTransition.SetTrigger("Start");

        yield return new WaitForSeconds(levelTransitionTime);

        SceneManager.LoadScene(level);
    }
}
