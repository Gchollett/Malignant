using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class MapNode : MonoBehaviour
{
    public static MapManager mm;
    public static DataManager dm;
    public static OverworldPlayer op;
    public List<MapNode> edges;

    private LineRenderer lr;
    private bool visited = false;
  

    private void Start() {
        mm = MapManager.Instance;
        dm = DataManager.Instance;
        op = OverworldPlayer.Instance;
        lr = gameObject.GetComponent<LineRenderer>();
    }

    public abstract void OnVisit();

    private IEnumerator moveThenVisit(){
        op.Move(this);
        yield return new WaitUntil(() => !op.isMoving);
        if(!visited)
        {
            OnVisit();
            mm.numVisited+=1;
            visited = true;
        }
    }
    private void OnMouseDown() {
            if(mm.movingEnabled && op.ReadyToMove(this)){
                StartCoroutine(moveThenVisit());
            }
    }

}
