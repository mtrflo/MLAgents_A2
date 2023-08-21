using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class KickerAgent : Agent
{
    public KickOutEnv kickOutEnv;
    Kicker kicker;
    public override void OnEpisodeBegin()
    {
        if (transform.childCount > 0)
            Destroy(transform.GetChild(0).gameObject);
        KickOutEnv env = Instantiate(kickOutEnv, transform);
        kicker = env.kicker;
    }

    VectorSensor sensor;
    public override void CollectObservations(VectorSensor sensor)
    {
        this.sensor = sensor;
        AddObs(
            kicker.rb.transform.localPosition.x,
            kicker.rb.transform.localPosition.z,
            kicker.rb.velocity.x,
            kicker.rb.velocity.y,
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
    }

    void AddObs(params float[] obs)
    {
        foreach (float item in obs)
        {
            sensor.AddObservation(item);
        }
    }
}
