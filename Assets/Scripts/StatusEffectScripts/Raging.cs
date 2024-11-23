using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raging : StatusEffect
{
    public override void effect(CreatureCard card)
    {
        card.tempPower+=2;
        card.tempHealth-=1;
    }
    public override void deffect(CreatureCard card)
    {
        card.tempPower-=2;
        card.tempHealth+=1;
    }

}