using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BayatGames.SaveGameFree;
using PixelCrushers.Wrappers;
using PixelCrushers.DialogueSystem;

public class gameOver : MonoBehaviour {

    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    Scene CurrentScene;

	// Use this for initialization
	void Start () {

        CurrentScene = SceneManager.GetActiveScene();
		
	}
	
    public void ResetLevel()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        SaveSystem.DeleteSavedGameInSlot(0);
        SaveSystem.DeleteSavedGameInSlot(1);
        PersistentDataManager.Reset();
        SceneManager.LoadScene("Dungeon_V1");
        SceneManager.LoadScene("Pause_Menu", LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        string sceneName = CurrentScene.name;

        if (sceneName == "Dungeon_V1")
            SaveSystem.SaveToSlot(0);

        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.UnloadSceneAsync("Pause_Menu");
        SceneManager.LoadScene("Game_Intro");


    }


}
