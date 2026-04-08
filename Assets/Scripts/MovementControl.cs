// This scrript is used to manage the player input and character movement.

using UnityEngine;
using UnityEngine.InputSystem;

public class MovementControl: MonoBehaviour
{
    // Is the character controlled by the player
    [SerializeField] bool playerControlled;
    
    // Input Variables
    [SerializeField] Vector2 moveValue;
    [SerializeField] float rotateValue;
    public InputAction runAction, attackAction, attack2Action, damageAction;
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
        //if (playerControlled)
        //{
        //    if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.forward, out hit, 3.0f))
        //    {
        //        if (interactableObject == null)
        //        {
        //            hit.transform.TryGetComponent(out interactableObject);
        //        }                
        //    }
        //    else
        //    {
        //        interactableObject = null;
        //    }

        //    if (interactableObject != null && !interactPanel.activeSelf)
        //    {
        //        interactPanel.SetActive(true);
        //    }
        //    else if (interactableObject == null && interactPanel.activeSelf)
        //    {
        //        interactPanel.SetActive(false);
        //    }

        //    if (Keyboard.current.eKey.wasPressedThisFrame && interactableObject != null)
        //    {
        //        interactableObject.Interact();
        //    }
        //}        
        
        // If the character is not dead then accept player or AI input
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death") == false)
        {
            // Only allow the character to attack at specific intervals
            if (playerControlled && Time.time > attackTimeStamp + attackRate)
            {
                if (attackAction.WasPressedThisFrame())
                {
                    myAnimControl.CharacterAttack();
                    attackTimeStamp = Time.time;
                }
                if (attack2Action.WasPressedThisFrame())
                {
                    myAnimControl.CharacterAttack2();
                    attackTimeStamp = Time.time;
                }
            }
            else
            {                
                PlayAI();
            }
        }

        // Enable or disable player input based on the playerControlled bool
        if (playerControlled)
        {
            GetComponent<PlayerInput>().enabled = true;
            Camera.main.transform.position = camLocation.transform.position;
            Camera.main.transform.rotation = camLocation.transform.rotation;
            //cam.enabled = true;
            gameObject.tag = "Player";
            //myUI.TurnOffUI();
        }
        else
        {
            GetComponent<PlayerInput>().enabled = false;
            //cam.enabled = false;
            gameObject.tag = "Untagged";
            //myUI.TurnOnUI();
        }
    }
    void FixedUpdate()
    {
        // Update the movement of the character if it is not dead
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death") ==  false)
        {
            if (playerControlled)
            {
                // Call the SetMovementValues method
                SetMovementValues();

                // Call the MovePlayer method
                MoveCharacter();
            }
        }
    }

    // This method is called by the Player Action Map on the prefab object
    public void OnPlayerMove(InputAction.CallbackContext context)
    {
        moveValue = context.ReadValue<Vector2>();
    }

    public void OnPlayerRotate(InputAction.CallbackContext context)
    {
        rotateValue = context.ReadValue<float>();
    }

    public void SetMovementValues()
    {
        // If the player is pressing the run button
        if (runAction.IsPressed())
        {
            // If the player is moving forward
            if (moveValue.y > 0)
            {
                // Set the moveSpeed to the runForwardSpeed.
                currentMoveSpeed = runForwardSpeed;
                isRunning = true;
            }
            // If the player is moving backward
            else if (moveValue.y < 0)
            {
                // Set the moveSpeed to the runBackwardSpeed.
                currentMoveSpeed = runBackwardSpeed;
                isRunning = false;
            }
            else if (moveValue.x != 0)
            {
                // Set the moveSpeed to the runBackwardSpeed.
                currentMoveSpeed = runBackwardSpeed;
                isRunning = false;
            }
        }
        // If the player is not pressing the run button
        else
        {
            // Set the moveSpeed to the walkSpeed.
            currentMoveSpeed = walkSpeed;
            isRunning = false;
        }

        // Update the Animation Control
        myAnimControl.AdjustMovementValues(moveValue, currentMoveSpeed, isRunning);
    }
    
    public void MoveCharacter()
    {
        // If the player is not currently playing an attack animation then move and rotate.        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01") == false && 
            animator.GetCurrentAnimatorStateInfo(0).IsName("Attack02") == false &&
            animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit") == false &&
            animator.GetCurrentAnimatorStateInfo(0).IsName("Death") == false)
        {           
            // Move and Rotate the player based on the input values
            transform.Translate(new Vector3(moveValue.x, 0, moveValue.y) * currentMoveSpeed * Time.deltaTime);
            transform.Rotate(Vector3.up, rotateValue * rotateSpeed * Time.deltaTime);
        }
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
        if (other.tag == "Player" && !playerControlled)
        {
            target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.gameObject == target)
        {
            target = null;
        }
    }

    private void OnEnable()
    {
        runAction.Enable();
        attackAction.Enable();
        attack2Action.Enable();
        damageAction.Enable();
    }

    private void OnDisable()
    {
        runAction.Disable();
        attackAction.Disable();
        attack2Action.Disable();
        damageAction.Disable();
    }
}
