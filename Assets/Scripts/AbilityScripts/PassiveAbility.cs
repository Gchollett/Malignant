using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAbility : Ability
{
    public abstract void staticAction();

    public override bool ProcessAbility()
    {
        staticAction();
        return true;
    }

    public override bool ProcessAbility(int pips)
    {
        return false;
    }
}
