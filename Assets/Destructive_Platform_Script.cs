﻿using UnityEngine;
using System.Collections;

public class Destructive_Platform_Script : MonoBehaviour {

    public Vector3 forceToApply = Vector3.zero;
    public Vector3 pointToApplyForce = Vector3.zero;
    public float forceMagnitude = 0.0f;
    public Vector3 constantVectorForce = Vector3.zero;
    public float timeToApplyCF = 0.0f;

    private float startTime = 0.0f;

	// Use this for initialization
	void Start () {
        forceToApply.Normalize();
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - startTime < timeToApplyCF)
        {
            this.rigidbody.AddForce(constantVectorForce * -UnityEngine.Physics.gravity.y);
        }
	}

    public void ApplyStagedForce()
    {
        this.rigidbody.AddForceAtPosition(forceToApply * forceMagnitude * -UnityEngine.Physics.gravity.y, pointToApplyForce + this.transform.position);
        startTime = Time.time;
    }
}
