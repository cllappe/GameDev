using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStateMachine : MonoBehaviour
{

	public Character character;
	public int actionsLeft = 0;
	private Character player;

	private bool enemyDmgEnabled;
	private Text turnBox;
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
			GameObject turnBoxGO = GameObject.Find("Turn Counter Text");
			turnBox = turnBoxGO.GetComponent<Text>();
		}
		else if (character.charType == Character.Type.ENEMY)
		{
			player = GameObject.Find("Player").GetComponent<CharacterStateMachine>().character;
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
						CardBattleManager.enemyTurn = true;
						Invoke("enemyAttack",2);
						enemyDmgEnabled = false;
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
						GameObject.Find("Enemy1DropZone").SetActive(false);
					}
					else if (name == "Enemy 2")
					{
						GameObject.Find("Enemy2DropZone").SetActive(false);
					}
					else if (name == "Enemy 3")
					{
						GameObject.Find("Enemy3DropZone").SetActive(false);
					}
					GameObject.Find("GameManager").GetComponent<CardBattleManager>().charOrder.RemoveAt(0);
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
						if (character.charType == Character.Type.ENEMY && GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount != 0)
						{
							enemyDmgEnabled = true;
							
							if (!GameObject.Find("LevelManager").GetComponent<LevelManager>().charRemoved)
							{
								currentState = TurnState.ACTION;
								GameObject.Find("GameManager").GetComponent<CardBattleManager>().charOrder.Remove(character);
								GameObject.Find("LevelManager").GetComponent<LevelManager>().charRemoved = true;
							}
							
						}

						if (!GameObject.Find("LevelManager").GetComponent<LevelManager>().charRemoved)
						{
							currentState = TurnState.ACTION;
                            GameObject.Find("GameManager").GetComponent<CardBattleManager>().charOrder.Remove(character);
							GameObject.Find("LevelManager").GetComponent<LevelManager>().charRemoved = true;
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
				enabled = false;
				break;
			}
			case (TurnState.DEFEAT):
			{
				turnBox.text = "Defeat";
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

	IEnumerator enemyAttackDelay()
	{
		if (enemyDmgEnabled)
		{
			yield break;
		}
		enemyDmgEnabled = true;
		yield return new WaitForSeconds(2);
		enemyAttack();

	}
	
	public void enemyAttack()
	{
		//Debug.Log(character.name + " " + actionsLeft);
		player.health -= character.basicAttackDmg;
		GameObject.Find("LevelManager").GetComponent<LevelManager>().UpdatePlayerHealthBar();
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
}

