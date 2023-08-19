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
    public BirdControl birdControl;

    public float reward = 0.1f, terminateReward = -1f;

    //public TimeController timeController;
    public float epsilon;
    public BirdEnv prefab_env;
    public PipeSpawner pipeSpawner;
    void MakeAction(int action)
    {
        if (action == 1)
            birdControl.JumpUp();
    }

    public override void OnEpisodeBegin()
    {
        if (transform.childCount > 0)
            Destroy(transform.GetChild(0).gameObject);
        BirdEnv env = Instantiate(prefab_env,transform);
        birdControl = env.birdControl;
        pipeSpawner = env.pipeSpawner;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        MakeAction(actions.DiscreteActions[0]);
        SetReward(reward);

        if (birdControl.dead)
        {
            SetReward(terminateReward);
            EndEpisode();
        }
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        #region Observation
        float[] distances = birdControl.GetDistances();
        Vector3 birdPos = birdControl.transform.position;
        PipeMove lastPipe = pipeSpawner.lastPipe;
        if (lastPipe == null)
            return;
        sensor.AddObservation(Vector3.Distance(birdPos, lastPipe.bottomPoint_l.position));
        sensor.AddObservation(Vector3.Distance(birdPos, lastPipe.bottomPoint_r.position));
        sensor.AddObservation(Vector3.Distance(birdPos, lastPipe.topPoint_l.position));
        sensor.AddObservation(Vector3.Distance(birdPos, lastPipe.topPoint_r.position));
        sensor.AddObservation(distances[0]);
        sensor.AddObservation(distances[1]);
        sensor.AddObservation(distances[2]);
        sensor.AddObservation(distances[3]);
        sensor.AddObservation(birdControl.rb.velocity.y);
        #endregion
    }
}
