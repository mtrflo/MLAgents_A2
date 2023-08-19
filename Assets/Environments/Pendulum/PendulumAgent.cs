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
        Time.timeScale = 1;
    }
    void MakeAction(int action)
    {
        float torque = force;
        if (action == 1)
            torque = -force * Time.deltaTime;

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
