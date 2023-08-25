using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Kicker : MonoBehaviour
{
    public float reward, term_reward, win_reward;


    public float speed; // speed of the bot movement
    public Rigidbody rb;
    public Rigidbody enemyRB;
    public bool isPlaying = true;
    public bool win = false;
    public Arena arena;
    public bool isFIrst;

    Vector3 startLocPos;
    private void Awake()
    {
        startLocPos = transform.localPosition;
    }
    void Start()
    {
        startLocPos = transform.localPosition;
        arena.OnExit += (go) =>
        {
            isPlaying = false;
            win = !(go == rb.gameObject);
        };
    }

    public int PlayerChooseAction()
    {
        int action = -1;
        if (Input.GetKey(KeyCode.LeftArrow))
            action = 0;
        if (Input.GetKey(KeyCode.RightArrow))
            action = 1;
        if (Input.GetKey(KeyCode.DownArrow))
            action = 2;
        if (Input.GetKey(KeyCode.UpArrow))
            action = 3;
        if(Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow))
            action = 4;
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow))
            action = 5;
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.DownArrow))
            action = 6;
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.UpArrow))
            action = 7;
        //MakeAction(action);
        return action;
    }
    Vector3 movement = Vector3.zero;
    bool isNewAction = false;
    public void MakeAction(int i)
    {
        float horizontal = 0.0f;
        float vertical = 0.0f;
        switch (i)
        {
            case 0:
                horizontal = -1.0f; break;
            case 1:
                horizontal = 1.0f; break;
            case 2:
                vertical = -1.0f; break;
            case 3:
                vertical = 1.0f; break;
            case 4:
                horizontal = 1;
                vertical = -1;
                break;
            case 5:
                horizontal = -1;
                vertical = 1;
                break;
            case 6:
                horizontal = -1;
                vertical = -1;
                break;
            case 7:
                horizontal = 1;
                vertical = 1;
                break;
        }
        movement = new Vector3(horizontal, 0.0f, vertical);
        isNewAction = true;
        // move the bot

    }
    private void FixedUpdate()
    {
        if (isNewAction)
        {
            rb.AddForce(movement * speed);
            isNewAction = false;
        }
    }
    public void ResetToStart()
    {
        //reset physics
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        //Reset locaLpos
        Vector3 rp = arena.randomPos;
        rp = isFIrst ? rp : -rp;
        rp.y = startLocPos.y;
        transform.localPosition = rp;
        isPlaying = true;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        //print("-rb.velocity  : " + (-rb.velocity));
    //        //print("collision.impulse : " + collision.impulse);
    //        Vector3 dir = rb.transform.position - enemyRB.transform.position;
    //        //rb.velocity = collision.rigidbody.velocity;
    //        //rb.AddForce(dir * ( rb.velocity.magnitude + collision.rigidbody.velocity.magnitude ) * speed * bounce);
    //        //rb.AddForce(collision.impulse * bounce);
    //        //rb.AddForce(-collision.impulse * 300);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    print("Trigger exit : " + other.tag);
    //    if (other.CompareTag("Arena"))
    //        isPlaying = false;
    //}
}
