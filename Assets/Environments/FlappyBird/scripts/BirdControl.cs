using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;

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

    private Sequence birdSequence;

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
    // Use this for initialization
    void Start () {

        startPos = transform.position;
        startRot = transform.rotation;
        animator = GetComponent<Animator>();
        float birdOffset = 0.05f;
        float birdTime = 0.3f;
        float birdStartY = transform.position.y;

        birdSequence = DOTween.Sequence();

        birdSequence.Append(transform.DOMoveY(birdStartY + birdOffset, birdTime).SetEase(Ease.Linear))
            .Append(transform.DOMoveY(birdStartY - 2 * birdOffset, 2 * birdTime).SetEase(Ease.Linear))
            .Append(transform.DOMoveY(birdStartY, birdTime).SetEase(Ease.Linear))
            .SetLoops(-1);
        inGame = true;
        JumpUp();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!inGame)
        {
            return;
        }
        birdSequence.Kill();

        if (!dead)
		{
			if (Input.GetButtonDown("Fire1"))
			{
                JumpUp();
			}
		}

		if (!landed)
		{
			float v = rb.velocity.y;
			
			float rotate = Mathf.Min(Mathf.Max(-90, v * rotateRate + 60), 30);
			
			transform.rotation = Quaternion.Euler(0f, 0f, rotate);
		}
		else
		{
			rb.rotation = -90;
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.name == "land" || other.name == "pipe_up" || other.name == "pipe_down")
		{
            //if (other.name == "pipe_up" || other.name == "pipe_down")
            //{
            //    pipe = other.transform.parent.GetComponent<PipeMove>().nextPipe;
            //    //Destroy(other.transform.parent.gameObject);
            //}
            if (!dead)
            {
                animator.SetTrigger("die");
				GameOver();
				OnDie();
            }
			

			if (other.name == "land")
			{
                rb.gravityScale = 0;
                rb.velocity = new Vector2(0, 0);

				landed = true;
			}
		}

        if (other.name == "pass_trigger")
        {
            scoreMgr.AddScore();
            pipeSpawner.PipePassed();
            OnPipePassed();
            AudioSource.PlayClipAtPoint(score, Vector3.zero);
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

        //FlappyBirdAgent.birdsCount--;
        //if (FlappyBirdAgent.birdsCount <= 0)
        //{
        //    FlappyBirdAgent.birdsCount = 0;
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //}
    }

    public void ResetComponent()
    {
        animator.ResetTrigger("die");
        dead = false;
        landed = false;
    }

    private void OnDestroy()
    {
        birdSequence.Kill();
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
