using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// Script for dragging and card hover on mouse over.
/// </summary>

public class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Targets targets;

    public Transform parentToReturnTo = null;
    public Transform placeholderParent = null;
    public float mouseOffset;
    public static bool validDrop = false;
    public static bool dragging = false;
    private bool validDropHappened = false;
    public static bool playerTurn;
    public static bool actionsIncreased = false;

    private Vector3 startPos;

    GameObject placeholder = null;

    private void OnMouseEnter()
    {
        if (!Input.GetMouseButton(0))
        {
            startPos = transform.position;
            iTween.MoveTo(gameObject, new Vector3(transform.position.x - 0.5f, transform.position.y + 1.2f, transform.position.z - 1.2f), 1f);
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (playerTurn)
        {
            dragging = true;

            placeholder = new GameObject();
            placeholder.transform.SetParent(this.transform.parent);
            LayoutElement le = placeholder.AddComponent<LayoutElement>();
            le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
            le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
            le.flexibleWidth = 0;
            le.flexibleHeight = 0;

            placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

            parentToReturnTo = this.transform.parent;
            placeholderParent = parentToReturnTo;
            this.transform.SetParent(this.transform.parent.parent);

            GetComponent<CanvasGroup>().blocksRaycasts = false;
            mouseOffset = Vector3.Distance(this.transform.position, Camera.main.transform.position);   
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (playerTurn)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(mouseOffset);
            rayPoint.z = 0;
            this.transform.position = rayPoint;
        

            if (placeholder.transform.parent != placeholderParent)
                placeholder.transform.SetParent(placeholderParent);

            int newSiblingIndex = placeholderParent.childCount;

            for (int i = 0; i < placeholderParent.childCount; i++)
            {
                if (this.transform.position.x < placeholderParent.GetChild(i).position.x)
                {

                    newSiblingIndex = i;

                    if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                        newSiblingIndex--;

                    break;
                }
            }

            placeholder.transform.SetSiblingIndex(newSiblingIndex);  
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (playerTurn)
        {
            if (validDrop){
                //Debug.Log("In Valid Drop");
                startPos = this.transform.position;
                validDropHappened = true;
                this.transform.SetParent(parentToReturnTo);
                this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
                //Debug.Log(DropZone.droppedOn);
                cardAction(DropZone.droppedOn);
                foreach (Transform child in transform) {
                    GameObject.Destroy(child.gameObject);
                }
            }
            else{
                startPos = new Vector3(this.transform.position.x, startPos.y, startPos.z);
                this.transform.SetParent(parentToReturnTo);
                this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
            }
            GetComponent<CanvasGroup>().blocksRaycasts = true;

            dragging = false;
            Destroy(placeholder);   
        }
    }

    private void OnMouseExit()
    {
        if (!validDropHappened)
        {
            iTween.MoveTo(gameObject, startPos, 0f);
        }

        validDrop = false;
    }

    private void cardAction(string dropZoneName)
    {
        if (this.GetComponent<CardDisplay>().numberOfTargets == 1)
        {
            if (dropZoneName == "Enemy1DropZone")
            {
                GameObject dropedOn = GameObject.Find("Enemy 1");
                Character dropedOnChar = dropedOn.GetComponent<CharacterStateMachine>().character;
                //Debug.Log(dropedOnChar.health);
                if (this.GetComponent<CardDisplay>().turnOnReflect)
                {
                    dropedOnChar.dmgReflected = true;
                }
                else
                {
                    dropedOnChar.health -= this.GetComponent<CardDisplay>().damage * 
                                           GameObject.Find("Player").GetComponent<CharacterStateMachine>().character.dmgMod;
                    StartCoroutine(dmgTo1EnemyCoRoutine(dropedOn));
                    if (this.GetComponent<CardDisplay>().lifeSteal)
                    {
                        GameObject player = GameObject.Find("Player");
                        player.GetComponent<CharacterStateMachine>().character.health +=
                            this.GetComponent<CardDisplay>().damage * 
                            GameObject.Find("Player").GetComponent<CharacterStateMachine>().character.dmgMod;
                        StartCoroutine(healPlayerCoRoutine(player, true));
                    }   
                }
            }
            else if (dropZoneName == "Enemy2DropZone")
            {
                GameObject dropedOn = GameObject.Find("Enemy 2");
                Character dropedOnChar = dropedOn.GetComponent<CharacterStateMachine>().character;
                //Debug.Log(dropedOnChar.health);
                if (this.GetComponent<CardDisplay>().turnOnReflect)
                {
                    dropedOnChar.dmgReflected = true;
                }
                else
                {
                    dropedOnChar.health -= this.GetComponent<CardDisplay>().damage * 
                                           GameObject.Find("Player").GetComponent<CharacterStateMachine>().character.dmgMod;
                    StartCoroutine(dmgTo1EnemyCoRoutine(dropedOn));
                    if (this.GetComponent<CardDisplay>().lifeSteal)
                    {
                        GameObject player = GameObject.Find("Player");
                        player.GetComponent<CharacterStateMachine>().character.health +=
                            this.GetComponent<CardDisplay>().damage * 
                            GameObject.Find("Player").GetComponent<CharacterStateMachine>().character.dmgMod;
                        StartCoroutine(healPlayerCoRoutine(player, true));
                    }   
                }
            }
            else if (dropZoneName == "Enemy3DropZone")
            {
                GameObject dropedOn = GameObject.Find("Enemy 3");
                Character dropedOnChar = dropedOn.GetComponent<CharacterStateMachine>().character;
                //Debug.Log(dropedOnChar.health);
                if (this.GetComponent<CardDisplay>().turnOnReflect)
                {
                    dropedOnChar.dmgReflected = true;
                }
                else
                {
                    dropedOnChar.health -= this.GetComponent<CardDisplay>().damage * 
                                           GameObject.Find("Player").GetComponent<CharacterStateMachine>().character.dmgMod;
                    StartCoroutine(dmgTo1EnemyCoRoutine(dropedOn));
                    if (this.GetComponent<CardDisplay>().lifeSteal)
                    {
                        GameObject player = GameObject.Find("Player");
                        player.GetComponent<CharacterStateMachine>().character.health +=
                            this.GetComponent<CardDisplay>().damage * 
                            GameObject.Find("Player").GetComponent<CharacterStateMachine>().character.dmgMod;
                        StartCoroutine(healPlayerCoRoutine(player, true));
                    }   
                }
            }
            else if (dropZoneName == "PlayerDropZone")
            {
                Debug.Log("Dropped a Card on The Player");
                GameObject dropedOn = GameObject.Find("Player");
                Character dropedOnChar = dropedOn.GetComponent<CharacterStateMachine>().character;
                if (GetComponent<CardDisplay>().cardType == Type.HEALCARD)
                {
                    dropedOnChar.health += this.GetComponent<CardDisplay>().heal;
                    StartCoroutine(healPlayerCoRoutine(dropedOn, false));
                }
                else
                {
                    if (this.GetComponent<CardDisplay>().turnIncrease != 0)
                    {
                        actionsIncreased = true;
                        dropedOn.GetComponent<CharacterStateMachine>().actionsLeft +=
                            this.GetComponent<CardDisplay>().turnIncrease;
                        actionsIncreased = false;
                        Debug.Log(dropedOn.GetComponent<CharacterStateMachine>().actionsLeft);
                    }

                    if (this.GetComponent<CardDisplay>().drawCards != 0)
                    {
                        Debug.Log("Draw Cards:" + this.GetComponent<CardDisplay>().drawCards);
                        for (int i = 0; i < this.GetComponent<CardDisplay>().drawCards; i++)
                        {
                            GameObject.Find("GameManager").GetComponent<CardBattleManager>().drawACard();
                        }
                    }

                    if (this.GetComponent<CardDisplay>().numOfTurns != 0)
                    {
                        int startTurn = GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount;
                        int endTurn = startTurn + this.GetComponent<CardDisplay>().numOfTurns + 1;
                        StartCoroutine(powerUpCoRoutine(startTurn,endTurn));
                    }
                }
            }
        }
        else
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enemies.Length; i++)
            {
                Character effectedChar = enemies[i].GetComponent<CharacterStateMachine>().character;
                effectedChar.health -= this.GetComponent<CardDisplay>().damage * 
                                       GameObject.Find("Player").GetComponent<CharacterStateMachine>().character.dmgMod;
                StartCoroutine(dmgTo1EnemyCoRoutine(enemies[i]));
            }
        }
    }

    IEnumerator powerUpCoRoutine(int startTurn,int endTurn)
    {
        Debug.Log("Entered Co-Routine. Current Turn: " + startTurn + " End Turn: " + endTurn);
        GameObject dropedOn = GameObject.Find("Player");
        Character dropedOnChar = dropedOn.GetComponent<CharacterStateMachine>().character;
        
        int initalLuck = dropedOnChar.luck;
        float initalDefence = dropedOnChar.defence;
        int initalDmgMod = dropedOnChar.dmgMod;

        if (startTurn == GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount)
        {
            if (this.GetComponent<CardDisplay>().luckUp != 0)
            {
                dropedOnChar.luck += this.GetComponent<CardDisplay>().luckUp;
                yield return new WaitUntil(() =>
                    GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount == endTurn);
                dropedOnChar.luck = initalLuck;
                Debug.Log("Luck Reset");
            }

            if (this.GetComponent<CardDisplay>().reduceDmgMod != 0.0f)
            {
                dropedOnChar.defence = this.GetComponent<CardDisplay>().reduceDmgMod;
                yield return new WaitUntil(() =>
                    GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount == endTurn);
                dropedOnChar.defence = initalDefence;
                Debug.Log("Defence Reset");
            }

            if (this.GetComponent<CardDisplay>().dmgIncMod != 0)
            {
                dropedOnChar.dmgMod = this.GetComponent<CardDisplay>().dmgIncMod;
                yield return new WaitUntil(() =>
                    GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount == endTurn);
                dropedOnChar.dmgMod = initalDmgMod;
                Debug.Log("Dmg Mod Reset");
            }
        }
    }

    IEnumerator dmgTo1EnemyCoRoutine(GameObject dropedOn)
    {
        dropedOn.GetComponent<CharacterDisplay>().CBText.GetComponent<Text>().color = Color.red;
        dropedOn.GetComponent<CharacterDisplay>().CBText.GetComponent<Text>().text = "-" + 
                                                       (this.GetComponent<CardDisplay>().damage * 
                 GameObject.Find("Player").GetComponent<CharacterStateMachine>().character.dmgMod);
        yield return new WaitForSeconds(2);
        dropedOn.GetComponent<CharacterDisplay>().CBText.GetComponent<Text>().text = "";
    }

    IEnumerator healPlayerCoRoutine(GameObject dropedOn, bool lifeSteal)
    {
        dropedOn.GetComponent<CharacterDisplay>().CBText.GetComponent<Text>().color = Color.green;
        if (lifeSteal)
        {
            dropedOn.GetComponent<CharacterDisplay>().CBText.GetComponent<Text>().text =
                "+" + (this.GetComponent<CardDisplay>().damage *
                       GameObject.Find("Player").GetComponent<CharacterStateMachine>().character.dmgMod);
        }
        else
        {
            dropedOn.GetComponent<CharacterDisplay>().CBText.GetComponent<Text>().text =
                "+" + this.GetComponent<CardDisplay>().heal;
        }

        yield return new WaitForSeconds(2);
        dropedOn.GetComponent<CharacterDisplay>().CBText.GetComponent<Text>().text = "";
    }
}
