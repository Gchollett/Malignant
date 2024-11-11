using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardGameManager : MonoBehaviour
{
    public static CardGameManager Instance;
    public Protag protag;
    public Player antag;
    private Player activePlayer;
    public Lane[] lanes;
    public Button button;
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
        Combat = 3,
        End = 4
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
        // Debug.Log($"Protag Health:{protag.health}\nAntag Health:{antag.health}\nProtag Pips:{protag.pips}\nAntag pips:{antag.pips}");
    }

    public void changeActivePlayer(){
        if(activePlayer == protag) activePlayer = antag;
        else activePlayer = protag;
        isWaiting = false;
    }

    public void changePhase(){
        phase++;
        if(phase > Phase.End){
            phase = Phase.Start;
        }
        isWaiting = false;
    }

    void mainGameLoop()
    {
        if(phase == Phase.Start && !isWaiting){
            button.gameObject.SetActive(false);
            gainPips();
            isWaiting = true;
            isDrawEnabled = true;
        }else if(phase == Phase.Play && !isWaiting){
            isDrawEnabled = false;
            isWaiting = true;
            if(activePlayer == protag){
            button.gameObject.SetActive(true);
                isMoveEnabled = true;
                isSacrificeEnabled = true;
            }else{
                button.gameObject.SetActive(false);
                isMoveEnabled = false;
                isSacrificeEnabled = false;
                antagMove();
            }
        }else if(phase == Phase.Activation && !isWaiting){
            isWaiting = true;
            if(activePlayer == protag){
                button.gameObject.SetActive(true);
                isActivationEnabled = true;
            }else{
                button.gameObject.SetActive(false);
                isActivationEnabled = false;
                antagActivate();
            }
        }else if(phase == Phase.Combat && !isWaiting){
            isWaiting = true;
            combat();
        }else if(phase == Phase.End && !isWaiting){
            cleanBoard();
            changePhase();
        }
    }
    //Start Phase Methods
        //ENABLE DRAW
    void gainPips(){
        protag.upPips();
        antag.upPips();
    }
    //Play Phase Methods
        //DISABLE DRAW
        //ENABLE MOVE
        //ENABLE SACRICE
    void antagMove(){
        if(antag.Hand.Count != 0){
            foreach(Lane lane in lanes){
                if(lane.protagCreature && !lane.antagCreature){
                    GameObject card  = antag.Hand[Random.Range(0,antag.Hand.Count-1)];
                    GameObject creature = Instantiate(card);
                    creature.transform.SetParent(antag.transform);
                    creature.GetComponent<CreatureCard>().lane = lane.gameObject;
                    lane.addAntagCreature(creature);
                    antag.Hand.Remove(card);
                    break;
                }
            }
        }
        changePhase();
        changeActivePlayer();
    }
    //Activation Phase Methods
        //DISABLE MOVE
        //DISABLE SACRIFICE
        //ENABLE ACTIVATIONS
    void antagActivate(){
        changePhase();
        changeActivePlayer();
    }
    //Combat Phase Methods
        //DISABLE ACTIVATIONS
    void combat(){
        foreach(Lane lane in lanes){
            if(lane.antagCreature) lane.protagCreature?.GetComponent<CreatureCard>().attack(lane.antagCreature.GetComponent<CreatureCard>());
            else lane.protagCreature?.GetComponent<CreatureCard>().attack(antag);
            if(lane.protagCreature) lane.antagCreature?.GetComponent<CreatureCard>().attack(lane.protagCreature.GetComponent<CreatureCard>());
            else lane.antagCreature?.GetComponent<CreatureCard>().attack(protag);
        }
        changePhase();
    }
    //End Phase Methods
    void cleanBoard(){
        foreach(Lane lane in lanes){
            lane.protagCreature?.GetComponent<CreatureCard>().UpdateCard();
            lane.antagCreature?.GetComponent<CreatureCard>().UpdateCard();
        }
    }
}
