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

    public Text turnMonitor;

    public Text DeckCounter;

    public static bool enemyTurn = false;

    public static bool draw1card;

    public GameObject dragBlock;

    private void Awake()
    {
        if (instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
       
    }

    private void Start()
    {
        dragBlock = GameObject.Find("Hand Block");
        turnMonitor = gameObject.GetComponent<Text>();
        Dragable.playerTurn = false;
        for (int j = 0; j < 25; j++)
        {
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
    }
    private void Update()
    {
        DeckCounter.text = "Remaining Cards =" + inUseDeck.Count;
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

    public void setTurnText(String thisText)
    {
        turnMonitor.text = thisText;
    }

    public void drawACard()
    {
        if (hand.transform.GetChild(0).childCount < 5){
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
            int randomSelectedCard = Random.Range(0, inUseDeck.Count);
            inUseDeck.RemoveAt(randomSelectedCard);
            GameObject PlayerUTD = GameObject.Find("PlayerUTD");
            PlayerUTD.GetComponent<UltimateTextDamageManager>().Add("Over Drawn. Lost a Card.",PlayerUTD.transform,"status");
        }
    }
}
