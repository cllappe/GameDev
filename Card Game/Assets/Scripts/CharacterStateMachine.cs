using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStateMachine : MonoBehaviour
{

	public Character character;
	private int actionsLeft = 0;
	private int turnCount = 0;
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
		turnCount = 1;
		currentState = TurnState.BEGINING;
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
				if (actionsLeft == 0)
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
						actionsLeft--;
						if (actionsLeft == 0)
						{
							Dragable.playerTurn = false;
							CardBattleManager.draw1card = true;
						}
					}
				}
				else
				{
					CardBattleManager.enemyTurn = true;
					StartCoroutine(enemyAttackDelay());	
				}	

				turnCount++;
				turnMonitor();
				break;
			}
			case (TurnState.ADDTOLIST):
			{
				if (character.charType == Character.Type.PLAYER && turnCount == 1)
				{
					CardBattleManager.charOrder.Insert(0,character);
				}
				else
				{
					CardBattleManager.charOrder.Add(character);	
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
				if (CardBattleManager.charOrder.Count > 1 && !CardBattleManager.enemyTurn && !Dragable.playerTurn)
				{

					if (character == CardBattleManager.charOrder.First())
					{
						currentState = TurnState.ACTION;
						CardBattleManager.charOrder.RemoveAt(0);
					}
				}
				else
				{
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
		if (CardBattleManager.deadEnemies == LevelManager.numberOfEnemies && character.charType == Character.Type.PLAYER)
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
		else if (character.health <= 0)
		{
			currentState = TurnState.DYING;
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
		//Debug.Log("Did Damage to Player");
		actionsLeft--;
		//Debug.Log(character.name + " " + actionsLeft);
		enemyDmgEnabled = false;
		CardBattleManager.enemyTurn = false;
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

