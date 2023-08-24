using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Arena : MonoBehaviour
{
    public Kicker k1,k2;
    public Action<GameObject> OnExit;
    public float r;
    private void Awake()
    {
        RandomizeKickerInitPos();
        OnExit = (go) => { RandomizeKickerInitPos(); };
    }
    private void Start()
    {
    }
    private void OnTriggerExit(Collider other)
    {
        //print("arena exit : "+ other.tag);
        if (other.CompareTag(k1.tag) || other.CompareTag(k2.tag))
        {
            OnExit.Invoke(other.gameObject);
            //env.Restart();
        }
    }
    public Vector3 randomPos;
    public void RandomizeKickerInitPos()
    {
        float randomAngle = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI);
        float x = Mathf.Sin(randomAngle);
        float y = Mathf.Cos(randomAngle);

        randomPos = new Vector3(x, 0, y) * r;
    }
}
