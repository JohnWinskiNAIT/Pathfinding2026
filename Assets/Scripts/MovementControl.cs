// This scrript is used to manage the player input and character movement.

using UnityEngine;
using UnityEngine.InputSystem;

public class MovementControl: MonoBehaviour
{
    bool isRunning;

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

    // AI Variables
    [SerializeField] GameObject target;

    // Personal Camera
    [SerializeField] GameObject camLocation;

    //HealthUI myUI;

    [SerializeField] GameObject interactPanel;
    RaycastHit hit;
    //IInteractable interactableObject;

    // Start is called before the first frame update
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
        myAnimControl = GetComponent<AnimationControl>();

        //myUI = GetComponentInChildren<HealthUI>();

        Screen.SetResolution(1600, 900, true );
    }

    // Update is called once per frame
    private void Update()
    {        
        // If the character is not dead then accept player or AI input
        PlayAI();
    }
    void FixedUpdate()
    {
        // Update the movement of the character if it is not dead
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death") ==  false)
        {
            //if (playerControlled)
            //{
            //    // Call the SetMovementValues method
            //    SetMovementValues();

            //    // Call the MovePlayer method
            //    MoveCharacter();
            //}
        }
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    public void PlayAI()
    {
        // As long as a target exists then rotate and get within attack range
        if (target != null)
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
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == target)
        {
            target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ally" && other.gameObject == target)
        {
            target = null;
        }
    }
}
