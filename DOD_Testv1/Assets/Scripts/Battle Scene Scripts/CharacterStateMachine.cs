using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Guirao.UltimateTextDamage;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class CharacterStateMachine : MonoBehaviour
{

	public Character character;
	public int actionsLeft = 0;
	private Character player;
	private GameObject playerGO;

	private bool enemyDmgEnabled;
	private TextMeshProUGUI turnBox;
	private bool dyingState;
	private bool isDead;
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
		if (character.charType == Character.Type.PLAYER)
		{
			GameObject turnBoxGO = GameObject.Find("Player Notification");
			turnBox = turnBoxGO.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
		}
		else if (character.charType == Character.Type.ENEMY || character.charType == Character.Type.MINIBOSS)
		{
			playerGO = GameObject.Find("Player");
			player = playerGO.GetComponent<CharacterStateMachine>().character;
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
				dyingState = false;
				isDead = false;
				turnMonitor();
				break;
			}
			case (TurnState.ACTION):
			{
				if (actionsLeft == 0 && !CardBattleManager.enemyTurn)
				{
					actionsLeft = 1;
				}

				if (character.charType == Character.Type.PLAYER)
				{
					//Debug.Log("Current Player Health " + character.health);
					//Debug.Log("Current Luck " + character.luck);
					turnBox.text = "Actions Remaining = " + actionsLeft;
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
							if (miniAttackVar <= character.luck)
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
				if (character.charType == Character.Type.PLAYER)
				{
             		turnBox.text = "Enemy Turn";
       			}
				if (GameObject.Find("GameManager").GetComponent<CardBattleManager>().charOrder.Count > 1 && !CardBattleManager.enemyTurn && !Dragable.playerTurn)
				{
					if (character == GameObject.Find("GameManager").GetComponent<CardBattleManager>().charOrder.First())
					{
						if ((character.charType == Character.Type.ENEMY ||character.charType == Character.Type.MINIBOSS )&& GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount != 0)
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
				turnBox.text = "VICTORY!";
				GameObject go = GameObject.Find("Animation Master");
				go.transform.GetChild(13).gameObject.SetActive(true);
				enabled = false;
				break;
			}
			case (TurnState.DEFEAT):
			{
				turnBox.text = "Defeat";
				GameObject go = GameObject.Find("Animation Master");
				go.transform.GetChild(10).SetPositionAndRotation(new Vector3(-5, 0, 4), Quaternion.identity);
				go.transform.GetChild(10).gameObject.SetActive(true);
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
				if (sceneName == "MiniBossBattle")
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
			character.health -= character.basicAttackDmg;
			character.dmgReflected = false;
			GameObject.Find("LevelManager").GetComponent<LevelManager>().UpdateHealthBars();
			GameObject UTD = GameObject.Find("MiniBossUTD");
			UTD.GetComponent<UltimateTextDamageManager>()
				.Add(character.basicAttackDmg.ToString(), UTD.transform, "damage");
			StartCoroutine(EnemyDmgAnimate(2));
			StartCoroutine(miniSpecialAttackFX());
		}
		else
		{
			GameObject PlayerUTD = GameObject.Find("PlayerUTD");
			int dmg =  Mathf.RoundToInt(character.basicAttackDmg * player.defence);
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

	IEnumerator miniSpecialAttackFX()
	{
		GameObject go = GameObject.Find("Animation Master");
		go.transform.GetChild(18).gameObject.SetActive(true);
		yield return new WaitForSeconds(2);
		go.transform.GetChild(18).gameObject.SetActive(false);
	}
}

