using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    [SerializeField] private Transform camera;

    [SerializeField] private Animator animator;

    [SerializeField] private Transform groundCheck;

    [SerializeField] private LayerMask groundMask;

    public float moveSpeed = 6f;
    private float runSpeed = 6f;
    private float walkSpeed = 3f;
    private float gravity = -9.81f * 2;
    private float jumpHeight = 2f;
    private float groundDistance = 0.1f;
    public bool isGrounded;
    private float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    private Vector3 velocity;
    private float lastY_Velocity;
    private bool isFalling;

    void Update()
    {
        // ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded)
        {
            // if the character was falling and now is grounded, we can assume he's finished falling and he has landed
            if(isFalling)
            {
                isFalling = false;
                animator.SetBool("isFalling", false);
                animator.SetBool("hasLanded", true);
            }

            // resetting default y velocity
            if(velocity.y < 0)
            {
                velocity.y = -2f;
            }

            // jumping
            if (Input.GetButtonDown("Jump")){
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                animator.SetBool("isJumping", true);
                animator.SetBool("hasLanded", false);
            }
        }

        // falling down
        velocity.y += gravity * Time.deltaTime;

        // if he was going up and now he's going down, we can assume he's finished jumping and has started to fall
        if (velocity.y < 0 && lastY_Velocity > 0)
        {
            isFalling = true;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }

        lastY_Velocity = velocity.y; // variable update

        controller.Move(velocity * Time.deltaTime);
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
    
        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);

            animator.SetBool("isMoving", true);

            if(Input.GetKey(KeyCode.LeftShift) && isGrounded)
            {
                Walk();
            }
            else if (!Input.GetKey(KeyCode.LeftShift) && isGrounded)
            {
                Run();
            }
        }

        else if(direction.magnitude == 0)
        {
            Idle();
        }

    }

    private void Idle()
    {
        animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    private void Run()
    {
        moveSpeed = runSpeed;
        animator.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
    }
}
