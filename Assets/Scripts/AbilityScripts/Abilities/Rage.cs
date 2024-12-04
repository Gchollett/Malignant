using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rage : ActivatedAbility
{
    private StatusEffect RagingPrefab;
    void Start(){
        RagingPrefab = Resources.Load<StatusEffect>("Prefabs/StatusEffects/Raging");
    }
    public override void activatedAction()
    {
        owner.applyStatusEffect(RagingPrefab,1);
    }
}