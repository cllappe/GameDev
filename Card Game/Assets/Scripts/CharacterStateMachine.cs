using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{

	public Character character;
	private int actionsLeft = 0;

	public enum TurnState
	{
		BEGINING,
		ACTION,
		ADDTOLIST,
		WAITING,
		DEAD,
		VICTORY,
		DYING
	}

	public TurnState currentState;

	// Use this for initialization
	void Start ()
	{
		currentState = TurnState.BEGINING;
		Debug.Log(character.health);
	}
	
	// Update is called once per frame
	private void Update () {
		switch (currentState)
		{
			case (TurnState.BEGINING):
			{
				turnMonitor();
				break;
			}
			case (TurnState.ACTION):
			{
				actionsLeft = 1;
				
				if (character.charType == Character.Type.PLAYER)
				{
					Debug.Log("Character Action");
					actionsLeft--;
				}
				else
				{
					Debug.Log("Enemy Action");
					character.health = 0;
					actionsLeft--;
				}
				turnMonitor();
				break;
			}
			case (TurnState.ADDTOLIST):
			{
				CardBattleManager.charOrder.Add(character);
				currentState = TurnState.WAITING;
				break;
			}
			case (TurnState.DYING):
			{
				if (character.charType == Character.Type.ENEMY || character.charType == Character.Type.MINIBOSS ||
				    character.charType == Character.Type.BOSS)
				{
					CardBattleManager.deadEnemies++;
					currentState = TurnState.DEAD;
					Debug.Log(CardBattleManager.deadEnemies);
				}
				break;
			}
			case (TurnState.DEAD):
			{
				break;
			}
			case (TurnState.WAITING):
			{
				if (CardBattleManager.charOrder.Count > 1)
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
				Debug.Log("Player Was Victorious");
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
		else if (actionsLeft == 0 && character.health > 0)
		{
			currentState = TurnState.ADDTOLIST;
			Debug.Log("Added " + character.charType + " to list.");
		}
		else if (character.health == 0)
		{
			currentState = TurnState.DYING;
		}
	}

	public void csm_Set(Character thisCharacter)
	{
		character = thisCharacter;
	}
}
