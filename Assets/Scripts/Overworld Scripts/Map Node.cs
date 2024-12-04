using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapNode : MonoBehaviour
{
    public static MapManager mm;
    public List<MapNode> edges;

    private LineRenderer lr;
    private bool visited = false;
  

    private void Start() {
        mm = MapManager.Instance;
        lr = gameObject.GetComponent<LineRenderer>();
    }

    public abstract void OnVisit();

    private void OnMouseDown() {
        if(!visited){
            OnVisit();
            visited = true;
        }
    }

}
