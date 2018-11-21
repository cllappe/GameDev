using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class LevelManager : MonoBehaviour
{

    public Character player;
    private Character playerCopy;
    public List<Character> enemyList;
    

    public List<Character> enemyArray;
    public static int enemyId;
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
    
    public int aggressionPowerUpEnd = 0;
    public int luckyPowerUpEnd = 0;
    public int defPowerUpEnd = 0;
    public bool aggressionPowerUpOn;
    public bool luckyPowerUpOn;
    public bool defPowerUpOn;


    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        string sceneName = currentScene.name;
        if (sceneName == "BattleScene")
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
                enemyId = UnityEngine.Random.Range(0, enemyArray.Count);
                Debug.Log(enemyId);
                Character enemyCopy = Instantiate(enemyArray[enemyId]);
                enemyList.Add(enemyCopy);
                EnemySetup(enemySpawn[i], i);
                dropZones[i].SetActive(true);
                enemyUTDs[i].SetActive(true);
            }

            playerCopy = Instantiate(player);
            PlayerSetup();
        }
        else if (sceneName == "MiniBossBattle")
        {
            numberOfEnemies = 1;
            Debug.Log("MiniBossBattle");
            enemySpawn.Add(enemy1Spawn);
            dropZones.Add(GameObject.Find("MiniBossDropZone"));
            enemyUTDs.Add(GameObject.Find("MiniBossUTD"));
            enemyId = UnityEngine.Random.Range(0, enemyArray.Count);
            Debug.Log(enemyId);
            Character enemyCopy = Instantiate(enemyArray[enemyId]);
            enemyList.Add(enemyCopy);
            EnemySetup(enemySpawn[0], 0);
            dropZones[0].SetActive(true);
            enemyUTDs[0].SetActive(true);
            playerCopy = Instantiate(player);
            PlayerSetup();
        }
        else if (sceneName == "BossBattle")
        {
            numberOfEnemies = 1;
            Debug.Log("BossBattle");
            enemySpawn.Add(enemy1Spawn);
            dropZones.Add(GameObject.Find("BossDropZone"));
            enemyUTDs.Add(GameObject.Find("BossUTD"));
            enemyId = UnityEngine.Random.Range(0, enemyArray.Count);
            Debug.Log(enemyId);
            Character enemyCopy = Instantiate(enemyArray[enemyId]);
            enemyList.Add(enemyCopy);
            EnemySetup(enemySpawn[0], 0);
            dropZones[0].SetActive(true);
            enemyUTDs[0].SetActive(true);
            playerCopy = Instantiate(player);
            PlayerSetup();
        }
        
    }

    private void EnemySetup(Transform enemySpawnPoint, int iteration)
    {
        GameObject go1 = Instantiate(avatarPrefab,
            new Vector3(enemySpawnPoint.position.x, enemySpawnPoint.position.y, -1.0f), Quaternion.identity);
        go1.name = "Enemy " + (iteration + 1);
        CharacterDisplay display1 = go1.GetComponent<CharacterDisplay>();
        CharacterStateMachine csm1 = go1.GetComponent<CharacterStateMachine>();
        display1.characterSetup(enemyList[iteration]);
        csm1.csm_Set(enemyList[iteration]);
        go1.tag = "Enemy";
        go1.transform.Rotate(Vector3.up * 180);
        Scene currentScene = SceneManager.GetActiveScene();

        string sceneName = currentScene.name;
        if (sceneName == "MiniBossBattle")
        {
            go1.transform.localScale += new Vector3(.5f, .5f, 0);
        }
        else if (sceneName == "BossBattle")
        {
            go1.transform.localScale += new Vector3(1.25f, 1.25f, 0);
        }
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
