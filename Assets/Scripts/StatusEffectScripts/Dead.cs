using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : StatusEffect
{
    public override void effect(Card card)
    {
        card.isAttackStopped = true;
        card.isBlockStopped = true;
    }
    public override void deffect(Card card)
    {
        card.isAttackStopped = false;
        card.isBlockStopped = false;
    }

}