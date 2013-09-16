﻿using UnityEngine;
using System.Collections;

public class Player_Movement_Script : MonoBehaviour {

    public float movementSpeed;
    public float forceValuePostJump;
    public float forceValuePreJump;

    public bool isJumping = true;
    public bool canJump = true;

    public bool isActive = true;

	// Use this for initialization
	void Start () {
	
	}
	
	void DoXVelocity()
	{
		this.transform.position += new Vector3(movementSpeed * Time.deltaTime, 0, 0);
	}
	
	void DoJump()
	{
		if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space)) && canJump)
        {
            this.rigidbody.AddForce(new Vector3(0, forceValuePreJump, 0));
            isJumping = true;
			canJump = false;
        }
		if ((Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.Space)) && isJumping)
        {
            isJumping = false;
        }
		
		//Jump farther if we keep the space bar held down longer.
		if (isJumping)
		{
			this.rigidbody.AddForce(new Vector3(0, forceValuePostJump, 0));
		}
	}
	
	// Update is called once per frame
	void Update () {
        if (!isActive) {return;}
        
		
		DoXVelocity();
		DoJump();
		

    }

    void OnCollisionEnter(Collision other) 
    {
		//Tags are precompiled into an enum, so they don't have to do string operations.
		//Check to make sure the thing we hit was below us
		//Also, don't bother.
        if (/*other.gameObject.CompareTag("Platform") &&*/ other.contacts[0].normal.y > 0)
        {
			print ("Can Jump Now "+Time.time);
            canJump = true;
            isJumping = false;
        }
    }
}
