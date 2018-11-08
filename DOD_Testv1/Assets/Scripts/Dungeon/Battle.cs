using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrushers.Wrappers;

public class Battle : MonoBehaviour {

    public string level;
    private DestructibleSaver ds;
    //private LoadingScreenBattle loadingScreen;
    //public GameObject lscontroller;


    private void Awake()
    {
 
        ds = this.gameObject.GetComponent<DestructibleSaver>();
       // loadingScreen = lscontroller.GetComponent<LoadingScreenBattle>();
    }

    // Use this for initialization
    void OnTriggerEnter2D(Collider2D Colider)
    {
        if (Colider.gameObject.name == "Player")
        {
            PlayerPrefs.SetString("Enemy", gameObject.name);
            //gameObject.SetActive(false);
            ds._isDestroyed = true;
            Destroy(gameObject);
            SaveSystem.SaveToSlot(1);

            //loadingScreen.LoadScreen(level);
            SceneManager.LoadScene(level);

            SceneManager.LoadScene("Pause_Menu", LoadSceneMode.Additive);
        }
        
    }
}
