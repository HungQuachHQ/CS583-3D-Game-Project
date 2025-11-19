using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1f;
    Rigidbody rb;

    // use to chase player
    [SerializeField] GameObject target_GO;
    Transform target;
    Vector3 moveDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (target_GO)
        {
            target = target_GO.transform;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target) 
        {
            Vector3 direction = (target.position - transform.position);
            direction.y = 0; // don't want to fly/jump after player
            direction.Normalize();

            // rotate towards player
            if (direction != Vector3.zero) 
            {
                Quaternion rotateTowards = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotateTowards, 10f*Time.deltaTime);
            }

            moveDirection = direction;
        }
    }

    private void FixedUpdate()
    {
        if (target)
        {
            Vector3 velocity = moveDirection * movementSpeed;
            velocity.y = rb.velocity.y; // attatchs it to gravity if rb gravity is checked
            rb.velocity = velocity;
        }
    }
}
