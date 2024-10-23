using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private int health = 10;
    private int pips = 0;
    public Creature[] Lanes;
    public GameObject[] Hand;
    private void FixedUpdate() {
        if(transform.childCount != Hand.Length){
            for(int i = 0; i < transform.childCount; i++){
                Destroy(transform.GetChild(i).gameObject);
            }
            for(int i = 0; i < Hand.Length; i++){
                GameObject card = Instantiate(Hand[i]);
                card.transform.parent = gameObject.transform;
                card.transform.position = gameObject.transform.position + new Vector3(2*(i-Hand.Length/2f+.5f),0,0);
            }
        }
    }
    public void damage(int d){
        health -= d;
    }
}
