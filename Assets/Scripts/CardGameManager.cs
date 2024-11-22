using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public TextMeshProUGUI protagHealth;
    public TextMeshProUGUI antagHealth;
    public TextMeshProUGUI protagPips;
    public TextMeshProUGUI antagPips;
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
        protagHealth.text = protag.health.ToString();
        antagHealth.text = antag.health.ToString();
        protagPips.text = $"Pips: {protag.pips}";
        antagPips.text = $"Pips: {antag.pips}";
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
                foreach(Lane lane in lanes){
                    if(lane.protagCreature){
                        List<Button> abilityButtons = new List<Button>
                        {
                            lane.protagCreature.GetComponent<CreatureCard>().abilityButton1,
                            lane.protagCreature.GetComponent<CreatureCard>().abilityButton2,
                            lane.protagCreature.GetComponent<CreatureCard>().abilityButton3
                        };
                        for(int i = 0; i < 3; i++){
                            abilityButtons[i].interactable = true;
                            int index = i;
                            abilityButtons[i].onClick.AddListener(() => {
                                if(lane.protagCreature.gameObject.GetComponent<CreatureCard>().abilities[index].ProcessAbility(protag.pips)){
                                    protag.lowerPips(((ActivatedAbility)lane.protagCreature.gameObject.GetComponent<CreatureCard>().abilities[index]).cost);
                                }
                            });
                        }
                    }
                }
            }else{
                foreach(Lane lane in lanes){
                    if(lane.protagCreature){
                        List<Button> abilityButtons = new List<Button>
                        {
                            lane.protagCreature.GetComponent<CreatureCard>().abilityButton1,
                            lane.protagCreature.GetComponent<CreatureCard>().abilityButton2,
                            lane.protagCreature.GetComponent<CreatureCard>().abilityButton3
                        };
                        for(int i = 0; i < 3; i++){
                            abilityButtons[i].interactable = false;
                            abilityButtons[i].onClick.RemoveAllListeners();
                        }
                    }
                }
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

    void playCard(Lane lane){
        GameObject card  = antag.Hand[UnityEngine.Random.Range(0,antag.Hand.Count)];
        GameObject creature = Instantiate(card);
        creature.transform.SetParent(antag.transform);
        creature.GetComponent<CreatureCard>().lane = lane.gameObject;
        creature.GetComponent<CreatureCard>().status = CardStatus.Antags;
        lane.addAntagCreature(creature);
        antag.Hand.Remove(card);
    }
    void antagMove(){
        if(antag.Hand.Count != 0){
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
                    playCard(emptyLanes[UnityEngine.Random.Range(0,emptyLanes.Count)]);
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
            if(lane.protagCreature){
                for(int i = 0; i <= lane.protagCreature.GetComponent<CreatureCard>().extraAttackCounter; i++){
                    if(lane.protagCreature.GetComponent<CreatureCard>().isAttackStopped) break;
                    if(lane.antagCreature && (!lane.protagCreature.GetComponent<CreatureCard>().isDealingDirect || lane.antagCreature.GetComponent<CreatureCard>().canBlockDirect)){
                        lane.protagCreature.GetComponent<CreatureCard>().attack(lane.antagCreature.GetComponent<CreatureCard>());
                    }
                    else{
                        lane.protagCreature.GetComponent<CreatureCard>().attack(antag);
                    }
                    lane.protagCreature.GetComponent<CreatureCard>().ActivateTrigger(Triggers.OnAttack);
                }
                lane.protagCreature.GetComponent<CreatureCard>().extraAttackCounter = 0;
            }
            if(lane.antagCreature){
                for(int i = 0; i <= lane.antagCreature.GetComponent<CreatureCard>().extraAttackCounter; i++){
                    if(lane.antagCreature.GetComponent<CreatureCard>().isAttackStopped) break;
                    if(lane.protagCreature && (!lane.antagCreature.GetComponent<CreatureCard>().isDealingDirect || lane.protagCreature.GetComponent<CreatureCard>().canBlockDirect)){
                        lane.antagCreature.GetComponent<CreatureCard>().attack(lane.protagCreature.GetComponent<CreatureCard>());
                    }
                    else{
                        lane.antagCreature.GetComponent<CreatureCard>().attack(protag);
                    }
                    lane.antagCreature.GetComponent<CreatureCard>().ActivateTrigger(Triggers.OnAttack);
                }
                lane.antagCreature.GetComponent<CreatureCard>().extraAttackCounter = 0;
            }
        }
        changePhase();
    }
    //End Phase Methods
    void cleanBoard(){
        foreach(Lane lane in lanes){
            lane.protagCreature?.GetComponent<CreatureCard>().UpdateCard();
            lane.antagCreature?.GetComponent<CreatureCard>().UpdateCard();
            lane.protagCreature?.GetComponent<CreatureCard>().ActivateTrigger(Triggers.OnEnd);
            lane.antagCreature?.GetComponent<CreatureCard>().ActivateTrigger(Triggers.OnEnd);
        }
    }
}
