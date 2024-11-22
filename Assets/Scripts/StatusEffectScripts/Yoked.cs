using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yoked : StatusEffect
{
    public override void effect(CreatureCard card)
    {
        card.tempPower+=1;
    }
    public override void deffect(CreatureCard card)
    {
        card.tempPower-=1;
    }

}