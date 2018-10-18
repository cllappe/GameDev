using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Battle : MonoBehaviour {

    public string level;

    // Use this for initialization
    void OnTriggerEnter2D(Collider2D Colider)
    {
        if (Colider.gameObject.name == "Player")
            SceneManager.LoadScene("BattleScene");
    }
}
