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

    public bool turnOnReflect;
    public int numOfTurns;

    public int drawCards;

    public int luckUp;

    public float reduceDmgMod;
    public int dmgIncMod;
    public bool lifeSteal;

    public bool canCrit;
    public int critNumOfTargets;
    public int critHeal;
    public bool critLifeSteal;
    public int critNumOfTurns;
    public int critDmgAdd;
    public bool critSkip;
    public bool critEnemyCritNegate;
    
    
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
        turnOnReflect = card.reflects;
        numOfTurns = card.numOfTurns;
        luckUp = card.addLuck;
        reduceDmgMod = card.damageReducedBy;
        dmgIncMod = card.increaseDamage;
        lifeSteal = card.lifeSteal;

        canCrit = card.canCrit;
        critNumOfTargets = card.critNumOfTargets;
        critHeal = card.critHeal;
        critLifeSteal = card.critLifeSteal;
        critNumOfTurns = card.critNumOfTurns;
        critDmgAdd = card.critDmgAdd;
        critSkip = card.critSkip;
        critEnemyCritNegate = card.critEnemyCritNegate;
    }
}
