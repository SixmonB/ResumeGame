using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChasePlayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    private Rigidbody rb;
    private Transform target;
    private Vector3 moveDirection;
    private float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    private bool isTriggered = false;
    [SerializeField] Animator animator;
    public bool isMoving = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        BossTrigger.current.onDoorwayTriggerEnter += ActivateBoss;
        animator = GetComponent<Animator>();
    }

    private void ActivateBoss()
    {
        isTriggered = true;
        isMoving = true;
        animator.SetBool("isChasing", true);
    }

    private void FixedUpdate()
    {
        if (isTriggered && isMoving)
        {
            if (target != null)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                moveDirection = direction;
                moveDirection.y = 0;
                rb.rotation = Quaternion.Euler(0f, angle, 0f);
                rb.velocity = moveDirection * moveSpeed;       
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isMoving = false;
            animator.SetBool("inRange", true);
            animator.SetBool("isChasing", false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isMoving = true;
            animator.SetBool("inRange", false);
            animator.SetBool("isChasing", true);
        }
    }
}
