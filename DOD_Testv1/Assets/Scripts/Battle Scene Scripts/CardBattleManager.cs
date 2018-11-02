using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Guirao.UltimateTextDamage;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardBattleManager : MonoBehaviour {

    public CardBattleManager instance;

    public GameObject cardPrefab;

    public Deck deck;

    public Canvas hand;

    public List<Card> inUseDeck;
    
    public List<Character> charOrder;

    public static int deadEnemies = 0;

    public static bool enemyTurn = false;

    public static bool draw1card;

    public GameObject dragBlock;

    public GameObject CombatLog;

    public GameObject playerGO;

    private int emptyDeckDmg;

    private void Start()
    {
        dragBlock = GameObject.Find("Hand Block");
        Dragable.playerTurn = false;
        for (int j = 0; j < 25; j++)
        {
            inUseDeck.Add(deck.cards[j]);
            inUseDeck.Add(deck.cards[j]);
        }
        for (int i = 0; i < 5; i++){
            GameObject go = Instantiate(cardPrefab) as GameObject;
            CardDisplay display = go.GetComponent<CardDisplay>();
            Dragable dragable = go.GetComponent<Dragable>();
            int randomSelectedCard = Random.Range(0, inUseDeck.Count);
            display.CardSetup(inUseDeck[randomSelectedCard]);
            inUseDeck.RemoveAt(randomSelectedCard);
            display.transform.SetParent(hand.transform.GetChild(0), false);
            dragable.parentToReturnTo = display.transform.parent;
            dragable.placeholderParent = display.transform.parent;
            dragable.targets = display.cardTargets;
        }
        emptyDeckDmg = 1;
    }
    
    private void Update()
    {
        if (draw1card)
        {
            drawACard();
            draw1card = false;
        }

        if (Dragable.playerTurn)
        {
            dragBlock.SetActive(false);
        }
        else
        {
            dragBlock.SetActive(true);
        }
    }

    public void drawACard()
    {
        if (inUseDeck.Count != 0){
            if (hand.transform.GetChild(0).childCount < 5)
            {
                GameObject go = Instantiate(cardPrefab) as GameObject;
                CardDisplay display = go.GetComponent<CardDisplay>();
                Dragable dragable = go.GetComponent<Dragable>();
                int randomSelectedCard = Random.Range(0, inUseDeck.Count);
                display.CardSetup(inUseDeck[randomSelectedCard]);
                inUseDeck.RemoveAt(randomSelectedCard);
                display.transform.SetParent(hand.transform.GetChild(0), false);
                dragable.parentToReturnTo = display.transform.parent;
                dragable.placeholderParent = display.transform.parent;
                dragable.targets = display.cardTargets;
            }
            else
            {
                CombatLog.GetComponent<CombatLogPopulate>()
                    .populate("Overdrawn. You're hand was full, no additional card drawn.", false);
            }
        }
        else
        {
            playerGO = GameObject.Find("Player");
            CombatLog.GetComponent<CombatLogPopulate>()
                .populate("You have run out of cards. Take " + emptyDeckDmg + " damage.", false);
            playerGO.GetComponent<CharacterStateMachine>().character.health -= emptyDeckDmg;
            emptyDeckDmg++;
        }
    }
}
