using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDead : ActivatedAbility
{
    StatusEffect DeadPrefab;
    void Start()
    {
        DeadPrefab = Resources.Load<StatusEffect>("Prefabs/StatusEffects/Dead");
    }
    public override void activatedAction()
    {
        owner.applyStatusEffect(DeadPrefab,1);
    }
}