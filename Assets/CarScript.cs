using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    public float maxVel = 5f;
    public float accel = 2f;
    public float steer = 2f;
    public float desel = 1f;

    public Rigidbody2D rb;
    private bool isMoving;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Application.targetFrameRate = 12;
    }

    void FixedUpdate()
    {
        /*if(Input.GetKey(KeyCode.W))
        {
            Forward();
        }else if (Input.GetKey(KeyCode.S))
        {
            Backward();
        } else
        {
            rb.velocity = rb.velocity * (1 - desel * Time.fixedDeltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            Steer(1);
        } else if (Input.GetKey(KeyCode.A))
        {
            Steer(-1);
        }
        */

        if(rb.velocity.magnitude > maxVel)
        {
            rb.velocity = rb.velocity.normalized * maxVel;
        }

    }

    public void Forward()
    {
        rb.AddForce(transform.right*accel);
    }

    public void Backward()
    {
        if(rb.velocity.magnitude < 0.5f)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        rb.AddForce(transform.right*-1 * accel * 1.3f);
    }

    public void Steer(int direction)
    {
        rb.MoveRotation(rb.rotation - direction * steer);
        float currentVel = rb.velocity.magnitude;
        rb.velocity = transform.right * currentVel;
    }

    public void ResetVel()
    {
        rb.velocity = Vector2.zero;
    }
}
