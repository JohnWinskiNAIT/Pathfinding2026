using UnityEngine;
using UnityEngine.AI;

public class NavMeshAI : MonoBehaviour
{
    [SerializeField] Vector3 targetLocation;
    [SerializeField] GameObject target;
    NavMeshAgent agent;

    AnimationControl myAnimControl;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        myAnimControl = GetComponent<AnimationControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetLocation != Vector3.zero)
        {
            agent.destination = targetLocation;
        }
        
        if (myAnimControl != null)
        {
            myAnimControl.AdjustMovementValues(new Vector2(0,agent.velocity.magnitude), 1, true);
        }
    }

    public void SetTarget(Vector3 location)
    {
        targetLocation = location;
        agent.stoppingDistance = 0;
    }

    public void SetTarget(GameObject targetObject)
    {
        target = targetObject;
        targetLocation = targetObject.transform.position;
        agent.stoppingDistance = 3;
    }
}
