﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Guirao.UltimateTextDamage;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using PixelCrushers.Wrappers;
using Random = System.Random;

public class CharacterStateMachine : MonoBehaviour
{

	public Character character;
	public int actionsLeft = 0;
	private Character player;
	private GameObject playerGO;
	private Animator turnAnimation;

	private bool enemyDmgEnabled;
	private bool dyingState;
	private bool isDead;
	private bool autoHeal;
	
	private bool justExitedWaiting;
	public enum TurnState
	{
		BEGINING,
		ACTION,
		ADDTOLIST,
		WAITING,
		DEAD,
		VICTORY,
		DYING,
		DEFEAT
	}

	public TurnState currentState;

	// Use this for initialization
	void Start ()
	{
        //Setting Battle as the LAST LEVEL for Save 
        //PlayerPrefs.SetInt("lastLevel", SceneManager.GetActiveScene().buildIndex);
        //PlayerPrefs.SetString("lastLevel", SceneManager.GetActiveScene().name);

        if (character.charType == Character.Type.PLAYER)
		{
			turnAnimation = GameObject.Find("PlayerTurn").GetComponent<Animator>();
		}
		else if (character.charType == Character.Type.ENEMY || character.charType == Character.Type.MINIBOSS || character.charType == Character.Type.BOSS)
		{
			turnAnimation = GameObject.Find("EnemyTurn").GetComponent<Animator>();
			playerGO = GameObject.Find("Player");
			player = playerGO.GetComponent<CharacterStateMachine>().character;
            //Getting Health from Dungeon 
            player.health = DialogueLua.GetActorField("Player", "health").asInt;
		}
		currentState = TurnState.BEGINING;
		Dragable.actionsIncreased = false;
	}
	
	// Update is called once per frame
	private void Update ()
	{
		switch (currentState)
		{
			case (TurnState.BEGINING):
			{
				string CurrentScene = SceneManager.GetActiveScene().name;
				if (Character.Type.PLAYER == character.charType && CurrentScene == "BossBattle")
				{
					autoHeal = true;
				}
				dyingState = false;
				isDead = false;
				turnMonitor();
				break;
			}
			case (TurnState.ACTION):
			{
				if (justExitedWaiting)
				{
					turnAnimation.Play("turnAnimation");
					justExitedWaiting = false;
				}
				if (actionsLeft == 0 && !CardBattleManager.enemyTurn)
				{
					actionsLeft = 1;
				}

				if (character.charType == Character.Type.PLAYER)
				{
					if (autoHeal && character.health <= 20)
					{
						character.health = 100;
						GameObject.Find("Combat Log").GetComponent<CombatLogPopulate>().populate("Auto Heal has been triggered! Use your second chance wisely!.", false);
						autoHeal = false;
					}
					//Debug.Log("Current Player Health " + character.health);
					//Debug.Log("Current Luck " + character.luck);

					Dragable.playerTurn = true;
					if (Dragable.validDrop)
					{
						GameObject.Find("LevelManager").GetComponent<LevelManager>().UpdateHealthBars();
						actionsLeft--;
						if (actionsLeft == 0 && !Dragable.actionsIncreased)
						{
							Dragable.playerTurn = false;
							CardBattleManager.draw1card = true;
							GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount++;
							//Debug.Log(GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount);
						}
						else if (GameObject.Find("Hand").transform.childCount == 0)
						{
							actionsLeft = 0;
							Dragable.playerTurn = false;
							CardBattleManager.draw1card = true;
							GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount++;
							GameObject CombatLog = GameObject.Find("Combat Log");
							CombatLog.GetComponent<CombatLogPopulate>().populate("You ran out of cards. Unfortunate.", false);
						}
						//Debug.Log("CSM " + actionsLeft);
						Dragable.validDrop = false;
					}
				}
				else
				{
					if (enemyDmgEnabled)
					{
						Scene currentScene = SceneManager.GetActiveScene();

						string sceneName = currentScene.name;
						if (sceneName == "MiniBossBattle")
						{
							int miniAttackVar = UnityEngine.Random.Range(0, 100);
							if (miniAttackVar <= character.specialPrecent1)
							{
								CardBattleManager.enemyTurn = true;
								Invoke("miniBossSpecialAttack",2);
								enemyDmgEnabled = false;
							}
							else
							{
								CardBattleManager.enemyTurn = true;
								Invoke("enemyAttack", 2);
								enemyDmgEnabled = false;
							}
						}
						else if (sceneName == "BossBattle")
						{
							int bossAttackVar = UnityEngine.Random.Range(0, 100);
							if (bossAttackVar <= character.specialMod1)
							{
								CardBattleManager.enemyTurn = true;
								Invoke("bossSpecialAttack1",2);
								enemyDmgEnabled = false;
							}
							else if (bossAttackVar <= character.specialMod1 + character.specialMod2 && bossAttackVar > character.specialMod1)
							{
								CardBattleManager.enemyTurn = true;
								Invoke("bossSpecialAttack2",2);
								enemyDmgEnabled = false;
							}
							else
							{
								CardBattleManager.enemyTurn = true;
								Invoke("enemyAttack", 2);
								enemyDmgEnabled = false;
							}
						}
						else
						{
							CardBattleManager.enemyTurn = true;
							Invoke("enemyAttack", 2);
							enemyDmgEnabled = false;	
						}
					}
				}	
				turnMonitor();
				break;
			}
			case (TurnState.ADDTOLIST):
			{
				if (character.charType == Character.Type.PLAYER && GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount == 0)
				{
					GameObject.Find("GameManager").GetComponent<CardBattleManager>().charOrder.Insert(0,character);
				}
				else
				{
					GameObject.Find("GameManager").GetComponent<CardBattleManager>().charOrder.Add(character);
					character.skipTurn = false;
				}
				currentState = TurnState.WAITING;
				break;
			}
			case (TurnState.DYING):
			{
				if (character.charType == Character.Type.ENEMY || character.charType == Character.Type.MINIBOSS ||
				    character.charType == Character.Type.BOSS)
				{
					//CardBattleManager.deadEnemies++;
					deathCount();
					dyingState = true;
					if (this.name == "Enemy 1")
					{
						StartCoroutine(DeathAnimate(1));
					}
					else if (name == "Enemy 2")
					{
						StartCoroutine(DeathAnimate(2));
					}
					else if (name == "Enemy 3")
					{
						StartCoroutine(DeathAnimate(3));
					}					
					else if (name == "Player")
					{
						StartCoroutine(DeathAnimate(4));
					}
					//Debug.Log(CardBattleManager.deadEnemies);
					currentState = TurnState.DEAD;
					//Debug.Log(CardBattleManager.deadEnemies);
				}
				else
				{
					currentState = TurnState.DEFEAT;
				}
				break;
			}
			case (TurnState.DEAD):
			{
				Debug.Log(name);
				if (!isDead)
				{
					if (name == "Enemy 1")
					{
						Scene currentScene = SceneManager.GetActiveScene();

						string sceneName = currentScene.name;
						if (sceneName == "BattleScene")
						{
							GameObject.Find("Enemy1DropZone").SetActive(false);
							GameObject.Find("Enemy1UTD").SetActive(false);	
						}
						else if (sceneName == "MiniBossBattle")
						{
							GameObject.Find("MiniBossDropZone").SetActive(false);
							GameObject.Find("MiniBossUTD").SetActive(false);
						}
					}
					else if (name == "Enemy 2")
					{
						GameObject.Find("Enemy2DropZone").SetActive(false);
						GameObject.Find("Enemy2UTD").SetActive(false);
					}
					else if (name == "Enemy 3")
					{
						GameObject.Find("Enemy3DropZone").SetActive(false);
						GameObject.Find("Enemy3UTD").SetActive(false);
					}
					GameObject.Find("GameManager").GetComponent<CardBattleManager>().charOrder.Remove(character);
					GameObject.Find(name).SetActive(false);
					isDead = true;
				}
				break;
			}
			case (TurnState.WAITING):
			{					
				if (GameObject.Find("GameManager").GetComponent<CardBattleManager>().charOrder.Count > 1 && !CardBattleManager.enemyTurn && !Dragable.playerTurn)
				{
					if (character == GameObject.Find("GameManager").GetComponent<CardBattleManager>().charOrder.First())
					{
						if ((character.charType == Character.Type.ENEMY ||character.charType == Character.Type.MINIBOSS || character.charType == Character.Type.BOSS )&& GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount != 0)
						{
							if (character.skipTurn)
							{
								currentState = TurnState.ADDTOLIST;
								GameObject.Find("GameManager").GetComponent<CardBattleManager>().charOrder.Remove(character);
								GameObject.Find("LevelManager").GetComponent<LevelManager>().charRemoved = false;
							}
							else
							{
								enemyDmgEnabled = true;
							
								if (!GameObject.Find("LevelManager").GetComponent<LevelManager>().charRemoved)
								{
									currentState = TurnState.ACTION;
									justExitedWaiting = true;
									GameObject.Find("GameManager").GetComponent<CardBattleManager>().charOrder.Remove(character);
									GameObject.Find("LevelManager").GetComponent<LevelManager>().charRemoved = true;
								}	
							}
							
						}

						if (!GameObject.Find("LevelManager").GetComponent<LevelManager>().charRemoved)
						{
							if (character.skipTurn)
							{
								currentState = TurnState.ADDTOLIST;
								GameObject.Find("GameManager").GetComponent<CardBattleManager>().charOrder.Remove(character);
								GameObject.Find("LevelManager").GetComponent<LevelManager>().charRemoved = false;
							}
							else
							{
								currentState = TurnState.ACTION;
								justExitedWaiting = true;
								GameObject.Find("GameManager").GetComponent<CardBattleManager>().charOrder.Remove(character);
								GameObject.Find("LevelManager").GetComponent<LevelManager>().charRemoved = true;
							}	
						}
					}
				}
				else
				{
					GameObject.Find("LevelManager").GetComponent<LevelManager>().charRemoved = false;
					turnMonitor();
				}
				break;
			}
			case (TurnState.VICTORY):
			{
				GameObject.Find("TurnAnimations").transform.GetChild(2).gameObject.SetActive(true);
				GameObject.Find("VICTORY").GetComponent<Animator>().Play("Victory");
				
                //Saved HP after Winning Battle                  
				PlayerPrefs.SetInt("battlehp", character.health);
                PlayerPrefs.SetString("lastlevel", "battle");

                //Win Animation                  
				GameObject go = GameObject.Find("Animation Master");                 
				go.transform.GetChild(13).gameObject.SetActive(true);                 
				enabled = false;

                //Reset State Machine                          
				LevelManager.numberOfEnemies = 0;                 
				CardBattleManager.deadEnemies = 0;
                 // Loading player position in dungeon before battle & Menu
				Invoke("toDungeon",3);
                 break; 
			}
			case (TurnState.DEFEAT):
			{
				GameObject.Find("TurnAnimations").transform.GetChild(3).gameObject.SetActive(true);
				GameObject.Find("DEFEAT").GetComponent<Animator>().Play("Defeat");
				GameObject go = GameObject.Find("Animation Master");
				go.transform.GetChild(10).SetPositionAndRotation(new Vector3(-5, 0, 4), Quaternion.identity);
				go.transform.GetChild(10).gameObject.SetActive(true);
                
                Time.timeScale = 1f;
                SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);

				enabled = false;
				break;
			}
		}
	}
	public void turnMonitor()
	{
		if (character.health <= 0)
		{
			currentState = TurnState.DYING;
		}
		else if (CardBattleManager.deadEnemies == LevelManager.numberOfEnemies && character.charType == Character.Type.PLAYER)
		{
			currentState = TurnState.VICTORY;
		}
		else if (currentState == TurnState.WAITING)
		{
			currentState = TurnState.WAITING;
		}
		else if ((actionsLeft == 0 && character.health > 0 && currentState == TurnState.ACTION) || currentState == TurnState.BEGINING)
		{
			currentState = TurnState.ADDTOLIST;
			//Debug.Log("Added " + character.charType + " to list.");
		}
	}

	public void csm_Set(Character thisCharacter)
	{
		character = thisCharacter;
	}


	
	public void enemyAttack()
	{
		//Debug.Log(character.name + " " + actionsLeft);
		if (character.dmgReflected)
		{
			//Debug.Log("Damage Should Be Reflected");
			character.health -= character.basicAttackDmg;
			character.dmgReflected = false;
			GameObject.Find("LevelManager").GetComponent<LevelManager>().UpdateHealthBars();
			if (name == "Enemy 1")
			{
				Scene currentScene = SceneManager.GetActiveScene();

				string sceneName = currentScene.name;
				if (sceneName == "MiniBossBattle")
				{
					GameObject UTD = GameObject.Find("MiniBossUTD");
					UTD.GetComponent<UltimateTextDamageManager>().Add(character.basicAttackDmg.ToString(), UTD.transform, "damage");
					StartCoroutine(EnemyDmgAnimate(2));
					StartCoroutine(AttackFX(2));
				}
				else if (sceneName == "BossBattle")
				{
					GameObject UTD = GameObject.Find("BossUTD");
					UTD.GetComponent<UltimateTextDamageManager>().Add(character.basicAttackDmg.ToString(), UTD.transform, "damage");
					StartCoroutine(EnemyDmgAnimate(2));
					StartCoroutine(AttackFX(2));
				}
				else
				{
					GameObject UTD = GameObject.Find("Enemy1UTD");
					UTD.GetComponent<UltimateTextDamageManager>().Add(character.basicAttackDmg.ToString(), UTD.transform, "damage");
					StartCoroutine(EnemyDmgAnimate(1));
					StartCoroutine(AttackFX(1));
				}
			}
			else if (name == "Enemy 2")
			{
				GameObject UTD = GameObject.Find("Enemy2UTD");
				UTD.GetComponent<UltimateTextDamageManager>().Add(character.basicAttackDmg.ToString(), UTD.transform, "damage");
				StartCoroutine(EnemyDmgAnimate(2));
				StartCoroutine(AttackFX(2));
			}
			else if (name == "Enemy 3")
			{
				GameObject UTD = GameObject.Find("Enemy3UTD");
				UTD.GetComponent<UltimateTextDamageManager>().Add(character.basicAttackDmg.ToString(), UTD.transform, "damage");
				StartCoroutine(EnemyDmgAnimate(3));
				StartCoroutine(AttackFX(3));
			}
		}
		else
		{
			GameObject PlayerUTD = GameObject.Find("PlayerUTD");
			int dmg =  Mathf.RoundToInt(character.basicAttackDmg * player.defence);
			player.health -= dmg;
			PlayerUTD.GetComponent<UltimateTextDamageManager>().Add(dmg.ToString(), PlayerUTD.transform, "damage");
			GameObject.Find("LevelManager").GetComponent<LevelManager>().UpdatePlayerHealthBar();
			if (name == "Enemy 1")
			{
				Scene currentScene = SceneManager.GetActiveScene();

				string sceneName = currentScene.name;
				if (sceneName == "MiniBossBattle" || sceneName == "BossBattle")
				{
					StartCoroutine(AttackFX(2));
				}
				else
				{
					StartCoroutine(AttackFX(1));	
				}
			}
			else if (name == "Enemy 2")
			{
				StartCoroutine(AttackFX(2));
			}
			else if (name == "Enemy 3")
			{
				StartCoroutine(AttackFX(3));
			}
			StartCoroutine(EnemyDmgAnimate(4));
		}
		//Debug.Log("Did Damage to Player");
		actionsLeft--;
		//Debug.Log(character.name + " " + actionsLeft);
		CardBattleManager.enemyTurn = false;
		//enemyDmgEnabled = false;
		turnMonitor();
	}

	public void deathCount()
	{
		if (!dyingState)
		{
			CardBattleManager.deadEnemies++;
			//Debug.Log("In deathcount function with dead enemies = " + CardBattleManager.deadEnemies);
		}
	}

	IEnumerator EnemyDmgAnimate(int tarNum)
	{
		GameObject go = GameObject.Find("Animation Master");
		if (tarNum == 1)
		{
			go.transform.GetChild(5).SetPositionAndRotation(new Vector3(1, 0, 4), Quaternion.identity);	
			go.transform.GetChild(5).gameObject.SetActive(true);
			yield return new WaitForSeconds(2);
			go.transform.GetChild(5).gameObject.SetActive(false);
		}
		else if (tarNum == 2)
		{
			go.transform.GetChild(5).SetPositionAndRotation(new Vector3(4, 0.5f , 4), Quaternion.identity);
			go.transform.GetChild(5).gameObject.SetActive(true);
			yield return new WaitForSeconds(2);
			go.transform.GetChild(5).gameObject.SetActive(false);
		}
		else if (tarNum == 3)
		{
			go.transform.GetChild(5).SetPositionAndRotation(new Vector3(7, 0, 4), Quaternion.identity);
			go.transform.GetChild(5).gameObject.SetActive(true);
			yield return new WaitForSeconds(2);
			go.transform.GetChild(5).gameObject.SetActive(false);
		}
		else if (tarNum == 4)
		{
			go.transform.GetChild(5).SetPositionAndRotation(new Vector3(-5, 0, 4), Quaternion.identity);
			go.transform.GetChild(5).gameObject.SetActive(true);
			yield return new WaitForSeconds(2);
			go.transform.GetChild(5).gameObject.SetActive(false);
		}
	}

	IEnumerator AttackFX(int attackingNum)
	{
		GameObject go = GameObject.Find("Animation Master");
		if (attackingNum == 1)
		{
			go.transform.GetChild(15).gameObject.SetActive(true);
			yield return new WaitForSeconds(2);
			go.transform.GetChild(15).gameObject.SetActive(false);
		}
		else if (attackingNum == 2)
		{
			Debug.Log("Enemy Attack 2 should turn on");
			go.transform.GetChild(16).gameObject.SetActive(true);
			yield return new WaitForSeconds(2);
			go.transform.GetChild(16).gameObject.SetActive(false);			
		}
		else if (attackingNum == 3)
		{
			go.transform.GetChild(17).gameObject.SetActive(true);
			yield return new WaitForSeconds(2);
			go.transform.GetChild(17).gameObject.SetActive(false);
		}
	}

	IEnumerator DeathAnimate(int tarNum)
	{
		GameObject go = GameObject.Find("Animation Master");
		if (tarNum == 1)
		{
			go.transform.GetChild(10).SetPositionAndRotation(new Vector3(1, 0, 4), Quaternion.identity);	
			go.transform.GetChild(10).gameObject.SetActive(true);
			yield return new WaitForSeconds(2);
			go.transform.GetChild(10).gameObject.SetActive(false);
		}
		else if (tarNum == 2)
		{
			go.transform.GetChild(10).SetPositionAndRotation(new Vector3(4, 0.5f , 4), Quaternion.identity);
			go.transform.GetChild(10).gameObject.SetActive(true);
			yield return new WaitForSeconds(2);
			go.transform.GetChild(10).gameObject.SetActive(false);
		}
		else if (tarNum == 3)
		{
			go.transform.GetChild(10).SetPositionAndRotation(new Vector3(7, 0, 4), Quaternion.identity);
			go.transform.GetChild(10).gameObject.SetActive(true);
			yield return new WaitForSeconds(2);
			go.transform.GetChild(10).gameObject.SetActive(false);
		}
	}

	public void miniBossSpecialAttack()
	{
		Debug.Log("MiniBossSpecialAttack function called");
		if (character.dmgReflected)
		{
			//Debug.Log("Damage Should Be Reflected");
			character.health -= (int)(character.basicAttackDmg * character.specialMod1);
			character.dmgReflected = false;
			GameObject.Find("LevelManager").GetComponent<LevelManager>().UpdateHealthBars();
			GameObject UTD = GameObject.Find("MiniBossUTD");
			UTD.GetComponent<UltimateTextDamageManager>()
				.Add((character.basicAttackDmg * character.specialMod1).ToString(), UTD.transform, "damage");
			StartCoroutine(EnemyDmgAnimate(2));
			StartCoroutine(miniSpecialAttackFX());
		}
		else
		{
			GameObject PlayerUTD = GameObject.Find("PlayerUTD");
			int dmg =  Mathf.RoundToInt(character.basicAttackDmg * character.specialMod1 * player.defence);
			player.health -= dmg;
			PlayerUTD.GetComponent<UltimateTextDamageManager>().Add(dmg.ToString(), PlayerUTD.transform, "damage");
			GameObject.Find("LevelManager").GetComponent<LevelManager>().UpdatePlayerHealthBar();
			StartCoroutine(miniSpecialAttackFX());
		}
		//Debug.Log("Did Damage to Player");
		actionsLeft--;
		//Debug.Log(character.name + " " + actionsLeft);
		CardBattleManager.enemyTurn = false;
		//enemyDmgEnabled = false;
		turnMonitor();
	}

	public void bossSpecialAttack1()
	{
		Debug.Log("BossSpecialAttack1 function called");
		if (character.dmgReflected)
		{
			//Debug.Log("Damage Should Be Reflected");
			character.health -= (int)(character.basicAttackDmg * character.specialMod1);
			character.dmgReflected = false;
			GameObject.Find("LevelManager").GetComponent<LevelManager>().UpdateHealthBars();
			GameObject UTD = GameObject.Find("BossUTD");
			UTD.GetComponent<UltimateTextDamageManager>()
				.Add((character.basicAttackDmg * character.specialMod1).ToString(), UTD.transform, "damage");
			StartCoroutine(EnemyDmgAnimate(2));
			StartCoroutine(bossSpecialAttackFX1());
		}
		else
		{
			GameObject PlayerUTD = GameObject.Find("PlayerUTD");
			int dmg =  Mathf.RoundToInt(character.basicAttackDmg * character.specialMod1 * player.defence);
			player.health -= dmg;
			PlayerUTD.GetComponent<UltimateTextDamageManager>().Add(dmg.ToString(), PlayerUTD.transform, "damage");
			GameObject.Find("LevelManager").GetComponent<LevelManager>().UpdatePlayerHealthBar();
			StartCoroutine(bossSpecialAttackFX1());
		}
		//Debug.Log("Did Damage to Player");
		actionsLeft--;
		//Debug.Log(character.name + " " + actionsLeft);
		CardBattleManager.enemyTurn = false;
		//enemyDmgEnabled = false;
		turnMonitor();
	}

	public void bossSpecialAttack2()
	{
		Debug.Log("BossSpecialAttack2 function called");
		if (character.dmgReflected)
		{
			//Debug.Log("Damage Should Be Reflected");
			character.health -= (int)(character.basicAttackDmg * character.specialMod2);
			character.dmgReflected = false;
			GameObject.Find("LevelManager").GetComponent<LevelManager>().UpdateHealthBars();
			GameObject UTD = GameObject.Find("BossUTD");
			UTD.GetComponent<UltimateTextDamageManager>()
				.Add((character.basicAttackDmg * character.specialMod2).ToString(), UTD.transform, "damage");
			StartCoroutine(EnemyDmgAnimate(2));
			StartCoroutine(bossSpecialAttackFX2());
		}
		else
		{
			GameObject PlayerUTD = GameObject.Find("PlayerUTD");
			int dmg =  Mathf.RoundToInt(character.basicAttackDmg * character.specialMod2 * player.defence);
			player.health -= dmg;
			PlayerUTD.GetComponent<UltimateTextDamageManager>().Add(dmg.ToString(), PlayerUTD.transform, "damage");
			GameObject.Find("LevelManager").GetComponent<LevelManager>().UpdatePlayerHealthBar();
			StartCoroutine(bossSpecialAttackFX2());
		}
		//Debug.Log("Did Damage to Player");
		actionsLeft--;
		//Debug.Log(character.name + " " + actionsLeft);
		CardBattleManager.enemyTurn = false;
		//enemyDmgEnabled = false;
		turnMonitor();
	}

	IEnumerator miniSpecialAttackFX()
	{
		GameObject go = GameObject.Find("Animation Master");
		go.transform.GetChild(18).gameObject.SetActive(true);
		yield return new WaitForSeconds(2);
		go.transform.GetChild(18).gameObject.SetActive(false);
	}

	IEnumerator bossSpecialAttackFX1()
	{
		GameObject go = GameObject.Find("Animation Master");
		go.transform.GetChild(19).gameObject.SetActive(true);
		yield return new WaitForSeconds(2);
		go.transform.GetChild(19).gameObject.SetActive(false);
	}

	IEnumerator bossSpecialAttackFX2()
	{
		GameObject go = GameObject.Find("Animation Master");
		go.transform.GetChild(20).gameObject.SetActive(true);
		yield return new WaitForSeconds(2);
		go.transform.GetChild(20).gameObject.SetActive(false);
	}

	public void toDungeon()
	{  
		GameObject.Find("LoadingScreenControl").GetComponent<LoadingScreenControl>().LoadScreen(9);
		SaveSystem.LoadFromSlot(1); 
		SceneManager.LoadScene("Pause_Menu", LoadSceneMode.Additive);
	}
}

