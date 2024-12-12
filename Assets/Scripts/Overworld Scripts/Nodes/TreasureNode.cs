using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TreasureNode : MapNode
{
    public override void OnVisit()
    {   
        GameObject.Find("Pop-up Canvas").transform.GetChild(0).gameObject.SetActive(true);
    }
}
