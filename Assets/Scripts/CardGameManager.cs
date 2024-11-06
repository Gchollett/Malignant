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
    public bool isDrawEnabled {get; private set;}
    public bool isMoveEnabled {get; private set;}
    public bool isSacrificeEnabled {get; private set;}
    public bool isActivationEnabled {get; private set;}
    public bool isWaiting {get; set;}

    class Node{
        Phase val;
        Node next;

        Node(Phase v = Phase.Start, Node n = null){
            val = v;
            next = n;
        }
    }
    
    public enum Phase {
        Start = 0,
        Play = 1,
        Activation = 2,
        Combat = 3
    }


    void Awake()
    {
        if(!Instance) Instance = this;
    }

    void Start()
    {
        phase = Phase.Start;
        isWaiting = false;
        activePlayer = protag;
    }

    void FixedUpdate(){
        mainGameLoop();
        Debug.Log($"Phase:{phase}\nActive Player:{activePlayer}\nProtag Pips:{protag.pips}\nAntag pips:{antag.pips}");
    }

    public void changeActivePlayer(){
        if(activePlayer == protag) activePlayer = antag;
        else activePlayer = protag;
    }

    public void changePhase(){
        phase++;
        if(phase > Phase.Combat){
            phase = Phase.Start;
        }
    }

    void mainGameLoop()
    {
        if(phase == Phase.Start && !isWaiting){
            cleanBoard();
            gainPips();
            isWaiting = true;
            isDrawEnabled = true;
        }else if(phase == Phase.Play && !isWaiting){
            isDrawEnabled = false;
            isWaiting = true;
            if(activePlayer == protag){
                isMoveEnabled = true;
                isSacrificeEnabled = true;
            }else{
                isMoveEnabled = false;
                isSacrificeEnabled = false;
                antagMove();
            }
        }else if(phase == Phase.Activation && !isWaiting){
            isWaiting = true;
            if(activePlayer == protag){
                isActivationEnabled = true;
            }else{
                isActivationEnabled = false;
                antagActivate();
            }
        }else if(phase == Phase.Combat && !isWaiting){
            isWaiting = true;
            combat();
        }
    }
    //Start Phase Methods
        //ENABLE DRAW
    void cleanBoard(){
        foreach(Lane lane in lanes){
            lane.protagCreature?.GetComponent<CreatureCard>().UpdateCard();
            lane.antagCreature?.GetComponent<CreatureCard>().UpdateCard();
        }
    }
    void gainPips(){
        protag.upPips();
        antag.upPips();
    }
    //Play Phase Methods
        //DISABLE DRAW
        //ENABLE MOVE
        //ENABLE SACRICE
    void antagMove(){

    }
    //Activation Phase Methods
        //DISABLE MOVE
        //DISABLE SACRIFICE
        //ENABLE ACTIVATIONS
    void antagActivate(){

    }
    //Combat Phase Methods
        //DISABLE ACTIVATIONS
    void combat(){

    }
}
