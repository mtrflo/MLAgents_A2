using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Barracuda;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;

public class KickerAgent : Agent
{
    public Kicker kicker;
    public BehaviorParameters behaviorParameters;
    public KickOutEnv kickOutEnv;
    private bool isInference => behaviorParameters.BehaviorType == BehaviorType.InferenceOnly;
    private bool isHeruistic => behaviorParameters.BehaviorType == BehaviorType.HeuristicOnly;
    private bool isDefault => behaviorParameters.BehaviorType == BehaviorType.Default;
    public bool isGameStarted => kickOutEnv.isGameStarted;
    public override void OnEpisodeBegin()
    {
        kickOutEnv.LevelStarted();
        kicker?.ResetToStart();
        print("OnEpisodeBegin");
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
        if (!isDefault && !isGameStarted)
            return;
        
        // print(kicker.name + " action: " + actions.DiscreteActions[0]);
        int[] d_actions = { actions.DiscreteActions[0] , actions.DiscreteActions[1] , actions.DiscreteActions[2] };
        kicker.MakeAction(d_actions);
        float s_reward = kicker.isPlaying ? kicker.reward : (kicker.win ? kicker.win_reward : kicker.term_reward);
        SetReward(s_reward);
        // print(kicker.name + " : reward : " + s_reward);
        if (!kicker.isPlaying)
            EndEpisode();
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        if (!isDefault && !isGameStarted)
            return;

        ActionSegment<int> discAct = actionsOut.DiscreteActions;
        int[] dir = kicker.PlayerChooseAction();
        discAct[0] = dir[0];
        discAct[1] = dir[1];
        discAct[2] = dir[2];
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
;