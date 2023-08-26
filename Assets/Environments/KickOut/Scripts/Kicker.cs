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

    public Vector2Int PlayerChooseAction()
    {
        Vector2Int dir = Vector2Int.zero;
        #region
        if (Input.GetKey(KeyCode.RightArrow))
            dir.x = 1;
        if (Input.GetKey(KeyCode.LeftArrow))
            dir.x= 2;
        if (Input.GetKey(KeyCode.UpArrow))
            dir.y = 1;
        if (Input.GetKey(KeyCode.DownArrow))
            dir.y = 2;
        #endregion input
        //MakeAction(x, y);
        return dir;
    }
    Vector3 movement = Vector3.zero;
    bool isNewAction = false;
    public void MakeAction(int x, int y)
    {
        float horizontal = 0.0f;
        float vertical = 0.0f;
        switch (x)
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
        switch (y)
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
        isNewAction = true;
        rb.AddForce(movement * speed);

        // move the bot

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
