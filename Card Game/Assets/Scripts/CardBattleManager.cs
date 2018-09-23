using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBattleManager : MonoBehaviour {

    public CardBattleManager instance;

    public GameObject cardPrefab;

    public Deck deck;

    public Vector3[] spawnPoints;

    public Canvas hand;

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
        for (int i = 0; i < 5; i++){
            GameObject go = Instantiate(cardPrefab) as GameObject;
            CardDisplay display = go.GetComponent<CardDisplay>();
            Dragable dragable = go.GetComponent<Dragable>();
            display.CardSetup(deck.cards[Random.Range(0, 6)]);
            display.transform.SetParent(hand.transform.GetChild(0), false);
            dragable.parentToReturnTo = display.transform.parent;
            dragable.placeholderParent = display.transform.parent;


        }
    }
    private void Update()
    {
        
        if (hand.transform.GetChild(0).childCount < 5 && !Input.GetMouseButton(0)){
            GameObject go = Instantiate(cardPrefab) as GameObject;
            CardDisplay display = go.GetComponent<CardDisplay>();
            Dragable dragable = go.GetComponent<Dragable>();
            display.CardSetup(deck.cards[Random.Range(0, 6)]);
            display.transform.SetParent(hand.transform.GetChild(0), false);
            dragable.parentToReturnTo = display.transform.parent;
            dragable.placeholderParent = display.transform.parent;
        }
    }
}
