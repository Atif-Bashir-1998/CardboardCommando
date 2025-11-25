using UnityEngine;

public class MoveToMouseClick : MonoBehaviour
{
    // Public variables configurable in the Inspector
    public float speed = 4.0f; // Increased for more reasonable movement
    public float stopDistance = 0.1f; // How close we need to be to the target to stop
    public float rotationSpeed = 8.0f; // Separate rotation speed
    
    // Private variables for tracking state and components
    private Vector3 targetPosition;
    private Vector3 shootDirection;
    private Animator animator;
    private bool isShooting = false;

    // Animation parameter names
    private const string MOVEMENT_PARAM_NAME = "isRunning";
    private const string FIRING_PARAM_NAME = "isFiring";

    void Start()
    {
        animator = GetComponent<Animator>();
        targetPosition = transform.position;
        shootDirection = transform.forward;

        if (animator != null)
        {
            animator.SetBool(MOVEMENT_PARAM_NAME, false); 
            animator.SetBool(FIRING_PARAM_NAME, false);
        }
    }

    void Update()
    {
        // 1. Check for RIGHT Mouse Click Input for movement
        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                targetPosition = hit.point;
                isShooting = false; // Stop shooting when moving
            }
        }

        // 2. Check for LEFT Mouse Click Input for firing
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Set the shoot direction and position
                Vector3 shootTarget = hit.point;
                shootTarget.y = transform.position.y; // Keep at same height for flat rotation
                shootDirection = (shootTarget - transform.position).normalized;
                
                // Trigger shooting
                isShooting = true;
                
                // Stop movement and start firing animation
                if (animator != null)
                {
                    animator.SetBool(MOVEMENT_PARAM_NAME, false);
                    animator.SetBool(FIRING_PARAM_NAME, true);
                }
            }
        }

        // Stop firing when left mouse button is released
        if (Input.GetMouseButtonUp(0)) // Left mouse button released
        {
            isShooting = false;
            if (animator != null)
            {
                animator.SetBool(FIRING_PARAM_NAME, false);
            }
        }

        // Handle rotation based on current action
        if (isShooting)
        {
            targetPosition = transform.position;
            // Rotate to face shooting direction
            RotateTowardsDirection(shootDirection);
        }
        else
        {
            // Handle movement and rotation for normal movement
            HandleMovement();
        }
    }

    // Separate function to handle rotation towards a specific direction
    private void RotateTowardsDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }

    // Separate function to handle movement logic
    private void HandleMovement()
    {
        // Calculate the remaining distance to the target
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget > stopDistance)
        {
            // Move the soldier at constant speed using MoveTowards
            transform.position = Vector3.MoveTowards(
                transform.position,     
                targetPosition,         
                Time.deltaTime * speed  
            );
            
            // Look in the direction of travel
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0; // Keep rotation flat on the ground
            
            // Use the rotation function
            RotateTowardsDirection(direction);
            
            // Animation Logic: Start Walking
            if (animator != null)
            {
                animator.SetBool(MOVEMENT_PARAM_NAME, true);
            }
        }
        else
        {
            // Animation Logic: Stop Walking and Idle
            if (animator != null)
            {
                animator.SetBool(MOVEMENT_PARAM_NAME, false);
            }
        }
    }
}