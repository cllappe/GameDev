using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Threading;
using Guirao.UltimateTextDamage;
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
        GameObject CombatLog = GameObject.Find("Combat Log");
        if (this.GetComponent<CardDisplay>().numberOfTargets == 1)
        {
            if (dropZoneName == "Enemy1DropZone")
            {
                GameObject EnemyUTD = GameObject.Find("Enemy1UTD");
                GameObject dropedOn = GameObject.Find("Enemy 1");
                Character dropedOnChar = dropedOn.GetComponent<CharacterStateMachine>().character;
                //Debug.Log(dropedOnChar.health);
                if (this.GetComponent<CardDisplay>().turnOnReflect)
                {
                    dropedOnChar.dmgReflected = true;
                    CombatLog.GetComponent<CombatLogPopulate>().populate("Reflecting Enemy 1's next attack.",false);
                    if (this.GetComponent<CardDisplay>().canCrit)
                    {
                        critMethod(dropedOn,CombatLog);
                    }
                }
                else
                {
                    int damage = this.GetComponent<CardDisplay>().damage * 
                                 GameObject.Find("Player").GetComponent<CharacterStateMachine>().character.dmgMod;
                    dropedOnChar.health -= damage;
                    EnemyUTD.GetComponent<UltimateTextDamageManager>().Add(damage.ToString(),EnemyUTD.transform.position, "damage");
                    if (this.GetComponent<CardDisplay>().lifeSteal)
                    {
                        GameObject player = GameObject.Find("Player");
                        player.GetComponent<CharacterStateMachine>().character.health += damage;
                        if (player.GetComponent<CharacterStateMachine>().character.health > 100)
                        {
                            player.GetComponent<CharacterStateMachine>().character.health = 100;
                        }
                        GameObject PlayerUTD = GameObject.Find("PlayerUTD");
                        PlayerUTD.GetComponent<UltimateTextDamageManager>()
                            .Add(damage.ToString(), PlayerUTD.transform, "heal");
                    }
                    if (this.GetComponent<CardDisplay>().canCrit)
                    {
                        critMethod(dropedOn,CombatLog);
                    }
                }

                StartCoroutine(annimationCoRoutine(this.GetComponent<CardDisplay>().nameText.text, 1));
            }
            else if (dropZoneName == "Enemy2DropZone")
            {
                GameObject EnemyUTD = GameObject.Find("Enemy2UTD");
                GameObject dropedOn = GameObject.Find("Enemy 2");
                Character dropedOnChar = dropedOn.GetComponent<CharacterStateMachine>().character;
                //Debug.Log(dropedOnChar.health);
                if (this.GetComponent<CardDisplay>().turnOnReflect)
                {
                    dropedOnChar.dmgReflected = true;
                    CombatLog.GetComponent<CombatLogPopulate>().populate("Reflecting Enemy 2's next attack.", false);
                    if (this.GetComponent<CardDisplay>().canCrit)
                    {
                        critMethod(dropedOn,CombatLog);
                    }
                }
                else
                {
                    int damage = this.GetComponent<CardDisplay>().damage * 
                                 GameObject.Find("Player").GetComponent<CharacterStateMachine>().character.dmgMod;
                    dropedOnChar.health -= damage;
                    EnemyUTD.GetComponent<UltimateTextDamageManager>().Add(damage.ToString(),EnemyUTD.transform.position, "damage");
                    if (this.GetComponent<CardDisplay>().lifeSteal)
                    {
                        GameObject player = GameObject.Find("Player");
                        player.GetComponent<CharacterStateMachine>().character.health += damage;
                        if (player.GetComponent<CharacterStateMachine>().character.health > 100)
                        {
                            player.GetComponent<CharacterStateMachine>().character.health = 100;
                        }
                        GameObject PlayerUTD = GameObject.Find("PlayerUTD");
                        PlayerUTD.GetComponent<UltimateTextDamageManager>()
                            .Add(damage.ToString(), PlayerUTD.transform, "heal");
                    } 
                    if (this.GetComponent<CardDisplay>().canCrit)
                    {
                        critMethod(dropedOn,CombatLog);
                    }
                }
                StartCoroutine(annimationCoRoutine(this.GetComponent<CardDisplay>().nameText.text, 2));
            }
            else if (dropZoneName == "Enemy3DropZone")
            {
                GameObject EnemyUTD = GameObject.Find("Enemy3UTD");
                GameObject dropedOn = GameObject.Find("Enemy 3");
                Character dropedOnChar = dropedOn.GetComponent<CharacterStateMachine>().character;
                //Debug.Log(dropedOnChar.health);
                if (this.GetComponent<CardDisplay>().turnOnReflect)
                {
                    dropedOnChar.dmgReflected = true;
                    CombatLog.GetComponent<CombatLogPopulate>().populate("Reflecting Enemy 3's next attack.", false);
                    if (this.GetComponent<CardDisplay>().canCrit)
                    {
                        critMethod(dropedOn,CombatLog);
                    }
                }
                else
                {
                    int damage = this.GetComponent<CardDisplay>().damage * 
                                 GameObject.Find("Player").GetComponent<CharacterStateMachine>().character.dmgMod;
                    dropedOnChar.health -= damage;
                    EnemyUTD.GetComponent<UltimateTextDamageManager>().Add(damage.ToString(),EnemyUTD.transform.position, "damage");
                    if (this.GetComponent<CardDisplay>().lifeSteal)
                    {
                        GameObject player = GameObject.Find("Player");
                        player.GetComponent<CharacterStateMachine>().character.health += damage;
                        if (player.GetComponent<CharacterStateMachine>().character.health > 100)
                        {
                            player.GetComponent<CharacterStateMachine>().character.health = 100;
                        }
                        GameObject PlayerUTD = GameObject.Find("PlayerUTD");
                        PlayerUTD.GetComponent<UltimateTextDamageManager>()
                            .Add(damage.ToString(), PlayerUTD.transform, "heal");
                    }  
                    if (this.GetComponent<CardDisplay>().canCrit)
                    {
                        critMethod(dropedOn,CombatLog);
                    }
                }
                StartCoroutine(annimationCoRoutine(this.GetComponent<CardDisplay>().nameText.text, 3));
            }
            else if (dropZoneName == "PlayerDropZone")
            {
                GameObject PlayerUTD = GameObject.Find("PlayerUTD");
                GameObject dropedOn = GameObject.Find("Player");
                Character dropedOnChar = dropedOn.GetComponent<CharacterStateMachine>().character;
                if (GetComponent<CardDisplay>().cardType == Type.HEALCARD)
                {
                    dropedOnChar.health += this.GetComponent<CardDisplay>().heal;
                    if (dropedOnChar.health > 100)
                    {
                        dropedOnChar.health = 100;
                    }

                    PlayerUTD.GetComponent<UltimateTextDamageManager>()
                        .Add(this.GetComponent<CardDisplay>().heal.ToString(), PlayerUTD.transform, "heal");
                    if (this.GetComponent<CardDisplay>().canCrit)
                    {
                        critMethod(dropedOn, CombatLog);
                    }
                }
                else
                {
                    if (this.GetComponent<CardDisplay>().turnIncrease != 0)
                    {
                        actionsIncreased = true;
                        dropedOn.GetComponent<CharacterStateMachine>().actionsLeft +=
                            this.GetComponent<CardDisplay>().turnIncrease;
                        actionsIncreased = false;
                        CombatLog.GetComponent<CombatLogPopulate>().populate("Added " + this.GetComponent<CardDisplay>().turnIncrease + " actions this turn.", false);
                    }

                    if (this.GetComponent<CardDisplay>().drawCards != 0)
                    {
                        Debug.Log("Draw Cards:" + this.GetComponent<CardDisplay>().drawCards);
                        for (int i = 0; i < this.GetComponent<CardDisplay>().drawCards; i++)
                        {
                            GameObject.Find("GameManager").GetComponent<CardBattleManager>().drawACard();
                            CombatLog.GetComponent<CombatLogPopulate>().populate("Drawing a card.", false);
                        }
                    }

                    if (this.GetComponent<CardDisplay>().numOfTurns != 0)
                    {
                        int startTurn = GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount;
                        int endTurn = startTurn + this.GetComponent<CardDisplay>().numOfTurns + 1;
                        StartCoroutine(powerUpCoRoutine(PlayerUTD, startTurn,endTurn, CombatLog));
                    }
                }
                StartCoroutine(annimationCoRoutine(this.GetComponent<CardDisplay>().nameText.text, 4));
            }
        }
        else
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject[] utds = GameObject.FindGameObjectsWithTag("UTD");
            for (int i = 0; i < enemies.Length; i++)
            {
                int dmg = this.GetComponent<CardDisplay>().damage * 
                    GameObject.Find("Player").GetComponent<CharacterStateMachine>().character.dmgMod;
                Character effectedChar = enemies[i].GetComponent<CharacterStateMachine>().character;
                effectedChar.health -= dmg;
                utds[i].GetComponent<UltimateTextDamageManager>().Add(dmg.ToString(), utds[i].transform, "damage");
            }

            if (this.GetComponent<CardDisplay>().canCrit)
            {
                GameObject dummy = enemies[0];
                critMethod(dummy, CombatLog);
            }

            StartCoroutine(annimationCoRoutine(this.GetComponent<CardDisplay>().nameText.text, 1));
        }
    }

    IEnumerator powerUpCoRoutine(GameObject PlayerUTD,int startTurn,int endTurn, GameObject CombatLog)
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
                Debug.Log(dropedOnChar.luck);
                CombatLog.GetComponent<CombatLogPopulate>().populate(this.GetComponent<CardDisplay>().luckUp + "% luck added for 3 turns.", false);
                yield return new WaitUntil(() =>
                    GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount == endTurn);
                CombatLog.GetComponent<CombatLogPopulate>().populate("Luck has returned to normal.", false);
                dropedOnChar.luck = initalLuck;
                Debug.Log("Luck Reset");
            }

            if (this.GetComponent<CardDisplay>().reduceDmgMod != 0.0f)
            {
                dropedOnChar.defence = this.GetComponent<CardDisplay>().reduceDmgMod;
                CombatLog.GetComponent<CombatLogPopulate>().populate(this.GetComponent<CardDisplay>().reduceDmgMod*100 + "% damage received for 3 turns.", false);
                yield return new WaitUntil(() =>
                    GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount == endTurn);
                CombatLog.GetComponent<CombatLogPopulate>().populate("Damage received has returned to 100%.", false);
                dropedOnChar.defence = initalDefence;
                Debug.Log("Defence Reset");
            }

            if (this.GetComponent<CardDisplay>().dmgIncMod != 0)
            {
                dropedOnChar.dmgMod = this.GetComponent<CardDisplay>().dmgIncMod;
                CombatLog.GetComponent<CombatLogPopulate>().populate(this.GetComponent<CardDisplay>().dmgIncMod + " times damage done for 3 turns.", false);
                yield return new WaitUntil(() =>
                    GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount == endTurn);
                CombatLog.GetComponent<CombatLogPopulate>().populate("Out going damage is no longer modified by " + this.GetComponent<CardDisplay>().dmgIncMod + ".", false);
                dropedOnChar.dmgMod = initalDmgMod;
                Debug.Log("Dmg Mod Reset");
            }
        }
    }

    void critMethod(GameObject DropedOn, GameObject CombatLog)
    {
        GameObject player = GameObject.Find("Player");
        int blessingValue = Random.Range(0, 100);
        if (blessingValue <= player.GetComponent<CharacterStateMachine>().character.luck)
        {
            StartCoroutine(annimationCoRoutine("Crit", 4));
            if (this.GetComponent<CardDisplay>().critHeal != 0)
            {
                Debug.Log("Heal Crit");
                player.GetComponent<CharacterStateMachine>().character.health +=
                    this.GetComponent<CardDisplay>().critHeal;
                if (player.GetComponent<CharacterStateMachine>().character.health > 100)
                {
                    player.GetComponent<CharacterStateMachine>().character.health = 100;
                }

                CombatLog.GetComponent<CombatLogPopulate>().populate("CRITICAL: Healed for 100%.", true);
            }

            if (this.GetComponent<CardDisplay>().critLifeSteal)
            {
                Debug.Log("Life Steal Crit");
                player.GetComponent<CharacterStateMachine>().character.health +=
                    this.GetComponent<CardDisplay>().damage * player.GetComponent<CharacterDisplay>().dmgMod;
                if (player.GetComponent<CharacterStateMachine>().character.health > 100)
                {
                    player.GetComponent<CharacterStateMachine>().character.health = 100;
                }

                CombatLog.GetComponent<CombatLogPopulate>().populate(
                    "CRITICAL: Life Steal triggered for " + this.GetComponent<CardDisplay>().damage *
                    player.GetComponent<CharacterDisplay>().dmgMod + ".", true);
            }

            if (this.GetComponent<CardDisplay>().numOfTurns != 0)
            {
                Debug.Log("Play Extra Crit");
                player.GetComponent<CharacterStateMachine>().actionsLeft += this.GetComponent<CardDisplay>().critNumOfTurns;
                CombatLog.GetComponent<CombatLogPopulate>().populate("CRITICAL: Gained " + this.GetComponent<CardDisplay>().critNumOfTurns + " extra actions this turn.", true);
            }

            if (this.GetComponent<CardDisplay>().critDmgAdd != 0)
            {
                Debug.Log("Dmg Crit");
                int dmg = this.GetComponent<CardDisplay>().critDmgAdd;
                if (this.GetComponent<CardDisplay>().critNumOfTargets == 3)
                {
                    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    for (int i = 0; i < enemies.Length; i++)
                    {
                        Character effectedChar = enemies[i].GetComponent<CharacterStateMachine>().character;
                        effectedChar.health -= dmg;
                    }

                    CombatLog.GetComponent<CombatLogPopulate>()
                        .populate("CRITICAL: " + dmg + " extra damage done to all enemies.", true);
                }
                else
                {
                    Character effectedChar = DropedOn.GetComponent<CharacterStateMachine>().character;
                    effectedChar.health -= dmg;
                    CombatLog.GetComponent<CombatLogPopulate>()
                        .populate("CRITICAL: " + dmg + " extra damage done to " + DropedOn.name, true);
                }
            }

            if (this.GetComponent<CardDisplay>().critSkip)
            {
                Debug.Log("Crit Skip");
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                for (int i = 0; i < enemies.Length; i++)
                {
                    Character effectedChar = enemies[i].GetComponent<CharacterStateMachine>().character;
                    effectedChar.skipTurn = true;
                }
                CombatLog.GetComponent<CombatLogPopulate>()
                    .populate("CRITICAL: All enemies attack skipped this turn.", true);
            }

            if (this.GetComponent<CardDisplay>().critEnemyCritNegate)
            {
                Debug.Log("Crit Neg");
                Character dropedOnChar = DropedOn.GetComponent<CharacterStateMachine>().character;
                CombatLog.GetComponent<CombatLogPopulate>()
                    .populate("CRITICAL: " + DropedOn.name + " luck negated for three turns.", true);
                StartCoroutine(critNegateCoRoutine(dropedOnChar,
                    GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount,
                    GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount + 4,CombatLog, DropedOn.name));

            }

            if (this.GetComponent<CardDisplay>().critDraw != 0)
            {
                Debug.Log("Draw crit");
                for (int i = 0; i < this.GetComponent<CardDisplay>().critDraw; i++)
                {
                    GameObject.Find("GameManager").GetComponent<CardBattleManager>().drawACard();
                    CombatLog.GetComponent<CombatLogPopulate>()
                        .populate("CRITICAL: Draw an extra card.", true);
                }
            }
        }
    }

    IEnumerator critNegateCoRoutine(Character effectedChar, int startTurn, int endTurn, GameObject CombatLog, string enemyName)
    {
        int initalLuck = effectedChar.luck;
        effectedChar.luck = 0;
        yield return new WaitUntil(() =>
            GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount == endTurn);
        effectedChar.luck = initalLuck;
        CombatLog.GetComponent<CombatLogPopulate>()
            .populate(enemyName + "'s luck has returned to normal.", false);
    }

    IEnumerator annimationCoRoutine(string CardName, int tarNum)
    {
        GameObject go = GameObject.Find("Animation Master");
        if (CardName == "Scorch")
        {
            go.transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(5);
            go.transform.GetChild(0).gameObject.SetActive(false);
        }
        else if (CardName == "Wave")
        {
            go.transform.GetChild(1).gameObject.SetActive(true);
            yield return new WaitForSeconds(3);
            go.transform.GetChild(1).gameObject.SetActive(false);
        }
        else if (CardName == "Tremor")
        {
            go.transform.GetChild(2).gameObject.SetActive(true);
            yield return new WaitForSeconds(5);
            go.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (CardName == "Drain")
        {
            if (tarNum == 1)
            {
                go.transform.GetChild(3).SetPositionAndRotation(new Vector3(1,0,2), Quaternion.identity);
                go.transform.GetChild(3).gameObject.SetActive(true);
                yield return new WaitForSeconds(3);
                go.transform.GetChild(3).gameObject.SetActive(false);
            }
            else if (tarNum == 2)
            {
                go.transform.GetChild(3).SetPositionAndRotation(new Vector3(4,0.5f,2), Quaternion.identity);
                go.transform.GetChild(3).gameObject.SetActive(true);
                yield return new WaitForSeconds(3);
                go.transform.GetChild(3).gameObject.SetActive(false);
            }
            else if (tarNum == 3)
            {
                go.transform.GetChild(3).SetPositionAndRotation(new Vector3(7,0,2), Quaternion.identity);
                go.transform.GetChild(3).gameObject.SetActive(true);
                yield return new WaitForSeconds(3);
                go.transform.GetChild(3).gameObject.SetActive(false);
            }
        }
        else if (CardName == "Counterstrike")
        {
            if (tarNum == 1)
            {
                go.transform.GetChild(4).SetPositionAndRotation(new Vector3(1, 0, 4), Quaternion.identity);
                go.transform.GetChild(4).gameObject.SetActive(true);
                yield return new WaitForSeconds(3);
                go.transform.GetChild(4).gameObject.SetActive(false);
            }
            else if (tarNum == 2)
            {
                go.transform.GetChild(4).SetPositionAndRotation(new Vector3(4, 0.5f, 4), Quaternion.identity);
                go.transform.GetChild(4).gameObject.SetActive(true);
                yield return new WaitForSeconds(3);
                go.transform.GetChild(4).gameObject.SetActive(false);
            }
            else if (tarNum == 3)
            {
                go.transform.GetChild(4).SetPositionAndRotation(new Vector3(7, 0, 4), Quaternion.identity);
                go.transform.GetChild(4).gameObject.SetActive(true);
                yield return new WaitForSeconds(3);
                go.transform.GetChild(4).gameObject.SetActive(false);
            }
        }
        else if (CardName == "Poke")
        {
            if (tarNum == 1)
            {
                go.transform.GetChild(5).SetPositionAndRotation(new Vector3(1, 0, 4), Quaternion.identity);
                go.transform.GetChild(5).gameObject.SetActive(true);
                yield return new WaitForSeconds(3);
                go.transform.GetChild(5).gameObject.SetActive(false);
            }
            else if (tarNum == 2)
            {
                go.transform.GetChild(5).SetPositionAndRotation(new Vector3(4, 0.5f, 4), Quaternion.identity);
                go.transform.GetChild(5).gameObject.SetActive(true);
                yield return new WaitForSeconds(3);
                go.transform.GetChild(5).gameObject.SetActive(false);
            }
            else if (tarNum == 3)
            {
                go.transform.GetChild(5).SetPositionAndRotation(new Vector3(7, 0, 4), Quaternion.identity);
                go.transform.GetChild(5).gameObject.SetActive(true);
                yield return new WaitForSeconds(3);
                go.transform.GetChild(5).gameObject.SetActive(false);
            }
        }
        else if (CardName == "Potion")
        {
            go.transform.GetChild(6).gameObject.SetActive(true);
            yield return new WaitForSeconds(3);
            go.transform.GetChild(6).gameObject.SetActive(false);            
        }
        else if (CardName == "Lucky")
        {
            int endTurn = GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount + 4;
            go.transform.GetChild(7).gameObject.SetActive(true);
            yield return new WaitUntil(() =>
                GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount == endTurn);
            go.transform.GetChild(7).gameObject.SetActive(false);                
        }
        else if (CardName == "Defensive")
        {
            int endTurn = GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount + 4;
            go.transform.GetChild(8).gameObject.SetActive(true);
            yield return new WaitUntil(() =>
                GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount ==
                endTurn);
            go.transform.GetChild(8).gameObject.SetActive(false);                
        }
        else if (CardName == "Sleight")
        {
            go.transform.GetChild(9).gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            go.transform.GetChild(9).gameObject.SetActive(false);                
        }
        else if (CardName == "Aggression")
        {
            int endTurn = GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount + 4;
            go.transform.GetChild(11).gameObject.SetActive(true);
            yield return new WaitUntil(() =>
                GameObject.Find("LevelManager").GetComponent<LevelManager>().turnCount ==
                endTurn);
            go.transform.GetChild(11).gameObject.SetActive(false);                
        }
        else if (CardName == "Haste")
        {
            go.transform.GetChild(12).gameObject.SetActive(true);
            yield return new WaitForSeconds(3);
            go.transform.GetChild(12).gameObject.SetActive(false); 
        }
        else if (CardName == "Crit")
        {
            go.transform.GetChild(14).gameObject.SetActive(true);
            yield return new WaitForSeconds(3);
            go.transform.GetChild(14).gameObject.SetActive(false); 
        }
    }
    
}
