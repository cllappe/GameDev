﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {

    public Animator animator;

    private int levelToLoad;
	
	// Update is called once per frame


    public void FadeToNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex+1);

    }

    public void FadeToLevel (int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
        
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
        SceneManager.LoadScene("Pause_Menu", LoadSceneMode.Additive);
    }
}
