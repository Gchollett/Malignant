using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class HunterNode : MapNode
{
    public override void OnVisit()
    {   
       SceneManager.LoadScene("Hunter");
    }
}
