using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldPlayer : MonoBehaviour
{
    public static OverworldPlayer Instance;
    MapManager mm;
    public MapNode currentNode;
    public void initializePlayer()
    {   
        Debug.Log("Running");
        mm = MapManager.Instance;
        currentNode = mm.nodes[0][0];
        Debug.Log($"Player started at node {currentNode}");
        Debug.Log( currentNode.GetComponent<LineRenderer>().colorGradient.colorKeys[0].color);
        currentNode.GetComponent<LineRenderer>().colorGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
        );
        Debug.Log( currentNode.GetComponent<LineRenderer>().colorGradient.colorKeys[0].color);
    }

    // Update is called once per frame
    private void Update() {
        //Debug.Log("Player Update");
        currentNode.GetComponent<LineRenderer>().colorGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
        );
    }

}
