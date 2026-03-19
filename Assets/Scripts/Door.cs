using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] bool open = false;

    [SerializeField] Pathnode doorNode;

    MeshRenderer myRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (doorNode != null)
        {
            if (!open && doorNode.nodeActive)
            {
                doorNode.nodeActive = false;
                myRenderer.enabled = true;
            }
            if (open && !doorNode.nodeActive)
            {
                doorNode.nodeActive = true;
                myRenderer.enabled = false;
            }
        }
    }
}
