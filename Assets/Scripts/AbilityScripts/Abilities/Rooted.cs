using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooted : PassiveAbility
{
    void Start()
    {
        staticAction();
    }
    public override void staticAction()
    {
        owner.canBlockDirect = true;
    }
}
