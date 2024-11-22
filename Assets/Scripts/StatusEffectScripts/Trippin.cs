using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trippin : StatusEffect
{
    public override void effect(CreatureCard card)
    {
        card.isAttackStopped = true;
    }
    public override void deffect(CreatureCard card)
    {
        card.isAttackStopped = false;
    }

}