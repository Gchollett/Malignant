using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonolithNode : MapNode
{
    List<CardData> cards;
    List<Ability> abilities;
    private void Start() {
        cards = new List<CardData>(Resources.LoadAll<CardData>("ScriptableObjects/CreatureData").Where((x) => x.rarity == Rarity.Monolith));
        abilities = new List<Ability>(Resources.LoadAll<GameObject>("Prefabs/AbilityPrefabs").Select(x => x.GetComponent<Ability>()));
    }
    public override void OnVisit()
    {   
        CardData bossCard = cards[Random.Range(0,cards.Count)];
        HandData hand = new HandData();
        hand.Hand = new List<CardData>();
        for(int i =0;i < Random.Range(6,9);i++){
            CardData new_card = Instantiate(bossCard);
            for(int j = 0; j<Random.Range(1,3);j++){
                Ability ab = abilities[Random.Range(0,abilities.Count)];
                new_card.abilities.Add(ab);
                new_card.cardName = $"{ab.adjective} {new_card.cardName}";
            }
            hand.Hand.Add(new_card);
        }
        dm.EnemyHand = hand;
        dm.isMonolith = true;
        SceneManager.LoadScene("CardGame");
    }
}
