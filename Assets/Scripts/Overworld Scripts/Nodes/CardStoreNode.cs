using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CardStoreNode : MapNode
{
    public override void OnVisit()
    {   
       SceneManager.LoadScene("CardStore");
    }
}
