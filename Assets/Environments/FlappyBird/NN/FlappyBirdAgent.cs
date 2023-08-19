using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class FlappyBirdAgent : Agent
{

    public static int birdsCount = 0;
    public GameMain gameMain;
    public BirdControl birdControl;
    //public DQNAgent dQNAgent => DQNAgent.me;

    private float startDelay;
    public float delay;

    public float reward = 0.1f, terminateReward = -1f;
    public int distanceRewardCount = 50;
    public static int maxEpisodeCount;
    public int episodeCount;
    public int replaceTargetCount;
    public static int totalEpisodeCount;

    //public TimeController timeController;
    public float epsilon;
    public BirdEnv env;
    public PipeSpawner pipeSpawner;
    private Rigidbody2D rb;
    private void Awake()
    {
        distances = new float[rayPoints.Length];
        startDelay = delay;
    }
    private void Start()
    {
        startPos = birdControl.transform.position;
        startRot = birdControl.transform.rotation;
        rb = birdControl.GetComponent<Rigidbody2D>();
    }

    WaitForSecondsRealtime wfsr;
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

    void MakeAction(int action)
    {
        if (action == 1)
            birdControl.JumpUp();
    }

    void Restart()
    {
        env.Restart();
        Destroy(gameObject);
        //ResetAgent();
        /*
        birdsCount--;
        if (birdsCount <= 0)
        {
            birdsCount = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Destroy(gameObject);
        }
        */
    }
    Vector3 startPos;
    Quaternion startRot;
    public void ResetAgent()
    {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 1;
        episodeCount = 0;
        birdControl.transform.position = startPos;
        birdControl.transform.rotation = startRot;

        birdControl.ResetComponent();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        MakeAction(actions.DiscreteActions[0]);
        SetReward(reward);
    }
    public override void CollectObservations(VectorSensor sensor)
    {

        UpdateRayDistances();
        Vector3 birdPos = birdControl.transform.position;

        sensor.AddObservation(Vector3.Distance(birdPos, pipeSpawner.lastPipe.bottomPoint_l.position));
        sensor.AddObservation(Vector3.Distance(birdPos, pipeSpawner.lastPipe.bottomPoint_r.position));
        sensor.AddObservation(Vector3.Distance(birdPos, pipeSpawner.lastPipe.topPoint_l.position));
        sensor.AddObservation(Vector3.Distance(birdPos, pipeSpawner.lastPipe.topPoint_r.position));
        sensor.AddObservation(distances[0]);
        sensor.AddObservation(distances[1]);
        sensor.AddObservation(distances[2]);
        sensor.AddObservation(distances[3]);
        sensor.AddObservation(rb.velocity.y);

        episodeCount++;
        totalEpisodeCount++;

        if (birdControl.dead && !isDead)
        {
            SetReward(terminateReward);
            isDead = true;
            Restart();
        }

    }
    bool isDead = false;
    public override void Initialize()
    {
        
    }
}
