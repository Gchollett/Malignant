using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeNode : MapNode
{
    public override void OnVisit()
    {   
        GameObject.Find("Pop-up Canvas").transform.GetChild(1).gameObject.SetActive(true);
    }
}
