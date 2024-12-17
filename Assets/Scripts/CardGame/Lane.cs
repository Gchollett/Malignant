using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{   

    public GameObject protagCreature {get; set;}
    public GameObject antagCreature {get; set;}

    public Transform playerPt { get; private set; }
    public Transform enemyPt { get; private set; }
    // Start is called before the first frame update

    private void Start() {
        playerPt = transform.GetChild(0);
        enemyPt = transform.GetChild(1);
    }

    public void addAntagCreature(GameObject card)
    {
        if(antagCreature) return;
        antagCreature = card;
        card.transform.position = enemyPt.position;
    }

    public void addProtagCreature(GameObject card)
    {
        if(protagCreature) return;
        protagCreature = card;
        card.transform.position = playerPt.position;
    }

    public bool removeFromLane(GameObject card){
        if(card == protagCreature){
            protagCreature = null;
            return true;
        }
        else if(card == antagCreature){
            antagCreature = null;
            return true;
        }else{
            return false;
        }
    }

    public bool alreadyHasCard() {
        return protagCreature;
    }
}
