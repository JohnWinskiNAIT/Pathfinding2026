using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationControl : MonoBehaviour
{
    // Animator Variables
    Animator animator;

    // Health Script
    Health myHealth;
    HealthUI myUI;

    // Movement Variables
    Vector2 moveValue;
    float currentMoveSpeed;
    bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables
       
        animator = GetComponent<Animator>();
        myHealth = GetComponent<Health>();
        myUI = GetComponentInChildren<HealthUI>();
    }

    void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death") ==  false)
        {
            // Call the AnimatePlayer method
            AnimateCharacter();
        }
    }

    public void AdjustMovementValues(Vector2 newVector, float moveSpeed, bool walkOrRun)
    {
        // These values are sent from the MovementControl
        moveValue = newVector;
        currentMoveSpeed = moveSpeed;
        isRunning = walkOrRun;
    }

    public void AnimateCharacter()
    {
        if (isRunning)
        {
            // If the player is moving forward
            if (moveValue.y > 0)
            {
                // Turn on the run animation.
                animator.SetBool("Running", true);

                // Set the animation speed to the input value.
                animator.SetFloat("WalkingSpeed", moveValue.y);
            }
            // If the player is moving backward
            else if (moveValue.y < 0)
            {
                // Turn off the run animation.
                animator.SetBool("Running", false);

                // Set the animation speed to the input speed multiplied by 2. The walk animation needs to be multiplied to reduce sliding.
                animator.SetFloat("WalkingSpeed", moveValue.y * currentMoveSpeed * 2f);
            }
            else if (moveValue.x != 0)
            {
                animator.SetFloat("WalkingSpeed", moveValue.x * currentMoveSpeed * 2f);
            }
        }
        else
        {
            // Turn off the run animation.
            animator.SetBool("Running", false);

            if (moveValue.y != 0)
            {
                // Set the animation speed to the input speed multiplied by 2. The walk animation needs to be multiplied to reduce sliding.
                animator.SetFloat("WalkingSpeed", moveValue.y * currentMoveSpeed);
            }
            else if (moveValue.x != 0)
            {
                animator.SetFloat("WalkingSpeed", moveValue.x * currentMoveSpeed);
            }
        }

        // Turn on the walking animation if the player is moving at all.
        if (moveValue.y != 0 || moveValue.x != 0)
        {
            animator.SetBool("Walking", true);
        }
        // Turn off both walking and run animations if the player is not moving.
        else
        {
            animator.SetBool("Walking", false);
            animator.SetBool("Running", false);
        }
    }
    
    public void CharacterAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void CharacterAttack2()
    {
        animator.SetTrigger("Attack2");
    }

    public void CharacterDamaged()
    {
        if (myHealth.GetCurrentHealth() <= 0 && animator.GetCurrentAnimatorStateInfo(0).IsName("Death") == false)
        {
            animator.SetTrigger("Death");

            // Turn off colider and set physics to kinematic
            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;

            // Turn off healthbar
            myUI.TurnOffUI();
        }
        else
        {
            animator.SetTrigger("Damage");
        }
    }
}
