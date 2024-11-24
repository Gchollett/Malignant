using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frightened : StatusEffect
{
    public override void effect(CreatureCard card)
    {
        card.isBlockStopped = true;
        card.isAttackStopped = true;
    }
    public override void deffect(CreatureCard card)
    {
        card.isBlockStopped = false;
        card.isAttackStopped = false;
    }

}