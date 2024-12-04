using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestNode : MapNode
{
    public override void OnVisit()
    {   
        Debug.Log(mm);
       mm.MovePlayer(gameObject.transform.position, this);
       SceneManager.LoadScene("Main Scene 1");
    }
}
