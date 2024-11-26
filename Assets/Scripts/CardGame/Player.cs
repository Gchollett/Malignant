using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected GameObject CardPrefab;
    public int health {get; private set;}
    public int startingHealth {get;} = 10;
    public int pips {get; private set;}
    public List<CardData> Hand;

    void Start() {
        CardPrefab = Resources.Load<GameObject>("Prefabs/CreatureCard");
        health = startingHealth;
        pips = 0;
    }

    public void upPips(int count = 1){
        pips += count;
    }

    public int lowerPips(int count = 1){
        if(pips < count) {
            int ret = pips;
            pips = 0;
            return ret;
        }
        else {
            pips -= count;
            return count;
        }
    }

    public void damage(int d){
        health -= d;
    }
}
