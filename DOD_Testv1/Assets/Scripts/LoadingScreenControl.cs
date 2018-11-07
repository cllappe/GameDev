using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenControl : MonoBehaviour
{

    public GameObject loadingScreenObject;
    public Slider slider;

    private AsyncOperation async;

    public void LoadScreen(int loadIndex)
    {
        StartCoroutine(LoadingScreen(loadIndex));
    }

    IEnumerator LoadingScreen(int sceneIndex)
    {
        loadingScreenObject.SetActive(true);
        async = SceneManager.LoadSceneAsync(sceneIndex);
        async.allowSceneActivation = false;

        while (async.isDone == false)
        {
            slider.value = async.progress;
            if (async.progress == 0.9f)
            {
                slider.value = 1f;
                async.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
