using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckBuildingTest : MonoBehaviour
{
    private CardData[] CardObjects;
    private GameObject[] AbilityPrefabs;
    public Transform CardContent;
    public GameObject CardPrefab;
    public Transform AbilityContent;
    public GameObject AbilityPrefab;
    public Transform DeckContent;
    void Start()
    {
        CardObjects = Resources.LoadAll<CardData>("ScriptableObjects/CreatureData");
        AbilityPrefabs = Resources.LoadAll<GameObject>("Prefabs/AbilityPrefabs");
        foreach(CardData card in CardObjects){
            GameObject new_card = Instantiate(CardPrefab,CardContent);
            new_card.GetComponent<Card>().cardData = card;
            new_card.transform.localScale = Vector2.one;
            break;
        }
        foreach(GameObject ability in AbilityPrefabs){
            GameObject new_ability = Instantiate(AbilityPrefab,AbilityContent);
            new_ability.transform.localScale = Vector2.one;
        }
    }
}