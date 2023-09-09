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
        
        sensor.AddObservation(kicker.canUseBoost);
        AddObservationPack(
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
        
        //discrete
        //int[] d_actions = { actions.DiscreteActions[0] , actions.DiscreteActions[1] , actions.DiscreteActions[2] };
        //kicker.Make_C_Action(d_actions);

        //cont
        float angle = actions.ContinuousActions[0];
        //print("con" + angle);
        float il = Mathf.InverseLerp(-1,1,angle);
        //print("il : " + il);
        angle = Mathf.Lerp(0, 360, il);
        //print("deg angle" + angle);

        int boost = actions.DiscreteActions[0];
        kicker.Make_C_Action(angle, boost);

        float s_reward = kicker.isPlaying ? kicker.reward : (kicker.win ? kicker.win_reward : kicker.term_reward);
        SetReward(s_reward);
        // print(kicker.name + " : reward : " + s_reward);
        if (!kicker.isPlaying)
            EndEpisode();
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //discrete
        //ActionSegment<int> discAct = actionsOut.DiscreteActions;
        //int[] dir = kicker.PlayerChooseAction();
        //discAct[0] = dir[0];
        //discAct[1] = dir[1];
        //discAct[2] = dir[2];

        //cont
        ActionSegment<int> discAct = actionsOut.DiscreteActions;
        (float, int) hact = kicker.PlayerChooseActionCont();
        discAct[0] = hact.Item2;
        ActionSegment<float> conAct = actionsOut.ContinuousActions;
        conAct[0] = hact.Item1;
    }
    void AddObservationPack(params float[] obs)
    {
        // print(kicker.name + " : " + obs.ToCommaSeparatedString());
        foreach (float item in obs)
        {
            sensor.AddObservation(item);
        }
    }
}
