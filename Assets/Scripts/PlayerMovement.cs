using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f * 2;
    [SerializeField] private float jumpHeight = 3f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private float displacementTolerance = 0.005f;

    private Vector3 velocity;

    public bool isGrounded;

    public bool isMoving;

    private Vector3 lastPosition = new Vector3(0f,0f,0f);

    private CharacterController controller;

    private Animator animator;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // resetting default y velocity
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // inputs

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * moveSpeed * Time.deltaTime);

        // jump
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // falling down
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);


        Vector3 displacement = gameObject.transform.position - lastPosition;
        float displacementValue = displacement.magnitude;

        Debug.Log("Desplazamiento : " + displacementValue);

        if (displacementValue >= displacementTolerance && isGrounded == true)
        {
            isMoving = true;
            Move();
        }
        else
        {
            isMoving = false;
            Idle();
        }

        lastPosition = gameObject.transform.position;
    }

    private void Idle()
    {
        animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    private void Move()
    {
        animator.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
    }
}
