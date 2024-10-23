using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelly : TriggeredAbility
{
    public override bool trigger(string val){
        return val == "Died";
    }

    public override void triggeredAction()
    {
        
    }
}
