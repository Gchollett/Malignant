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
    public Button PowerButton;
    public Button HealthButton;
    public Canvas canvas;
    private DataManager dm;
    void Start()
    {
        dm = DataManager.Instance;
        DisplayDeck();
    }
    private void FixedUpdate() {
        if(dm.Deck.Count != DeckContent.childCount){
            FixDeck();
        }
    }
    private void DisplayDeck()
    {    
        foreach(CardData card in dm.Deck){
            GameObject new_card = Instantiate(DeckCardPrefab,DeckContent);
            new_card.GetComponent<Card>().cardData = card;
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

    public void Okay()
    {
        SceneManager.LoadScene("Overworld");
    }
}