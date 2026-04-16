using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAI : MonoBehaviour
{
    [SerializeField] Vector3 targetLocation;
    [SerializeField] GameObject target;
    NavMeshAgent agent;

    [SerializeField] bool followingCommand = false;

    // Movement Variables
    float currentMoveSpeed, walkSpeed, runForwardSpeed, runBackwardSpeed, rotateSpeed, rotateAISpeed;

    // Attack Variables
    float attackTimeStamp, attackRate;

    // Spawn Locations
    [SerializeField] GameObject spawnLocation;

    // Animator Variables
    Animator animator;

    // Animation Control Script
    AnimationControl myAnimControl;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize variables
        walkSpeed = 1.0f;
        runForwardSpeed = 3.0f;
        runBackwardSpeed = 2.0f;
        rotateSpeed = 100.0f;
        rotateAISpeed = 5.0f;
        attackRate = 1.0f;

        //transform.position = GameObject.Find("SpawnLocation").transform.position;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        myAnimControl = GetComponent<AnimationControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetLocation != Vector3.zero)
        {
            agent.destination = targetLocation;
            if (Vector3.Distance(targetLocation, transform.position) < 0.1f + agent.stoppingDistance)
            {
                followingCommand = false;
            }
        }        
        
        if (myAnimControl != null)
        {
            myAnimControl.AdjustMovementValues(new Vector2(0,agent.velocity.magnitude), 1, true);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death") == false)
        {
            PlayAI();
        }
    }

    public void PlayAI()
    {
        // As long as a target exists then rotate and get within attack range
        if (target != null)
        {
            if (target.GetComponent<Health>().GetCurrentHealth() > 0)
            {
                if (Vector3.Distance(target.transform.position, transform.position) < 3.0f)
                {
                    Vector3 targetDirection = target.transform.position - transform.position;

                    // The step size is equal to speed times frame time.
                    float singleStep = rotateAISpeed * Time.deltaTime;

                    // Rotate the forward vector towards the target direction by one step
                    Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

                    // Calculate a rotation a step closer to the target and applies rotation to this object
                    transform.rotation = Quaternion.LookRotation(newDirection);

                    // Randomly choose an attack
                    if (Time.time > attackTimeStamp + attackRate)
                    {
                        if (Random.Range(0, 2) == 0)
                        {
                            animator.SetTrigger("Attack");
                        }
                        else
                        {
                            animator.SetTrigger("Attack2");
                        }

                        attackTimeStamp = Time.time;
                    }
                }
                else
                {
                    targetLocation = target.transform.position;
                }
            }            
        }
    }

    public void SetTarget(Vector3 location)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death") == false)
        {
            targetLocation = location;
            target = null;
            agent.stoppingDistance = 0;
            followingCommand = true;
        }
    }

    public void SetTarget(GameObject targetObject)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death") == false)
        {
            target = targetObject;
            targetLocation = targetObject.transform.position;
            agent.stoppingDistance = 3;
            followingCommand = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death") == false)
        {
            if (transform.tag == "Ally")
            {
                if (other.tag == "Enemy")
                {
                    if (!followingCommand)
                    {
                        SetTarget(other.gameObject);
                    }
                }
            }

            if (transform.tag == "Enemy")
            {
                if (other.tag == "Ally")
                {
                    SetTarget(other.gameObject);
                }
            }
        }        
    }
}
