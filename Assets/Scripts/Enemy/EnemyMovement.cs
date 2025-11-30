using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    [SerializeField] private AudioClip walkSFX;

    [SerializeField] private float movementSpeed = 1f;
    Rigidbody rb;
    Animator animator;

    private EnemyHealth healthStatus;

    // use to chase player
    [SerializeField] GameObject player;
    //Transform target;
    Vector3 moveDirection;
    Vector3 direction;
    public Quaternion desiredRotation;

    public float detectionRange;
    public float distanceBetween;
    public float stopDistance;

    public float rotationSpeed = 1f;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        
        healthStatus = GetComponent<EnemyHealth>();
    }

    void Start() {
        player = GameObject.Find("Player");
        desiredRotation = transform.rotation;
    }

    void Update() {
        HandleMovement();
    }

    private void HandleMovement() {
        distanceBetween = Vector3.Distance(transform.position, player.transform.position);
        direction = player.transform.position - transform.position;
        direction.y = 0;    // To prevent from flying or jumping when chasing player.

        // Rotate towards the player.
        desiredRotation = Quaternion.LookRotation(direction, Vector3.up);
        if (distanceBetween < detectionRange && direction != Vector3.zero && !healthStatus.isDead) {
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, desiredRotation, rotationSpeed * Time.fixedDeltaTime));
        }

        if ((distanceBetween < detectionRange) && canMove()) {
            if (direction.magnitude > stopDistance) {
                moveDirection = direction.normalized;
                rb.velocity = new Vector3(moveDirection.x * movementSpeed, rb.velocity.y, moveDirection.z * movementSpeed);

                animator.SetBool("isWalking", true);
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

    private bool canMove() {
        if (healthStatus.isDead) {
            return false;
        }
        else {
            return true;
        }
    }

    public void PlayWalkSFX() {
        SoundManager.instance.PlaySound(walkSFX);
    }
}
