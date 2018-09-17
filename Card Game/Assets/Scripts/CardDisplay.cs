using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour {

    [HideInInspector]
    public Card card;

    public Text discriptionText;
    public Text nameText;

    public Image cardImage;

    public void CardSetup(Card thisCard){
        card = thisCard;

        nameText.text = card.cardName;
        discriptionText.text = card.discription;

        cardImage.sprite = card.art;
    }
}
