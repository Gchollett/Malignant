using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TMPro;
using UnityEngine;

public class DeckMenu : MonoBehaviour
{
    public GameObject CardPrefab;
    public Transform DeckContent;
    public Canvas canvas;
    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI StumpsText;
    private DataManager dm;
    private MapManager mm;
    private bool active = false;
    void Start()
    {
        dm = DataManager.Instance;
        mm = MapManager.Instance;
    }
    private void FixedUpdate() {
        if(dm.Deck.Count != DeckContent.childCount){
            FixDeck();
        }
        MoneyText.text = $"${dm.money}";
        StumpsText.text = $"Stumps Etches: {mm.numVisited}";
    }
    private void DisplayDeck()
    {    
        foreach(CardData card in dm.Deck){
            GameObject new_card = Instantiate(CardPrefab,DeckContent);
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

    private void OnEnable() {
        if(!mm) mm = MapManager.Instance;
        mm.movingEnabled = false;
    }

    private void OnDisable() {
        mm.movingEnabled = true;
    }

    public void Show_Hide()
    {
        active = !active;
        gameObject.SetActive(active);
    }
}
