using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleepy : StatusEffect
{
    public override void effect(CreatureCard card)
    {
        card.isAttackStopped = true;
        card.isAbilitiesStopped = true;
    }
    public override void deffect(CreatureCard card)
    {
        card.isAttackStopped = false;
        card.isAbilitiesStopped = false;
    }

}