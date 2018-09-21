using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public Character player;
    public Character enemy1;
    public Character enemy2;
    public Character enemy3;

    public GameObject avatarPrefab;

    public Transform playerSpawn;
    public Transform enemy1Spawn;
    public Transform enemy2Spawn;
    public Transform enemy3Spawn;

    private void Start()
    {
        PlayerSetup();
        EnemySetup();
    }

    private void EnemySetup()
    {
        GameObject go1 = Instantiate(avatarPrefab, new Vector3(enemy1Spawn.position.x, enemy1Spawn.position.y, -1.0f), Quaternion.identity);
        CharacterDisplay display1 = go1.GetComponent<CharacterDisplay>();
        display1.characterSetup(enemy1);

        GameObject go2 = Instantiate(avatarPrefab, new Vector3(enemy2Spawn.position.x, enemy2Spawn.position.y, -1.0f), Quaternion.identity);
        CharacterDisplay display2 = go2.GetComponent<CharacterDisplay>();
        display2.characterSetup(enemy2);

        GameObject go3 = Instantiate(avatarPrefab, new Vector3(enemy3Spawn.position.x, enemy3Spawn.position.y, -1.0f), Quaternion.identity);
        CharacterDisplay display3 = go3.GetComponent<CharacterDisplay>();
        display3.characterSetup(enemy3);
    }

    private void PlayerSetup()
    {
        GameObject go = Instantiate(avatarPrefab, new Vector3(playerSpawn.position.x, playerSpawn.position.y, -1.0f), Quaternion.identity);
        CharacterDisplay display = go.GetComponent<CharacterDisplay>();
        display.characterSetup(player);
    }
}
