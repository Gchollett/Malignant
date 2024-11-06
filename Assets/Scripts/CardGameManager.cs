using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGameManager : MonoBehaviour
{
    public static CardGameManager Instance;
    public Player protag;
    public Player antag;
    private Player activePlayer;
    public Lane[] lanes;
    public Phase phase {get; private set;}

    public enum Phase {
        Start,
        Play,
        Activation,
        Combat
    }

    void Awake()
    {
        if(!Instance) Instance = this;
    }

    void Start()
    {
        activePlayer = protag;
    }

    void changeActivePlayer(){
        if(activePlayer == protag) activePlayer = antag;
        else activePlayer = protag;
    }

    void changePhase(){
        phase++;
        if(phase > Phase.Combat){
            phase = Phase.Start;
        }
    }

    void mainGameLoop()
    {
        
    }
}
