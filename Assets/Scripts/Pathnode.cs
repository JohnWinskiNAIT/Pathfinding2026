using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathnode : MonoBehaviour
{
    public List<GameObject> connections;

    public bool nodeActive = true;

    Pathnode()
    {
        connections = new List<GameObject>();

        nodeActive = true;
    }

    public void AddConnection(GameObject target)
    {
        connections.Add(target);
    }

    public void ClearConnections()
    {
        connections.Clear();
    }

    public void GetConnections(ref List<GameObject> targets)
    {
        targets = connections;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.5f);

        foreach (GameObject target in connections)
        {
            if (target.GetComponent<Pathnode>().nodeActive == true)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            
            Gizmos.DrawLine(transform.position, target.transform.position + new Vector3(0, 0.5f, 0));
        }
    }
}