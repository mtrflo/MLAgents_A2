using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;
using UnityEngine.PlayerLoop;
using Unity.VisualScripting;

public class BirdControl : MonoBehaviour {

	public int rotateRate = 10;
	public float upSpeed = 10;
    public ScoreMgr scoreMgr;

    public AudioClip jumpUp;
    public AudioClip hit;
    public AudioClip score;
    public AudioSource a_source;
    public PipeSpawner pipeSpawner;

    public bool inGame = false;

	public bool dead = false;

	private bool landed = false;


	public Action OnDie;
    public Action OnPipePassed;
	private Animator animator;
    public Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        distances = new float[rayPoints.Length];

        OnDie = () => { };
        
        OnPipePassed = () => { };
    }
    Quaternion tartRot;
    // Use this for initialization
    void Start () {

        startPos = transform.position;
        startRot = transform.rotation;
        animator = GetComponent<Animator>();
        tartRot = transform.rotation;


        inGame = true;
        JumpUp();
    }

    bool isNewAction = false;
    public int lastAction = -1;
	void FixedUpdate () 
    {
		if (!landed)
		{
			float v = rb.velocity.y;
			
			float rotate = Mathf.Min(Mathf.Max(-90, v * rotateRate + 60), 30);
			
			transform.rotation = Quaternion.Euler(0f, 0f, rotate);
		}

        //if (isNewAction)
        //{
        //    if (lastAction > 0)
        //    {
        //        JumpUp();
        //    }
        //    isNewAction = false;
        //}
        //if (!dead && Input.GetButtonDown("Fire1"))
        //{
        //    JumpUp();
        //}
    }
    public void MakeAction(int action)
    {
        isNewAction = true;
        lastAction = action;
        if (action > 0)
        {
            JumpUp();
        }
    }
    void OnTriggerEnter2D (Collider2D other)
	{
		if (other.name == "land" || other.name == "pipe_up" || other.name == "pipe_down")
		{
            if (!dead)
            {
                //animator.SetTrigger("die");
				GameOver();
				OnDie();
            }

			if (other.name == "land")
			{
                rb.gravityScale = 0;
                rb.velocity = new Vector2(0, 0);
                rb.rotation = -90;
                landed = true;
			}
		}

        if (other.name == "pass_trigger")
        {
            scoreMgr.AddScore();
            pipeSpawner.PipePassed();
            Scoring.me.NewScore(scoreMgr.currentScore);
            OnPipePassed();
            a_source.PlayOneShot(score);
        }
	}

    float[] distances;
    public Transform[] rayPoints;
    void UpdateRayDistances()
    {
        for (int i = 0; i < distances.Length; i++)
            distances[i] = GetRayLength(rayPoints[i]);
    }
    private float GetRayLength(Transform point)
    {
        float dstns = -1;
        RaycastHit2D hit = Physics2D.Raycast(point.transform.position, point.right, 10, ~LayerMask.GetMask("bird"));
        if (hit.collider != null)
        {
            dstns = hit.distance;
        }
        return dstns;
    }
    public float[] GetDistances()
    {
        UpdateRayDistances();
        return distances;
    }
    public void JumpUp()
    {
        rb.velocity = new Vector2(0, upSpeed);
        a_source.PlayOneShot(jumpUp);
    }
	
	public void GameOver()
	{
		dead = true;
    }

    public void ResetComponent()
    {
        animator.ResetTrigger("die");
        dead = false;
        landed = false;
    }

    Vector3 startPos;
    Quaternion startRot;
    public void ResetAgent()
    {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 1;
        transform.position = startPos;
        transform.rotation = startRot;
        ResetComponent();
    }
}
