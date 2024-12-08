using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggeredAbility : Ability
{
    public abstract void triggeredAction();
    public override bool ProcessAbility()
    {
        return false;
    }

    public override bool ProcessAbility(int pips)
    {
        return false;
    }
}
