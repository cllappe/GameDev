using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = System.Object;

public class LevelManager : MonoBehaviour {

    public Character player;
    public List<Character> enemyList;

    public Enemies enemyArray;
    public static int enemyId = 0;
    public static int numberOfEnemies;

    public GameObject avatarPrefab;

    public Transform playerSpawn;
    public Transform enemy1Spawn;
    public Transform enemy2Spawn;
    public Transform enemy3Spawn;

    public List<Transform> enemySpawn;

    private void Start()
    {
        enemySpawn.Add(enemy1Spawn);
        enemySpawn.Add(enemy2Spawn);
        enemySpawn.Add(enemy3Spawn);
        numberOfEnemies = UnityEngine.Random.Range(1,4);
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Character enemyCopy = Instantiate(enemyArray.enemies[enemyId]);
            enemyList.Add(enemyCopy);
            EnemySetup(enemySpawn[i]);
        }
        PlayerSetup();
    }

    private void EnemySetup(Transform enemySpawnPoint)
    {
        GameObject go1 = Instantiate(avatarPrefab,
            new Vector3(enemySpawnPoint.position.x, enemySpawnPoint.position.y, -1.0f), Quaternion.identity);
        CharacterDisplay display1 = go1.GetComponent<CharacterDisplay>();
        CharacterStateMachine csm1 = go1.GetComponent<CharacterStateMachine>();
        display1.characterSetup(enemyList.First());
        csm1.csm_Set(enemyList[0]);
        enemyList.RemoveAt(0);
    }

    private void PlayerSetup()
    {
        GameObject go = Instantiate(avatarPrefab, new Vector3(playerSpawn.position.x, playerSpawn.position.y, -1.0f),
            Quaternion.identity);
        CharacterDisplay display = go.GetComponent<CharacterDisplay>();
        CharacterStateMachine csm = go.GetComponent<CharacterStateMachine>();
        display.characterSetup(player);
        csm.character = player;


    }
}
