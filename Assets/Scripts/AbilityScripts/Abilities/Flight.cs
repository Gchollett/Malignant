using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flight : PassiveAbility
{
    void Start()
    {
        staticAction();
    }
    public override void staticAction()
    {
        owner.isDealingDirect = true;
    }
}
