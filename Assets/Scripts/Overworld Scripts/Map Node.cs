using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapNode : MonoBehaviour
{
    public static MapManager mm;
    public List<MapNode> edges;

    private LineRenderer lr;
    

    private void Start() {
        mm = MapManager.Instance;
        lr = gameObject.GetComponent<LineRenderer>();
    }



    

}
