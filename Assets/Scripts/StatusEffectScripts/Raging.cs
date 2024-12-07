using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raging : StatusEffect
{
    public override void effect(Card card)
    {
        card.tempPower+=2;
        card.tempHealth-=1;
    }
    public override void deffect(Card card)
    {
        card.tempPower-=2;
        card.tempHealth+=1;
    }

}