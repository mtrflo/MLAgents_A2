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
    public float delay;
    public int teamID;


    public float reward, term_reward, win_reward;


    public bool IsAgent = true;
    public float speed, bounce = 10; // speed of the bot movement
    public Rigidbody rb;
    public Rigidbody enemyRB;
    public bool isPlaying = true;
    public bool win = false;
    public KickOutEnv env;
    public int episodeCount;
    public static int totalEpisodeCount;
    public Arena arena;
    public EnvCon envcon;

    Vector3 startLocPos;
    private void Awake()
    {
        startLocPos = transform.localPosition;
        
    }
    void Start()
    {
        arena.OnExit += (go) =>
        {
            isPlaying = false;
            win = !(go == rb.gameObject);
        };
    }

    int PlayerChooseAction()
    {
        int action = 0;
        if (Input.GetKey(KeyCode.LeftArrow))
            action = 1;
        if (Input.GetKey(KeyCode.RightArrow))
            action = 2;
        if (Input.GetKey(KeyCode.DownArrow))
            action = 3;
        if (Input.GetKey(KeyCode.UpArrow))
            action = 4;
        //MakeAction(action);
        return action;
    }
    public void MakeAction(int i)
    {
        float horizontal = 0.0f;
        float vertical = 0.0f;
        switch (i)
        {
            case 1:
                horizontal = -1.0f; break;
            case 2:
                horizontal = 1.0f; break;
            case 3:
                vertical = -1.0f; break;
            case 4:
                vertical = 1.0f; break;
        }

        // move the bot
        Vector3 movement = new Vector3(horizontal, 0.0f, vertical);
        rb.AddForce(movement * speed);
    }
    public void ResetToStart()
    {
        //if (rb.velocity.magnitude > 0.01f)
        //{
            rb.velocity = Vector3.zero;

            transform.localPosition = startLocPos;
            isPlaying = true;
        //}
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
