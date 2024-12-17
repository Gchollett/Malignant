using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AbilityStore : MonoBehaviour
{
    private GameObject[] AbilityPrefabs;
    [SerializeField] private int numItems = 3;
    public GameObject DeckCardPrefab;
    public Transform AbilityContent;
    public GameObject AbilityPrefab;
    public Transform DeckContent;
    public TextMeshProUGUI moneyText;
    public Canvas canvas;
    public List<(GameObject,int)> StoreAbilities {get;set;} = new List<(GameObject, int)>();
    private DataManager dm;
    void Start()
    {
        dm = DataManager.Instance;
        AbilityPrefabs = Resources.LoadAll<GameObject>("Prefabs/AbilityPrefabs");
        List<GameObject> chosenAbilities = new List<GameObject>();
        for(int i =0; i< numItems;i++){
            chosenAbilities.Add(AbilityPrefabs[Random.Range(0,AbilityPrefabs.Count())]);
        }
        foreach(GameObject ability in chosenAbilities){
            GameObject new_ability = Instantiate(AbilityPrefab,AbilityContent);
            new_ability.transform.localScale = Vector2.one;
            new_ability.transform.GetChild(0).GetComponent<AbilityPreview>().ab = ability.GetComponent<Ability>();
            DragToSell dts = new_ability.AddComponent<DragToSell>();    
            int cost = 10+Random.Range(0,11);
            dts.cost =  cost;
            dts.ab = this;
            new_ability.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"${cost}";
            StoreAbilities.Add((new_ability,cost));
        }
        DisplayDeck();
    }
    private void FixedUpdate() {
        if(dm.Deck.Count != DeckContent.childCount){
            FixDeck();
        }
        foreach((GameObject,int) ab in StoreAbilities){
            if(ab.Item2 > dm.money){
                ab.Item1.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.red;
            }else{
                ab.Item1.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.white;
            }
        }
        moneyText.text = $"${dm.money}";
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