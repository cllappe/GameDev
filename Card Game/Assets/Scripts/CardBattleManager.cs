using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBattleManager : MonoBehaviour {

    public CardBattleManager instance;

    public GameObject cardPrefab;

    public Card[] cards;

    public Vector3[] spawnPoints;

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
        for (int i = 0; i < spawnPoints.Length; i++){
            GameObject go = Instantiate(cardPrefab, spawnPoints[i], Quaternion.identity);
            CardDisplay display = go.GetComponent<CardDisplay>();
            display.CardSetup(cards[Random.Range(0, cards.Length)]);
        }
    }
}
