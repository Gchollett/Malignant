using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TreasureNode : MapNode
{
    public override void OnVisit()
    {   
       SceneManager.LoadScene("Main Scene 1");
    }
}
