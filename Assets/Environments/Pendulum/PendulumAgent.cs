using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.UIElements;

public class PendulumAgent : Agent
{
    public Rigidbody rb;
    public float reward = 0.1f, terminateReward = -1f, scoreReward = 1, distanceReward = 0f;
    public Transform point;
    public float force;
    static int oid;
    public float epsilon;
    private void Awake()
    {
        epsilon = Mathf.Lerp(0, 1, oid / (float)MultiEnviroment.me.count);
        oid++;
    }
    private void Start()
    {
        //InvokeRepeating(nameof(End), 0, 10);
        Time.timeScale = 1;
    }
    // void End()
    // {
    //     EndEpisode();
    // }

    void ChooseAction()
    {
        //current_state.Clear();

        //Vector3 toPo = (point.position - transform.position).normalized;
        //float angle = Vector3.Angle(toPo, Vector3.up);
        //float sign = MathF.Sign(point.position.x - transform.position.x);
        //if(sign > 0)
        //{
        //    angle = 180 + (180 - angle);
        //}
        //angle = angle * Mathf.Deg2Rad;


        //SetOb(Mathf.Sin(angle) / Mathf.PI * 2);
        //AddObservation(Mathf.Cos(angle) / Mathf.PI * 2);
        //float angvel = rb.angularVelocity.magnitude * Mathf.Sign(rb.angularVelocity.z);
        //AddObservation(angvel / 4);


        //if (prev_state.Count == 0)
        //    Utils.CopyTo(current_state, prev_state);


        //float s_reward = Mathf.Cos(angle);
        //lastAngle = angle;

        //_Transition.Set(prev_state.ToArray(), action, current_state.ToArray(), s_reward);
        //Utils.CopyTo(current_state, prev_state);
        //action = RLAlg.SelectAction(current_state.ToArray(), epsilon);
        //MakeAction(action);
        //Learn();
        //episodeCount++;
    }
    void MakeAction(int action)
    {
        float torque = force;
        if (action == 1)
            torque = -force;

        rb.AddTorque(0, 0, torque);
    }
    float angle;
    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 toPo = (point.position - transform.position).normalized;
        angle = Vector3.Angle(toPo, Vector3.up);
        float sign = MathF.Sign(point.position.x - transform.position.x);
        if (sign > 0)
            angle = 180 + (180 - angle);
        angle = angle * Mathf.Deg2Rad;

        float angvel = rb.angularVelocity.magnitude * Mathf.Sign(rb.angularVelocity.z);

        float ob1_sin = Mathf.Sin(angle) / Mathf.PI * 2;
        float ob2_cos = Mathf.Cos(angle) / Mathf.PI * 2;
        float ob3_angvel4 = angvel / 4;

        sensor.AddObservation(ob1_sin);
        sensor.AddObservation(ob2_cos);
        sensor.AddObservation(ob3_angvel4);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        MakeAction(actions.DiscreteActions[0]);
        SetReward(Mathf.Cos(angle));
    }

}
