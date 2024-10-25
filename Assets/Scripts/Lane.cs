using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{   
    public bool hasCard = false;
    // Start is called before the first frame update

    public void cardInLane(){
        Debug.Log($"cardPlayedInLane played in gameObject {gameObject.name}");
        hasCard = true;
    }

    public bool alreadyHasCard() {
        return hasCard;
    }

    public void cardRemovedFromLane(){
        Debug.Log($"cardRemovedFromLane played in gameObject {gameObject.name}");
        hasCard = false;
    }
}
