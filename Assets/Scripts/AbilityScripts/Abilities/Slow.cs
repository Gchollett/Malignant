using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : TriggeredAbility
{
    StatusEffect SlowedPrefab;
    void Start()
    {
        SlowedPrefab = Resources.Load<StatusEffect>("Prefabs/StatusEffects/Slowed");
        owner.staticTriggers.TryAdd(Triggers.OnAttack,new List<Action>());
        owner.staticTriggers[Triggers.OnAttack].Add(delegate () {triggeredAction();});
    }
    public override void triggeredAction()
    {
        owner.applyStatusEffect(SlowedPrefab,2);
    }
}