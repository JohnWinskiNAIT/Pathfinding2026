using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] GameObject startNode, destNode, currentNode, targetNode;

    [SerializeField] List<int> nodeIDs = new List<int>();
    [SerializeField] List<GameObject> pathList = new List<GameObject>();
    [SerializeField] List<GameObject> nodeList = new List<GameObject>();

    [SerializeField] bool findPath = true;
    bool validPath;
    int pathIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = startNode.transform.position;
        currentNode = startNode;
    }

    // Update is called once per frame
    void Update()
    {
        if (findPath)
        {
            findPath = false;
            FindPath();
            pathIndex = 0;
            targetNode = pathList[pathIndex];
        }
  
        // Move to target
        if (targetNode != null && validPath)
        {
            // Switch current node when target reached
            if (Vector3.Distance(transform.position, targetNode.transform.position) < 0.15f)
            {
                currentNode = targetNode;

                if (currentNode == destNode)
                {
                    targetNode = null;
                }
                else
                {                    
                    findPath = true;
                }               
            }

            if (targetNode != null)
            {
                transform.Translate((targetNode.transform.position - transform.position).normalized * Time.deltaTime * 3);
            }            
        }        
    }

    void FindPath()
    {
        nodeList.Clear();
        nodeList.Add(destNode);
        nodeIDs.Clear();
        nodeIDs.Add(0);

        bool found = false;
        validPath = false;
        int currentIndex;

        // Number the nodes
        for (int i = 0; i < nodeList.Count && !found; i++)
        {
            Pathnode nodeScript = nodeList[i].GetComponent<Pathnode>();

            for (int j = 0; j < nodeScript.connections.Count && !found; j++)
            {
                if (!nodeList.Contains(nodeScript.connections[j]) && nodeScript.connections[j].GetComponent<Pathnode>().nodeActive)
                {
                    nodeList.Add(nodeScript.connections[j]);
                    nodeIDs.Add(nodeIDs[i] + 1);
                }

                if (nodeScript.connections[j] == currentNode)
                {
                    found = true;
                }
            }

            if (i > 10000)
            {
                found = true;
            }
        }

        // Make the path
        int numberOfNodes;

        currentIndex = nodeList.FindIndex(x => x == currentNode);
        if (currentIndex > -1)
        {
            numberOfNodes = nodeIDs[currentIndex];
            pathList.Clear();
            pathList.Add(nodeList[currentIndex]);

            int currentID = nodeIDs[nodeIDs.Count - 1];

            for (int i = 0; i < numberOfNodes; i++)
            {
                Pathnode nodeScript = pathList[i].GetComponent<Pathnode>();

                found = false;

                for (int j = 0; j < nodeScript.connections.Count && !found; j++)
                {
                    currentIndex = nodeList.FindIndex(x => x == nodeScript.connections[j]);
                    if (currentIndex > -1)
                    {
                        if (nodeIDs[currentIndex] == currentID - 1)
                        {
                            pathList.Add(nodeList[currentIndex]);
                            currentID--;
                            found = true;
                        }
                    }

                }
            }
        }
        
        pathList.RemoveAt(0);
        if (found)
        {
            validPath = true;
        }
    }
}
