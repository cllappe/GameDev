using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour {

    [HideInInspector]
    public Card card;
    [HideInInspector]
    public Targets cardTargets;

    public Text discriptionText;
    public Text nameText;

    public Image cardImage;
    public int damage;

    public int numberOfTargets;

    public void CardSetup(Card thisCard){
        card = thisCard;

        nameText.text = card.cardName;
        discriptionText.text = card.discription;

        cardImage.sprite = card.art;
        cardTargets = card.targets;
        damage = card.damage;
        numberOfTargets = card.numberOfTargets;
    }
}
