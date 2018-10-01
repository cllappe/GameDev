using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardBattleManager : MonoBehaviour {

    public CardBattleManager instance;

    public GameObject cardPrefab;

    public Deck deck;

    public Canvas hand;

    public List<Card> inUseDeck;
    
    public static List<Character> charOrder = new List<Character>();

    public static int deadEnemies = 0;

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
        
        if (hand.transform.GetChild(0).childCount < 5 && !Dragable.dragging){
            GameObject go = Instantiate(cardPrefab) as GameObject;
            CardDisplay display = go.GetComponent<CardDisplay>();
            Dragable dragable = go.GetComponent<Dragable>();
            int randomSelectedCard = Random.Range(0, inUseDeck.Count);
            Debug.Log(inUseDeck.Count);
            display.CardSetup(inUseDeck[randomSelectedCard]);
            inUseDeck.RemoveAt(randomSelectedCard);
            display.transform.SetParent(hand.transform.GetChild(0), false);
            dragable.parentToReturnTo = display.transform.parent;
            dragable.placeholderParent = display.transform.parent;
        }
    }
}
