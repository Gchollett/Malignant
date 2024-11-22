using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Honk : ActivatedAbility
{
    private StatusEffect YokedPrefab;
    void Start(){
        YokedPrefab = Resources.Load<StatusEffect>("Prefabs/StatusEffects/Yoked");
    }
    public override void activatedAction()
    {
        for(int i = 0; i< gm.lanes.Count(); i++){
            gm.lanes[i].protagCreature.GetComponent<CreatureCard>().applyStatusEffect(YokedPrefab,1);
        }
    }
}