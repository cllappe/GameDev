using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour {

    [HideInInspector]
    public Card card;
    [HideInInspector]
    public Targets cardTargets;
    [HideInInspector] 
    public Type cardType;

    public Text discriptionText;
    public Text nameText;

    public Image cardImage;
    public int damage;

    public int numberOfTargets;

    public int heal;

    public int turnIncrease;

    public int drawCards;
    public void CardSetup(Card thisCard){
        card = thisCard;

        nameText.text = card.cardName;
        discriptionText.text = card.discription;
        cardType = card.type;
        cardImage.sprite = card.art;
        cardTargets = card.targets;
        damage = card.damage;
        numberOfTargets = card.numberOfTargets;
        heal = card.heal;
        turnIncrease = card.playableAmountIncrease;
        drawCards = card.draw;
    }
}
