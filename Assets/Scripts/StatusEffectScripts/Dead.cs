using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : StatusEffect
{
    public override void effect(CreatureCard card)
    {
        card.isAttackStopped = true;
        card.isBlockStopped = true;
    }
    public override void deffect(CreatureCard card)
    {
        card.isAttackStopped = false;
        card.isBlockStopped = false;
    }

}