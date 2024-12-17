using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Hunter : MonoBehaviour
{
    public GameObject DeckCardPrefab;
    public Transform DeckContent;
    public Button powerButton;
    public Button healthButton;
    public CardHolder powerLane;
    public CardHolder healthLane;
    public CardHolder chosenLane;
    public float reusePercent = .5f;
    public Canvas canvas;
    private DataManager dm;
    public List<CardData> deck {get;set;} = new List<CardData>();
    void Start()
    {
        dm = DataManager.Instance;
        deck = new List<CardData>(dm.Deck);
        DisplayDeck();
    }
    private void FixedUpdate() {
        if(deck.Count != DeckContent.childCount){
            FixDeck();
        }
        if(!chosenLane.card){
            healthButton.enabled = false;
            powerButton.enabled = false;
        }else{
            healthButton.enabled = true;
            powerButton.enabled = true;
        }
        if(!healthLane.card){
            healthButton.enabled = false;
        }
        if(!powerLane){
            powerButton.enabled = false;
        }
    }
    private void DisplayDeck()
    {    
        foreach(CardData card in deck){
            GameObject new_card = Instantiate(DeckCardPrefab,DeckContent);
            new_card.GetComponent<Card>().cardData = card;
            CardDraggable cd = new_card.AddComponent<CardDraggable>();
            cd.card = card;
            cd.hunter = this;
            new_card.transform.localScale = Vector2.one*2;
        }
    }
    private void FixDeck()
    {
        for(int i = 0; i<DeckContent.childCount; i++){
                Destroy(DeckContent.GetChild(i).gameObject);
        }
        DisplayDeck();
    }


    public void GivePower()
    {
        chosenLane.card.power += powerLane.card.power;
        dm.Deck.Remove(powerLane.card);
        Destroy(powerLane.transform.GetChild(0).gameObject);
        powerLane.card = null;
        if(Random.Range(0f,1f) < reusePercent){
            reusePercent /= 2;
        }else{
            Okay();
        }
    }

    
    public void GiveHealth()
    {
        chosenLane.card.health += healthLane.card.health;
        dm.Deck.Remove(healthLane.card);
        Destroy(healthLane.transform.GetChild(0).gameObject);
        healthLane.card = null;
        if(Random.Range(0f,1f) < reusePercent){
            reusePercent /= 2;
        }else{
            Okay();
        }
    }

    public void Okay()
    {
        SceneManager.LoadScene("Overworld");
    }
}