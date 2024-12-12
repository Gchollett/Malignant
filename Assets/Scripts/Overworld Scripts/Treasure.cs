using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Treasure : MonoBehaviour
{
    private CardData[] CardObjects;
    public Transform CardContent;
    public GameObject CardPrefab;
    public Transform DeckContent;
    public Canvas canvas;
    private DataManager dm;
    private MapManager mm;
    void Start()
    {
        dm = DataManager.Instance;
        mm = MapManager.Instance;
    }
    private void FixedUpdate() {
        if(dm.Deck.Count != DeckContent.childCount){
            FixDeck();
        }
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
        generateCards();
    }

    private void generateCards(){
        CardObjects = Resources.LoadAll<CardData>("ScriptableObjects/CreatureData");
        List<CardData> chosenCards = new List<CardData>();
        List<CardData> rareCards = new List<CardData>(CardObjects.Where((x) => x.rarity == Rarity.Rare));
        List<CardData> commonCards = new List<CardData>(CardObjects.Where((x) => x.rarity == Rarity.Common));
        List<CardData> uncommonCards = new List<CardData>(CardObjects.Where((x) => x.rarity == Rarity.Uncommon));
        List<float> percents = new List<float>();
        if(!mm){
            percents = new List<float>{
                .4f,
                .3f,
                .3f
            };
        }else if(mm.numVisited < mm.nodeNum/3){
            percents = new List<float>{
                .8f,
                .15f,
                .05f
            };
        }else if(mm.numVisited >= mm.nodeNum/3){
            percents = new List<float>{
                .6f,
                .30f,
                .10f
            };
        }else if(mm.numVisited > mm.nodeNum/4){
            percents = new List<float>{
                .30f,
                .50f,
                .20f
            };
        }
        for(int i = 0; i<Random.Range(1,5);i++){
            if(Random.Range(0f,1f) < percents[0]){
                chosenCards.Add(commonCards[Random.Range(0,commonCards.Count)]);
            }else if(Random.Range(0f,1f) < percents[0]+percents[1]){
                chosenCards.Add(uncommonCards[Random.Range(0,uncommonCards.Count)]);
            }else{
                chosenCards.Add(rareCards[Random.Range(0,rareCards.Count)]);
            }
        }
        foreach(CardData card in chosenCards){
            GameObject new_card = Instantiate(CardPrefab,CardContent);
            new_card.GetComponent<Card>().cardData = card;
            new_card.transform.localScale = Vector2.one*2;
            Button button = new_card.AddComponent<Button>();
            int value = (((int)card.rarity)+1)*5+Random.Range(0,6);
            button.onClick.AddListener(() => {
                dm.Deck.Add(Instantiate(card));
                gameObject.SetActive(false);
            });
        }
    }
}