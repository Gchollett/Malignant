using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Creature : CreatureCard
{
    

    public Creature(CreatureCard card){
        cardName = card.cardName;
        flavorText = card.flavorText;
        power = card.power;
        health = card.health;
        ab1 = card.ab1;
        ab2 = card.ab2;
        ab3 = card.ab3;
    }
}
