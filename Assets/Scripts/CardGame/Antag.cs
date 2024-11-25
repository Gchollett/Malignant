using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Antag : Player {    
    void playCard(Lane lane){
        GameObject card  = Hand[Random.Range(0,Hand.Count)];
        GameObject creature = Instantiate(card,transform);
        lane.addAntagCreature(creature);
        creature.GetComponent<CreatureCard>().lane = lane.gameObject;
        creature.GetComponent<CreatureCard>().status = CardStatus.Antags;
        Hand.Remove(card);
    }
   public void MakePlay(Lane[] lanes){
        if(Hand.Count != 0){
            bool played = false;
            List<Lane> emptyLanes = new List<Lane>();
            foreach(Lane lane in lanes){
                if(!played && lane.protagCreature && !lane.antagCreature){
                    playCard(lane);
                    played = true;
                }else if(!lane.antagCreature){
                    emptyLanes.Add(lane);
                }
            }if(!played){
                if(emptyLanes.Count > 0){
                    playCard(emptyLanes[Random.Range(0,emptyLanes.Count)]);
                }
            }
        }
    }

    public void activateAbilities(Lane[] lanes)
    {
        foreach(Lane lane in lanes){ //Go through and activate debuffs
            if(lane.antagCreature && lane.antagCreature.GetComponent<CreatureCard>().abilities.Count > 0){
                CreatureCard creature = lane.antagCreature.GetComponent<CreatureCard>();
                foreach(Ability ab in creature.abilities){
                    float prob = 0.01f;
                    switch(ab){
                        case Scare:
                            prob = .05f;
                            if(pips >= ((ActivatedAbility)ab).cost+2) prob = .3f;
                            break;
                    }
                    if(Random.Range(0f,1f) > prob) ab.ProcessAbility(pips);
                }
            }            
        }
        foreach(Lane lane in lanes){ //Go through and activate buffs
            if(lane.antagCreature && lane.antagCreature.GetComponent<CreatureCard>().abilities.Count > 0){
                CreatureCard creature = lane.antagCreature.GetComponent<CreatureCard>();
                foreach(Ability ab in creature.abilities){
                    float prob = 0.01f;
                    switch(ab){
                        case Honk:
                            prob = .6f;
                            break;
                        case Hop:
                            prob = .7f;
                            break;                            
                        case Rage:
                            if(creature.getInGameHealth() > 1){
                                if(lane.protagCreature){
                                    CreatureCard protagCreature = lane.protagCreature.GetComponent<CreatureCard>();
                                    if(protagCreature.isBlockStopped || protagCreature.getInGamePower() < creature.getInGameHealth()-1 || protagCreature.getInGameHealth() <= creature.getInGamePower()+2){
                                        prob = .5f;
                                    }
                                }else{
                                    prob = .7f;
                                }
                            }
                            break;
                    }
                    if(Random.Range(0f,1f) > prob) ab.ProcessAbility(pips);
                }
            }            
        }
        foreach(Lane lane in lanes){ //Go through and activate Damage
            if(lane.antagCreature && lane.antagCreature.GetComponent<CreatureCard>().abilities.Count > 0){
                CreatureCard creature = lane.antagCreature.GetComponent<CreatureCard>();
                foreach(Ability ab in creature.abilities){
                    float prob = 0.01f;
                    switch(ab){
                        case Hasten:
                            if(lane.protagCreature){
                                if(creature.getInGamePower() * 2 >= lane.protagCreature.GetComponent<CreatureCard>().getInGameHealth()){
                                    prob = .7f;
                                }
                            }else{
                                if(creature.getInGamePower() > 1){
                                    prob = .8f;
                                }else if(creature.getInGamePower() == 1){
                                    prob = .2f;
                                }
                            }
                            break;
                        case Bonk:
                            if(lane.protagCreature && lane.protagCreature.GetComponent<CreatureCard>().getInGameHealth() <= creature.getInGamePower()*1.5){
                                prob = .5f;
                            }else{
                                prob = 0;
                            }
                            break;
                        case Snap:
                            if(lane.protagCreature){
                                prob = .6f;
                            }else{
                                prob = .2f;
                            }
                            break;
                    }
                    if(Random.Range(0f,1f) > prob) ab.ProcessAbility(pips);
                }
            }            
        }
        foreach(Lane lane in lanes){ //Go through and activate Misc
            if(lane.antagCreature && lane.antagCreature.GetComponent<CreatureCard>().abilities.Count > 0){
                CreatureCard creature = lane.antagCreature.GetComponent<CreatureCard>();
                foreach(Ability ab in creature.abilities){
                    float prob = 0.01f;
                    switch(ab){
                        case PlayDead:
                            if(lane.protagCreature){
                                CreatureCard protagCreature = lane.protagCreature.GetComponent<CreatureCard>();
                                foreach(Ability abi in creature.abilities){
                                    if((abi is Honk || abi is Jam || abi is Snap) && (protagCreature.getInGamePower() * (protagCreature.extraAttackCounter+1) >= creature.getInGameHealth() || protagCreature.hasAbility(typeof(Venomous))) && protagCreature.getInGamePower() < health/2){
                                        prob = .7f;
                                    }
                                }
                            }
                            break;
                    }
                    if(Random.Range(0f,1f) > prob) ab.ProcessAbility(pips);
                }
            }            
        }
    }
}