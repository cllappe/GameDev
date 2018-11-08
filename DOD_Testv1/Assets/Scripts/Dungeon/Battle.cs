using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrushers.Wrappers;

public class Battle : MonoBehaviour {

    public string level;
    private DestructibleSaver ds;

    private void Awake()
    {
 
        ds = this.gameObject.GetComponent<DestructibleSaver>();
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


          
            SceneManager.LoadSceneAsync(level);

            SceneManager.LoadScene("Pause_Menu", LoadSceneMode.Additive);
        }
        
    }
}
