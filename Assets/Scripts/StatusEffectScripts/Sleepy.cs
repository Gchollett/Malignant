using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleepy : StatusEffect
{
    public override void effect(Card card)
    {
        card.isAttackStopped = true;
        card.isAbilitiesStopped = true;
    }
    public override void deffect(Card card)
    {
        card.isAttackStopped = false;
        card.isAbilitiesStopped = false;
    }

}