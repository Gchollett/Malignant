using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckBuildingTest : MonoBehaviour
{
    private GameObject[] CardPrefabs;
    private GameObject[] AbilityPrefabs;
    public GameObject CardContent;
    public GameObject AbilityContent;
    void Start()
    {
        CardPrefabs = Resources.LoadAll<GameObject>("Prefabs/CreatureCardPrefabs");
        AbilityPrefabs = Resources.LoadAll<GameObject>("Prefabs/AbilityPrefabs");
        foreach(GameObject card in CardPrefabs){
            GameObject InstantiatedCard = Instantiate(card,CardContent.transform);
        }
        // foreach(GameObject ability in AbilityPrefabs){
        //     GameObject InstantiatedAbility = Instantiate(ability,AbilityContent.transform);
        // }
    }
}