using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowed : StatusEffect
{
    public override void effect(Card card)
    {
        card.isAttackStopped = true;
    }
    public override void deffect(Card card)
    {
        card.isAttackStopped = false;
    }

}