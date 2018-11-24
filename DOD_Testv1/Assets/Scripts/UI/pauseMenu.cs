using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BayatGames.SaveGameFree;
using PixelCrushers.Wrappers;
using PixelCrushers.DialogueSystem;

public class pauseMenu : MonoBehaviour {
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    Scene CurrentScene;

    private PlayerController player;
    private HealthBar hp;

    public GameObject[] enemy; 

    void Start()
    {
        CurrentScene = SceneManager.GetActiveScene();
        player = FindObjectOfType<PlayerController>();
        hp = FindObjectOfType<HealthBar>();
    }

    void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
	}

    public void ResumeGame()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    void PauseGame()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void ReturnToHub()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("LevelSelect");
        SceneManager.LoadScene("Pause_Menu", LoadSceneMode.Additive);
    }

    public void ResetLevel()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        SaveSystem.DeleteSavedGameInSlot(0);
        PersistentDataManager.Reset();
        SceneManager.LoadScene("Dungeon_V1");
        SceneManager.LoadScene("Pause_Menu", LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        string sceneName = CurrentScene.name;
        if(sceneName == "Dungeon_V1")
            SaveSystem.SaveToSlot(0);

        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.UnloadSceneAsync("Pause_Menu");
        SceneManager.LoadScene("Game_Intro");


    }
}
