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
    Quaternion startRot;
    private void Awake()
    {
        if(MultiEnviroment.me)
            epsilon = Mathf.Lerp(0, 1, oid / (float)MultiEnviroment.me.count);
        oid++;
        startRot = rb.transform.rotation;
    }
    private void Start()
    {
        Time.timeScale = 1;
    }
    void MakeAction(int action)
    {
        // print("make action")
        float torque = 0;
        if (action == 1)
            torque = -force;
        if (action == 2)
            torque = force;
        rb.AddTorque(0, 0, torque);
    }
    float angle;
    public override void OnEpisodeBegin()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.transform.rotation = startRot;
    }
    //public override void Initialize()
    //{
    //    rb.velocity = Vector3.zero;
    //    rb.angularVelocity = Vector3.zero;
    //    transform.rotation = startRot;
    //}
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
        //print("StepCount : " + StepCount);
        //print("MaxStep : " + MaxStep);

        if (MaxStep == StepCount)
            EndEpisode();
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discAct = actionsOut.DiscreteActions;
        discAct[0] = 0;
        if (Input.GetKey(KeyCode.LeftArrow))
            discAct[0] = 1;
        if (Input.GetKey(KeyCode.RightArrow))
            discAct[0] = 2;

    }
}
