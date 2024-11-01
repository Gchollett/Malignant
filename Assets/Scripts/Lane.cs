using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class Lane : MonoBehaviour
{   
    public bool hasCard = false;
    public Vector2 playerPt { get; private set; }
    public Vector2 enemyPt { get; private set; }
    // Start is called before the first frame update

    private void Start() {
        playerPt = transform.GetChild(0).transform.position;
        enemyPt = transform.GetChild(1).transform.position;
    }

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
