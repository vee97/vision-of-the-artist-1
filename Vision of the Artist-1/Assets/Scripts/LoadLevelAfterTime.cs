using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelAfterTime : MonoBehaviour
{
    [SerializeField]
    private float delayBeforeLoading = 10f;

    //[SerializeField]
    //private string sceneNameToLoad;

    private float timeElapsed;

    public Animator transition;
    public float transitionTime = 1f;

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > delayBeforeLoading)
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        // Play animation
        transition.SetTrigger("Start");

        // wait for animation to stop playing
        yield return new WaitForSeconds(transitionTime);

        // Load scene
        SceneManager.LoadScene(levelIndex);
    }
}
