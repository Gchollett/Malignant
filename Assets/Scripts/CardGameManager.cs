using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGameManager : MonoBehaviour
{
    public static CardGameManager Instance;
    public int laneNumber;
    public Player protag;
    public Player antag;
    void Awake()
    {
        if(!Instance) Instance = this;
    }
    void Start()
    {
        protag.Lanes = new Creature[laneNumber];
        antag.Lanes = new Creature[laneNumber];
    }
    public bool PlaceCardInLane(Player p, int i, CreatureCard card){
        if (i >= laneNumber || p.Lanes[i] != null) return false;
        p.Lanes[i] = new Creature(card);
        return true;
    }

}
