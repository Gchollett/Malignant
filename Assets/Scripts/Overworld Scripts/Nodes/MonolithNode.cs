using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonolithNode : MapNode
{
    public override void OnVisit()
    {   
       SceneManager.LoadScene("CardGame");
    }
}
