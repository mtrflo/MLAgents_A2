using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
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
        float distance = Vector3.Distance(kicker.rb.transform.position, kicker.enemyRB.transform.position);
        AddObs(
            kicker.rb.transform.localPosition.x,
            kicker.rb.transform.localPosition.z,
            kicker.rb.velocity.x,
            kicker.rb.velocity.z,
            kicker.enemyRB.transform.localPosition.x,
            kicker.enemyRB.transform.localPosition.z,
            kicker.enemyRB.velocity.x,
            kicker.enemyRB.velocity.z,
            distance
            );
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // print(kicker.name + " action: " + actions.DiscreteActions[0]);
        kicker.MakeAction(actions.DiscreteActions[0], actions.DiscreteActions[1]);
        float s_reward = kicker.isPlaying ? kicker.reward : (kicker.win ? kicker.win_reward : kicker.term_reward);
        SetReward(s_reward);
        // print(kicker.name + " : reward : " + s_reward);
        if (!kicker.isPlaying)
            EndEpisode();
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discAct = actionsOut.DiscreteActions;
        Vector2Int dir = kicker.PlayerChooseAction();
        // print("x : " + dir.x);
        // print("y : " + dir.y);
        discAct[0] = dir.x;
        discAct[1] = dir.y;
        // print("discAct[0] : "+ discAct[0]);
        // print("discAct[1] : "+ discAct[1]);
    }
    void AddObs(params float[] obs)
    {
        // print(kicker.name + " : " + obs.ToCommaSeparatedString());
        foreach (float item in obs)
        {
            sensor.AddObservation(item);
        }
    }
}
