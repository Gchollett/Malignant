using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scare : ActivatedAbility
{
    StatusEffect FrightenedPrefab;
    void Start()
    {
        FrightenedPrefab = Resources.Load<StatusEffect>("/Prefabs/StatusEffects/Frightened");
    }
    public override void activatedAction()
    {
        Lane lane = owner.lane.GetComponent<Lane>();
        if(owner.status==CardStatus.Protags) lane.antagCreature?.GetComponent<CreatureCard>().applyStatusEffect(FrightenedPrefab,2);
        else if(owner.status==CardStatus.Antags) lane.protagCreature?.GetComponent<CreatureCard>().applyStatusEffect(FrightenedPrefab,2);
    }
}