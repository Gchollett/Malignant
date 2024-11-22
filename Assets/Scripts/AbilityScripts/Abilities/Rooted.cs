using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
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
