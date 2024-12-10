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
        hands = Resources.LoadAll<HandData>("/ScriptableObjects/Hands");
    }
    public override void OnVisit()
    {   
        List<HandData> easyHands = new List<HandData>(hands.Where((x) => x.difficulty == Difficulty.Boss));
        dm.EnemyHand = easyHands[Random.Range(0,easyHands.Count)];
        SceneManager.LoadScene("CardGame");
    }
}
