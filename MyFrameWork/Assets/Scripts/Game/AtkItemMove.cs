using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkItemMove : MonoBehaviour
{
    public float mspeed;
    protected Vector3 direction;
    protected Rigidbody rd;

    protected virtual void Start()
    {
        direction = -transform.forward;
        rd = GetComponent<Rigidbody>();
    }

    protected virtual void FixedUpdate()
    {
        if (GameRootManager.Instance.isPlay)
        {
            Vector3 pos = rd.position;
            rd.position -= direction * mspeed * Time.fixedDeltaTime;
            rd.MovePosition(pos);
        }
    }
}