using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    private List<GameObject> Abilities;
    public GameObject AbilityPrefab;
    public Transform AbilityContent;
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
        if(!mm) mm = MapManager.Instance;
        mm.movingEnabled = false;
        generateAbilities();
    }

    private void OnDisable() {
        mm.movingEnabled = true;
    }

    private void generateAbilities(){
        for(int i = 0; i < AbilityContent.childCount;i++){
            Destroy(AbilityContent.GetChild(i).gameObject);
        }
        List<GameObject> ChosenAbilities = new List<GameObject>();
        Abilities = new List<GameObject>(Resources.LoadAll<GameObject>("Prefabs/AbilityPrefabs"));
        for(int i = 0; i < Random.Range(1,5); i++){
            int index = Random.Range(0,Abilities.Count);
            ChosenAbilities.Add(Abilities[index]);
            Abilities.RemoveAt(i);
        }
        foreach(GameObject ability in ChosenAbilities){
            GameObject new_ability = Instantiate(AbilityPrefab,AbilityContent);
            new_ability.transform.localScale = Vector2.one;
            new_ability.GetComponent<AbilityPreview>().ab = ability.GetComponent<Ability>();
            DragToUpgrade dtu = new_ability.AddComponent<DragToUpgrade>(); 
            dtu.ParentTransform = transform;      
        }
    }
}