using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AStarNode : MonoBehaviour
{

    public List<AStarNode> adyacent;
    public HashSet<Item> nearbyItems = new HashSet<Item>();
    public float radious = 2;
    public int heuristic = 1;
    public bool blocked;

    public void InitializeNodes()
    {
        ActualizeBlock();
        if (blocked) return;
        GetAdyacent();
    }

    public void ActualizeBlock()
    {
        blocked = false;

        var overlap = Physics.OverlapSphere(transform.position, 1, 2 << 0 & 15, QueryTriggerInteraction.Ignore);

        foreach (var item in overlap)
        {
            if (item == this) continue;
            if (item.gameObject.layer == 0 || item.gameObject.layer == 15)
            {
                Debug.Log(item.name);
                blocked = true;
                break;
            }
        }
    }

    void GetAdyacent()
    {
        adyacent = Physics.OverlapSphere(transform.position, radious)
                          .Where(x => x.GetComponent<AStarNode>() && x != this)
                          .Select(x => x.GetComponent<AStarNode>()).Where(x=>!x.blocked).ToList();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
        Gizmos.color = Color.blue;
        foreach (var wp in adyacent)
        {
            Gizmos.DrawLine(transform.position, wp.transform.position);
        }

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, radious);
    }
}
