using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossNode : MapNode
{
    HandData[] hands;
    private void Start() {
        hands = Resources.LoadAll<HandData>("ScriptableObjects/Hands");
    }
    public override void OnVisit()
    {   
        List<HandData> bossHands = new List<HandData>(hands.Where((x) => x.difficulty == Difficulty.Boss));
        dm.EnemyHand = bossHands[Random.Range(0,bossHands.Count)];
        SceneManager.LoadScene("CardGame");
    }
}
