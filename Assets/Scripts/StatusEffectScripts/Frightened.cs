using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frightened : StatusEffect
{
    public override void effect(Card card)
    {
        card.isBlockStopped = true;
        card.isAttackStopped = true;
    }
    public override void deffect(Card card)
    {
        card.isBlockStopped = false;
        card.isAttackStopped = false;
    }

}