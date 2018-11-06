using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawn : MonoBehaviour {

    public GameObject enemy;

	// Use this for initialization
	void Awake () {
        
        //if (PlayerPrefs.GetInt("lastlevel") == 7 || PlayerPrefs.GetInt("lastlevel") == 8 || PlayerPrefs.GetInt("lastlevel") == 9)
        if (PlayerPrefs.GetString("lastlevel") == "battle" && PlayerPrefs.GetString("Enemy") == enemy.name)
        {
            PlayerPrefs.DeleteKey("Enemy");
            foreach(Transform chest in transform)
            {
                chest.gameObject.SetActive(true);
            }
        }
	}
}
