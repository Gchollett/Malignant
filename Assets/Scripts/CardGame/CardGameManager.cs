using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardGameManager : MonoBehaviour
{
    public static CardGameManager Instance;
    public Texture2D sacCursor;
    public Protag protag;
    public Antag antag;
    private Player activePlayer;
    public Lane[] lanes;
    public Button button;
    [SerializeField] private Deck deck;
    public Phase phase {get; private set;}
    public bool isDrawEnabled {get; private set;}
    public bool isMoveEnabled {get; private set;}
    public bool isSacrificeEnabled {get; private set;}
    public bool isActivationEnabled {get; private set;}
    public bool isWaiting {get; set;}
    private DataManager dm;
    private MapManager mm;
    private OverworldPlayer op;
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
    private Vector3 deckStartPos;


    void Awake()
    {
        if(!Instance) Instance = this;
    }

    void Start()
    {
        mm = MapManager.Instance;
        op = OverworldPlayer.Instance;
        phase = Phase.Start;
        isWaiting = false;
        activePlayer = protag;
        dm = DataManager.Instance;
        if(dm){
            List<CardData> cards = new List<CardData>(dm.EnemyHand.Hand);
            antag.Hand = new List<CardData>(cards);
            protag.upPips(dm.startingPips);
        }
        deck.shuffle();
        int drawAmount = Math.Min(3,deck.cards.Count);
        for(int i = 0; i < drawAmount;i++){
            protag.AddGameCard(deck.draw(),-1);
        }
    }

    void FixedUpdate(){
        mainGameLoop();
        if(phase == Phase.Activation){
            foreach(Lane lane in lanes){
                lane.protagCreature?.GetComponent<Card>().CheckIfDead();
                lane.antagCreature?.GetComponent<Card>().CheckIfDead();
            }
        }
        protagHealth.text = protag.health.ToString();
        antagHealth.text = antag.health.ToString();
        protagPips.text = $"Pips: {protag.pips}";
        antagPips.text = $"Pips: {antag.pips}";
    }

    private void checkWin()
    {
        if(antag.health <= 0 && protag.health >= antag.health){
            dm.money += protag.pips;
            if(dm.isMonolith){
                dm.startingPips+=1;
                dm.isMonolith = false;
            }
            SceneManager.LoadScene("Overworld");
        }
        if(protag.health <= 0 && protag.health < antag.health){
            Destroy(dm.gameObject);
            Destroy(mm.gameObject);
            Destroy(op.gameObject);
            SceneManager.LoadScene(0);
        }
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

    public void disableSacrifice(){
        isSacrificeEnabled = false;
    }

    IEnumerator setButtonText(String text){
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        foreach(char c in text){
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text += c;
            yield return null;
        }
    }

    void mainGameLoop()
    {
        if(phase == Phase.Start && !isWaiting){
            StartCoroutine(StartPhase());
        }else if(phase == Phase.Play && !isWaiting){
            isDrawEnabled = false;
            isWaiting = true;
            if(activePlayer == protag){
                button.gameObject.SetActive(true);
                StartCoroutine(setButtonText("Done Playing?"));
                isMoveEnabled = true;
                isSacrificeEnabled = true;
                foreach(Lane lane in lanes){
                    if(lane.protagCreature) lane.protagCreature.GetComponent<HoverCursor>().cursor = sacCursor;
                }
            }else{
                button.gameObject.SetActive(false);
                isMoveEnabled = false;
                isSacrificeEnabled = false;
                foreach(Lane lane in lanes){
                    if(lane.protagCreature) lane.protagCreature.GetComponent<HoverCursor>().cursor = null;
                }
                StartCoroutine(playThenSwitch());
            }
        }else if(phase == Phase.Activation && !isWaiting){
            isWaiting = true;
            if(activePlayer == protag){
                button.gameObject.SetActive(true);
                StartCoroutine(setButtonText("Done Activating?"));
                isActivationEnabled = true;
                foreach(Lane lane in lanes){
                    if(lane.protagCreature && !lane.protagCreature.GetComponent<Card>().isAbilitiesStopped){
                        List<Button> abilityButtons = lane.protagCreature.GetComponent<CardText>().abilityButtons;
                        for(int i = 0; i < 3; i++){
                            abilityButtons[i].interactable = true;
                            int index = i;
                            abilityButtons[i].onClick.AddListener(() => {
                                if(lane.protagCreature.gameObject.GetComponent<Card>().abilities[index].ProcessAbility(protag.pips)){
                                    protag.lowerPips(((ActivatedAbility)lane.protagCreature.gameObject.GetComponent<Card>().abilities[index]).cost);
                                }
                                checkWin();
                            });
                        }
                    }
                }
            }else{
                foreach(Lane lane in lanes){
                    if(lane.protagCreature){
                        List<Button> abilityButtons = lane.protagCreature.GetComponent<CardText>().abilityButtons;
                        for(int i = 0; i < 3; i++){
                            abilityButtons[i].interactable = false;
                            abilityButtons[i].onClick.RemoveAllListeners();
                        }
                    }
                }
                button.gameObject.SetActive(false);
                isActivationEnabled = false;
                antag.activateAbilities(lanes);
                changePhase();
                changeActivePlayer();
            }
        }else if(phase == Phase.Combat && !isWaiting){
            isWaiting = true;
            StartCoroutine(combat());
        }else if(phase == Phase.End && !isWaiting){
            cleanBoard();
            checkWin();
            changePhase();
        }
    }
    //Start Phase Methods
        //ENABLE DRAW
    void gainPips(){
        protag.upPips();
        antag.upPips();
    }

    private IEnumerator StartPhase()
    {
        button.gameObject.SetActive(false);
        isWaiting = true;
        isDrawEnabled = true;
        foreach(Lane lane in lanes){
            lane.protagCreature?.GetComponent<Card>().ActivateTrigger(Triggers.OnStart);
            lane.antagCreature?.GetComponent<Card>().ActivateTrigger(Triggers.OnStart);
        }
        yield return new WaitForEndOfFrame();
        gainPips();
        yield return new WaitForEndOfFrame();
        if(deck.cards.Count >0) deck.GetComponent<Animator>().SetBool("Drawing",true);
        else changePhase();
    }
    //Play Phase Methods
        //DISABLE DRAW
        //ENABLE MOVE
        //ENABLE SACRICE
        IEnumerator playThenSwitch()
        {
            
                antag.MakePlay(lanes);
                yield return new WaitForFixedUpdate();
                changePhase();
                changeActivePlayer();
        }
    //Activation Phase Methods
        //DISABLE MOVE
        //DISABLE SACRIFICE
        //ENABLE ACTIVATIONS
    //Combat Phase Methods
        //DISABLE ACTIVATIONS
    private IEnumerator combat(){
        foreach(Lane lane in lanes){
            if(lane.protagCreature){
                for(int i = 0; i <= lane.protagCreature.GetComponent<Card>().extraAttackCounter; i++){
                    if(lane.protagCreature.GetComponent<Card>().isAttackStopped) break;
                    lane.protagCreature.GetComponent<Animator>().SetBool("PlayerAttacking",true);
                    yield return new WaitUntil(() => lane.protagCreature.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime<1f);
                    if(lane.antagCreature && !lane.antagCreature.GetComponent<Card>().isBlockStopped && (!lane.protagCreature.GetComponent<Card>().isDealingDirect || lane.antagCreature.GetComponent<Card>().canBlockDirect)){
                        lane.protagCreature.GetComponent<Card>().attack(lane.antagCreature.GetComponent<Card>());
                        lane.protagCreature?.GetComponent<Card>().ActivateTrigger(Triggers.OnDealingDamage);
                        lane.antagCreature?.GetComponent<Card>().ActivateTrigger(Triggers.OnTakingDamage);
                    }
                    else{
                        lane.protagCreature.GetComponent<Card>().attack(antag);
                        lane.protagCreature.GetComponent<Card>().ActivateTrigger(Triggers.OnDamagingPlayer);
                    }
                    lane.protagCreature.GetComponent<Card>().ActivateTrigger(Triggers.OnAttack);
                    lane.protagCreature.GetComponent<Animator>().SetBool("PlayerAttacking",false);
                }
                lane.protagCreature.GetComponent<Card>().extraAttackCounter = 0;
            }
            if(lane.antagCreature){
                for(int i = 0; i <= lane.antagCreature.GetComponent<Card>().extraAttackCounter; i++){
                    if(lane.antagCreature.GetComponent<Card>().isAttackStopped) break;
                    lane.antagCreature.GetComponent<Animator>().SetBool("EnemyAttacking",true);
                    yield return new WaitUntil(() => lane.antagCreature.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime<1f);
                    if(lane.protagCreature && !lane.protagCreature.GetComponent<Card>().isBlockStopped && (!lane.antagCreature.GetComponent<Card>().isDealingDirect || lane.protagCreature.GetComponent<Card>().canBlockDirect)){
                        lane.antagCreature.GetComponent<Card>().attack(lane.protagCreature.GetComponent<Card>());
                        lane.antagCreature?.GetComponent<Card>().ActivateTrigger(Triggers.OnDealingDamage);
                        lane.protagCreature?.GetComponent<Card>().ActivateTrigger(Triggers.OnTakingDamage);
                    }
                    else{
                        lane.antagCreature.GetComponent<Card>().attack(protag);
                        lane.antagCreature.GetComponent<Card>().ActivateTrigger(Triggers.OnDamagingPlayer);
                    }
                    lane.antagCreature.GetComponent<Card>().ActivateTrigger(Triggers.OnAttack);
                    lane.antagCreature.GetComponent<Animator>().SetBool("EnemyAttacking",false);
                }
                lane.antagCreature.GetComponent<Card>().extraAttackCounter = 0;
            }
        }
        changePhase();
    }
    //End Phase Methods
    void cleanBoard(){
        foreach(Lane lane in lanes){
            lane.protagCreature?.GetComponent<Card>().CheckIfDead();
            lane.antagCreature?.GetComponent<Card>().CheckIfDead();
            lane.protagCreature?.GetComponent<Card>().UpdateCard();
            lane.antagCreature?.GetComponent<Card>().UpdateCard();
            lane.protagCreature?.GetComponent<Card>().ActivateTrigger(Triggers.OnEnd);
            lane.antagCreature?.GetComponent<Card>().ActivateTrigger(Triggers.OnEnd);
        }
    }
}
