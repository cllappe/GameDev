using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public Character player;
    private Character playerCopy;
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

    public List<GameObject> dropZones;

    public Slider playerHealthBar;
    public Slider enemy1HealthBar;
    public Slider enemy2HealthBar;
    public Slider enemy3HealthBar;

    public List<Slider> healthBarList;
    public List<GameObject> enemyUTDs;

    public int turnCount;
    public bool charRemoved;


    private void Start()
    {
        enemySpawn.Add(enemy1Spawn);
        enemySpawn.Add(enemy2Spawn);
        enemySpawn.Add(enemy3Spawn);
        dropZones.Add(GameObject.Find("Enemy1DropZone"));
        dropZones.Add(GameObject.Find("Enemy2DropZone"));
        dropZones.Add(GameObject.Find("Enemy3DropZone"));
        enemyUTDs.Add(GameObject.Find("Enemy1UTD"));
        enemyUTDs.Add(GameObject.Find("Enemy2UTD"));
        enemyUTDs.Add(GameObject.Find("Enemy3UTD"));
        for (int i = 0; i < 3; i++)
        {
            dropZones[i].SetActive(false);
            enemyUTDs[i].SetActive(false);
        }

        numberOfEnemies = UnityEngine.Random.Range(1, 4);
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Character enemyCopy = Instantiate(enemyArray.enemies[enemyId]);
            enemyList.Add(enemyCopy);
            EnemySetup(enemySpawn[i], i);
            dropZones[i].SetActive(true);
            enemyUTDs[i].SetActive(true);
        }

        playerCopy = Instantiate(player);
        PlayerSetup();
    }

    private void EnemySetup(Transform enemySpawnPoint, int iteration)
    {
        GameObject go1 = Instantiate(avatarPrefab,
            new Vector3(enemySpawnPoint.position.x, enemySpawnPoint.position.y, -1.0f), Quaternion.identity);
        go1.name = "Enemy " + (iteration + 1);
        CharacterDisplay display1 = go1.GetComponent<CharacterDisplay>();
        CharacterStateMachine csm1 = go1.GetComponent<CharacterStateMachine>();
        display1.characterSetup(enemyList.First());
        csm1.csm_Set(enemyList[iteration]);
        go1.tag = "Enemy";
        go1.transform.Rotate(Vector3.up * 180);
        if (iteration == 0)
        {
            display1.healthBarGO = go1.transform.GetChild(0).GetChild(1).gameObject;
            enemy1HealthBar = display1.healthBarGO.GetComponent<Slider>();
            enemy1HealthBar.maxValue = display1.health;
            healthBarList.Add(enemy1HealthBar);
        }
        else if (iteration == 1)
        {
            display1.healthBarGO = go1.transform.GetChild(0).GetChild(1).gameObject;
            enemy2HealthBar = display1.healthBarGO.GetComponent<Slider>();
            enemy2HealthBar.maxValue = display1.health;
            healthBarList.Add(enemy2HealthBar);
        }
        else if (iteration == 2)
        {
            display1.healthBarGO = go1.transform.GetChild(0).GetChild(1).gameObject;
            enemy3HealthBar = display1.healthBarGO.GetComponent<Slider>();
            enemy3HealthBar.maxValue = display1.health;
            healthBarList.Add(enemy3HealthBar);
        }
    }

    private void PlayerSetup()
    {
        GameObject go = Instantiate(avatarPrefab,
            new Vector3(playerSpawn.position.x, playerSpawn.position.y, -1.0f),
            Quaternion.identity);
        go.name = "Player";
        CharacterDisplay display = go.GetComponent<CharacterDisplay>();
        CharacterStateMachine csm = go.GetComponent<CharacterStateMachine>();
        display.characterSetup(playerCopy);
        csm.character = playerCopy;
        display.healthBarGO = go.transform.GetChild(0).GetChild(1).gameObject;
        playerHealthBar = display.healthBarGO.GetComponent<Slider>();
        playerHealthBar.maxValue = display.health;
    }

    private void Update()
    {
        UpdateHealthBars();
        UpdatePlayerHealthBar();
    }

    public void UpdatePlayerHealthBar()
    {
        playerHealthBar.value = playerCopy.health;
    }

    public void UpdateHealthBars()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            healthBarList[i].value = enemyList[i].health;
        }
    }
}
