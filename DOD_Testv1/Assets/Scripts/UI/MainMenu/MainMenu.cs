using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrushers.Wrappers;
using PixelCrushers.DialogueSystem;

public class MainMenu : MonoBehaviour
{

    private PlayerController player;


    // Use this for initialization
    public void PlayGame()
    {
        SaveSystem.DeleteSavedGameInSlot(0);
        PersistentDataManager.Reset();

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("LevelSelect");
    }

    public void ContinueGame()
    {
        SaveSystem.LoadFromSlot(0);
        SceneManager.LoadScene("Pause_Menu", LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
        SceneManager.LoadScene("Game_Intro");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    private void OnGUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
