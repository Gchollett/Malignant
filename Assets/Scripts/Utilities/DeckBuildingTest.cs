using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckBuildingTest : MonoBehaviour
{
    private CardData[] CardObjects;
    private GameObject[] AbilityPrefabs;
    private HandData[] EnemyHands;
    public Transform CardContent;
    public GameObject CardPrefab;
    public Transform AbilityContent;
    public GameObject AbilityPrefab;
    public Transform DeckContent;
    public Canvas canvas;
    private DataManager dm;
    void Start()
    {
        dm = DataManager.Instance;
        CardObjects = Resources.LoadAll<CardData>("ScriptableObjects/CreatureData");
        EnemyHands = Resources.LoadAll<HandData>("ScriptableObjects/Hands");
        AbilityPrefabs = Resources.LoadAll<GameObject>("Prefabs/AbilityPrefabs");
        foreach(CardData card in CardObjects){
            GameObject new_card = Instantiate(CardPrefab,CardContent);
            new_card.GetComponent<Card>().cardData = card;
            new_card.transform.localScale = Vector2.one*2;
            Button button = new_card.AddComponent<Button>();
            button.onClick.AddListener(() => {
                dm.Deck.Add(Instantiate(card));
            });
        }
        foreach(GameObject ability in AbilityPrefabs){
            GameObject new_ability = Instantiate(AbilityPrefab,AbilityContent);
            new_ability.transform.localScale = Vector2.one;
            new_ability.GetComponent<AbilityPreview>().ab = ability.GetComponent<Ability>();
            new_ability.AddComponent<Draggable>();       
        }
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

    public void Okay()
    {
        dm.EnemyHand = EnemyHands[Random.Range(0,EnemyHands.Count()-1)];
        SceneManager.LoadScene("CardGame");
    }

    public void ClearDeck()
    {
        dm.Deck.Clear();
    }
}