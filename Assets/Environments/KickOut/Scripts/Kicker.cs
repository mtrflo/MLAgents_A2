using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Kicker : MonoBehaviour
{
    public float reward, term_reward, win_reward;


    public float speed, boostForceMltpy = 2,bounce = 2; // speed of the bot movement
    public float boostCooldown = 4;
    public Rigidbody rb;
    public Rigidbody enemyRB;
    public bool isPlaying = true;
    public bool win = false;
    public Arena arena;
    public bool isFIrst;

    public bool canUseBoost = true;
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

    public int[] PlayerChooseAction()
    {
        int[] playerActions = new int[3];
        #region
        if (Input.GetKey(KeyCode.RightArrow))
            playerActions[0] = 1;
        if (Input.GetKey(KeyCode.LeftArrow))
            playerActions[0] = 2;
        if (Input.GetKey(KeyCode.UpArrow))
            playerActions[1] = 1;
        if (Input.GetKey(KeyCode.DownArrow))
            playerActions[1] = 2;
        if (Input.GetKey(KeyCode.Space))
            playerActions[2] = 1;
        #endregion input
        return playerActions;
    }
    Vector3 movement = Vector3.zero;
    public void MakeAction(int[] actions)
    {
        float horizontal = 0.0f;
        float vertical = 0.0f;
        // horizontal
        switch (actions[0])
        {
            case 0:
                horizontal = 0; 
                break;
            case 1:
                horizontal = 1;
                break;
            case 2:
                horizontal = -1;
                break;
        }
        // vertical
        switch (actions[1])
        {
            case 0:
                vertical = 0;
                break;
            case 1:
                vertical = 1;
                break;
            case 2:
                vertical = -1;
                break;
        }
        
        movement = new Vector3(horizontal, 0.0f, vertical);
        movement *= speed;
        if (actions[2] == 1 && canUseBoost)
        {
            movement *= boostForceMltpy;
            canUseBoost = false;
            Invoke(nameof(BoostTimer), boostCooldown);
        }

        
        rb.AddForce(movement);

        // move the bot

    }
    void BoostTimer()
    {
        canUseBoost = true;
    }
    //private void FixedUpdate()
    //{
    //    if (isNewAction)
    //    {
    //        rb.AddForce(movement * speed);
    //        isNewAction = false;
    //    }
    //}
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
        CancelInvoke(nameof(BoostTimer));
        canUseBoost = true;
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
    //        rb.AddForce(collision.impulse * bounce);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    print("Trigger exit : " + other.tag);
    //    if (other.CompareTag("Arena"))
    //        isPlaying = false;
    //}
}
