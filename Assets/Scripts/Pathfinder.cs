using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] GameObject startNode, destNode, currentNode, targetNode, prevNode;
    [SerializeField] List<GameObject> targets;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = startNode.transform.position;
        currentNode = startNode;
    }

    // Update is called once per frame
    void Update()
    {
        // Get reference distance
        float closestDist = 100000;

        // Get list of connections
        currentNode.GetComponent<Pathnode>().GetConnections(ref targets);

        // Find closest connection
        for (int i = 0; i < targets.Count; i++)
        {
            if (Vector3.Distance(destNode.transform.position, targets[i].transform.position) < closestDist && targets[i] != prevNode &&
                targets[i].GetComponent<Pathnode>().nodeActive)
            {
                targetNode = targets[i];
                closestDist = Vector3.Distance(destNode.transform.position, targets[i].transform.position);
            }
            
        }

        // Move to target
        if (targetNode != null)
        {
            //targetNode.GetComponent<Pathnode>().nodeActive = false;
            //currentNode.GetComponent<Pathnode>().nodeActive = true;
            transform.Translate((targetNode.transform.position - transform.position).normalized * Time.deltaTime * 3);
        }

        // Switch current node when target reached
        if (Vector3.Distance(transform.position, targetNode.transform.position) < 0.15f)
        {            
            prevNode = currentNode;
            currentNode = targetNode;
            
            if (currentNode == destNode)
            {
                GameObject tempNode;

                tempNode = destNode;
                destNode = startNode;
                startNode = tempNode;
            }
        }
    }
}
