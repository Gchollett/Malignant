using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hasten : ActivatedAbility
{
    public override void activatedAction()
    {
        owner.extraAttackCounter += 1;
    }
}