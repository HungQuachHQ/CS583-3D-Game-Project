using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1f;
    Rigidbody rb;
    Animator animator;

    private EnemyHealth healthStatus;

    // use to chase player
    [SerializeField] GameObject player;
    //Transform target;
    Vector3 moveDirection;
    Vector3 direction;

    public float detectionRange;
    public float distanceBetween;
    public float stopDistance;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        healthStatus = GetComponent<EnemyHealth>();
    }

    void Start() {
        player = GameObject.Find("Player");
        //target = player.transform;
    }

    void Update() {
        HandleMovement();
    }

    private void HandleMovement() {
        distanceBetween = Vector3.Distance(transform.position, player.transform.position);
        direction = player.transform.position - transform.position;
        direction.y = 0;    // To prevent from flying or jumping when chasing player.

        if ((distanceBetween < detectionRange) && canMove()) {
            if (direction.magnitude > stopDistance) {
                moveDirection = direction.normalized;
                rb.velocity = new Vector3(moveDirection.x * movementSpeed, rb.velocity.y, moveDirection.z * movementSpeed);

                animator.SetBool("isWalking", true);

                // Rotate towards the player.
                if (direction != Vector3.zero && !healthStatus.isDead) {
                    Quaternion rotateTowards = Quaternion.LookRotation(direction, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotateTowards, 10f * Time.deltaTime);
                }
            }
            else {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
                animator.SetBool("isWalking", false);
            }
        }
        else {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            animator.SetBool("isWalking", false);
        }
    }

    //void FixedUpdate() {
    //    if (target) {
    //        animator.SetBool("isWalking", true);

    //        Vector3 velocity = moveDirection * movementSpeed;
    //        velocity.y = rb.velocity.y; // attatchs it to gravity if rb gravity is checked
    //        rb.velocity = velocity;
    //    }
    //}

    private bool canMove() {
        if (healthStatus.isDead) {
            return false;
        }
        else {
            return true;
        }
    }
}
