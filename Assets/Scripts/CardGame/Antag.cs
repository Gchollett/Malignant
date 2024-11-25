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
}