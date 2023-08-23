using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class KickerAgent : Agent
{
    public Kicker kicker;
    public override void OnEpisodeBegin()
    {
        kicker?.ResetToStart();
    }

    VectorSensor sensor;
    public override void CollectObservations(VectorSensor sensor)
    {
        this.sensor = sensor;
        AddObs(
            kicker.rb.transform.localPosition.x,
            kicker.rb.transform.localPosition.z,
            kicker.rb.velocity.x,
            kicker.rb.velocity.z,
            kicker.enemyRB.transform.localPosition.x,
            kicker.enemyRB.transform.localPosition.z,
            kicker.enemyRB.velocity.x,
            kicker.enemyRB.velocity.z
            );
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        kicker.MakeAction(actions.DiscreteActions[0]);
        float s_reward = kicker.isPlaying ? kicker.reward : (kicker.win ? kicker.win_reward : kicker.term_reward);
        SetReward(s_reward);
        //print("reward : " + s_reward);
        if (!kicker.isPlaying)
            EndEpisode();
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discAct = actionsOut.DiscreteActions;
        discAct[0] = kicker.PlayerChooseAction();
    }
    void AddObs(params float[] obs)
    {
        foreach (float item in obs)
        {
            sensor.AddObservation(item);
        }
    }
}
