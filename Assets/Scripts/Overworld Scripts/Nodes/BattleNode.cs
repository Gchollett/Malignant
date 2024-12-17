using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleNode : MapNode
{
    HandData[] hands;
    private void Start() {
        hands = Resources.LoadAll<HandData>("ScriptableObjects/Hands");
    }
    public override void OnVisit()
    {  
        if(mm.numVisited < mm.nodeNum/3){
            List<HandData> easyHands = new List<HandData>(hands.Where((x) => x.difficulty == Difficulty.Easy));
            dm.EnemyHand = easyHands[Random.Range(0,easyHands.Count)];
        }else if(mm.numVisited >= mm.nodeNum/3){
            List<HandData> mediumHands = new List<HandData>(hands.Where((x) => x.difficulty == Difficulty.Medium));
            dm.EnemyHand = mediumHands[Random.Range(0,mediumHands.Count)];
        }else if(mm.numVisited > mm.nodeNum/4){
            List<HandData> hardHands = new List<HandData>(hands.Where((x) => x.difficulty == Difficulty.Hard));
            dm.EnemyHand = hardHands[Random.Range(0,hardHands.Count)];
        }
        SceneManager.LoadScene("CardGame");
    }
}