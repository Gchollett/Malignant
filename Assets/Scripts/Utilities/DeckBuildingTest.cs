using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckBuildingTest : MonoBehaviour
{
    private CardData[] CardObjects;
    private GameObject[] AbilityPrefabs;
    public GameObject CardContent;
    public GameObject AbilityContent;
    void Start()
    {
        CardObjects = Resources.LoadAll<CardData>("ScriptableObjects/CreatureData");
        AbilityPrefabs = Resources.LoadAll<GameObject>("Prefabs/AbilityPrefabs");
    }
}