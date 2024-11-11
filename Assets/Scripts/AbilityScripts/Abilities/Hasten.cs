using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hasten : ActivatedAbility
{
    public override void activatedAction()
    {
        creature.addTrigger(CreatureCard.Trigger.OnAttack,(GameObject g) => {
            if(g.GetComponent<Player>())creature.attack(g.GetComponent<Player>());
            if(g.GetComponent<CreatureCard>())creature.attack(g.GetComponent<CreatureCard>());
        });
    }
}