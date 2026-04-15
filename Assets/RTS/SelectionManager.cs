using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] List<NavMeshAI> selectedAI;

    [SerializeField] InputAction selectionAction, deselectionAction, groupAction;

    RaycastHit hit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectedAI = new List<NavMeshAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selectionAction.WasPressedThisFrame())
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Ally")
                {
                    NavMeshAI agent = hit.transform.GetComponent<NavMeshAI>();
                    if (agent != null)
                    {
                        if (!groupAction.IsPressed())
                        {
                            selectedAI.Clear();
                        }
                        if (!selectedAI.Contains(agent))
                        {                           
                            selectedAI.Add(agent);
                        }
                    }
                    
                }
                else if (hit.transform.tag == "Ground")
                {
                    Debug.Log("Ground");
                    foreach (NavMeshAI agent in selectedAI)
                    {
                        agent.SetTarget(hit.point);
                    }
                }
                else if (hit.transform.tag == "Enemy")
                {
                    Debug.Log("Enemy");
                    foreach (NavMeshAI agent in selectedAI)
                    {
                        agent.SetTarget(hit.transform.gameObject);
                    }
                }
            }
        }

        if (deselectionAction.WasPressedThisFrame())
        {
            selectedAI.Clear();
        }
    }

    private void OnEnable()
    {
        selectionAction.Enable();
        deselectionAction.Enable();
        groupAction.Enable();
    }

    private void OnDisable()
    {
        selectionAction.Disable();
        deselectionAction.Disable();
        groupAction.Disable();
    }
}
