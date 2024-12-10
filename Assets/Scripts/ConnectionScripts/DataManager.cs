using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public HandData EnemyHand;
    public List<CardData> Deck;
    public int startingPips = 0;
    public int money = 10;
    public static MapManager mm;
    public static OverworldPlayer op;
    void Awake()
    {
        if(!Instance) Instance = this;
        else Destroy(gameObject);
        List<CardData> cards = new List<CardData>(Resources.LoadAll<CardData>("ScriptableObjects/CreatureData").Where((x) => x.rarity == Rarity.Common));
        for(int i =0; i<4;i++){
            Deck.Add(cards[Random.Range(0,cards.Count)]);
        }
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
        
}
