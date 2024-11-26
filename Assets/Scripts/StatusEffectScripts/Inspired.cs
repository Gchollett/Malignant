using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspired : StatusEffect
{
    public override void effect(Card card)
    {
        card.tempPower+=1;
    }
    public override void deffect(Card card)
    {
        card.tempPower-=1;
    }

}