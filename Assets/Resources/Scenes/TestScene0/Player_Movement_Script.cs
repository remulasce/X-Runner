﻿using UnityEngine;
using System.Collections;

public class Player_Movement_Script : MonoBehaviour {

    public float movementSpeed;
    public float forceValuePostJump;
    public float forceValuePreJump;

    public bool isJumping = true;
    public bool canJump = true;

    public bool isActive = true;
    public float timeJumpThreshold = 0.5f; // Will be used to allow for when the script can check if the player is in the air    

    [HideInInspector]
    public bool isInAir = false; // If the player is in the air, the camera will follow in y (prevents jenky camera movement)

    public bool isJetPackActive = false; // Will allow the player to float for longer when this is active
    public float jetPackFloatingForce = 0.0f; // This force will be added to the player's regular floating velocity

    // Values for being able to move left and right
    [System.Serializable]
    public class HorizontalMovementData
    {
        [Range(-5.0f, 0.0f)]
        public float leftMovementThreshold = 0.0f;

        [Range(0.0f, 20.0f)]
        public float rightMovementThreshold = 0.0f;

        [Range(0.0f, 15.0f)]
        public float movementSpeed = 0.0f;

        public float playerOffset = 0.0f;
    }

    public HorizontalMovementData horizontalMovement = new HorizontalMovementData();

    private float timeWhenLastJumped = 0.0f; // Will check when the player last jumped
    private bool hitWallSideways = false;

	// Use this for initialization
	void Start () {
		//We should persist for the next level transition
		//DontDestroyOnLoad(this);		not any more, because that was bogus.
	}
	
	void DoXVelocity()
	{
        if (!hitWallSideways)
        {
            this.transform.position += new Vector3(movementSpeed * Time.deltaTime, 0, 0);
        }
	}
	
	void DoJump()
	{
		if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space)) && canJump)
        {
            this.rigidbody.velocity = Vector3.zero;
            this.rigidbody.AddForce(new Vector3(0, forceValuePreJump, 0));
            isJumping = true;
			canJump = false;
            isInAir = true;
            timeWhenLastJumped = Time.time;
			
			//Application.LoadLevelAdditive("test_add_scene");
			//Application.LoadLevel("test_add_scene");
        }
		if ((Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.Space)) && isJumping)
        {
            isJumping = false;
        }
		
		//Jump farther if we keep the space bar held down longer.
		if (isJumping)
		{
			this.rigidbody.AddForce(new Vector3(0, forceValuePostJump, 0));
            if (isJetPackActive) // Add in the extra jetpack force if the player has attained it
            {
                this.rigidbody.AddForce(new Vector3(0, jetPackFloatingForce, 0));
            }
		}
	}

    void DoSideWaysMovement()
    {
        if ((isJumping || isInAir) && !isJetPackActive) // Player cannot move while in jump, unless the jetpack has been attained
        {
            return;
        }        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (horizontalMovement.playerOffset > horizontalMovement.leftMovementThreshold)
            {
                horizontalMovement.playerOffset -= horizontalMovement.movementSpeed * Time.deltaTime;
                this.transform.position -= new Vector3(horizontalMovement.movementSpeed * Time.deltaTime, 0, 0);
            }
            else
            {
                horizontalMovement.playerOffset = horizontalMovement.leftMovementThreshold;
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (horizontalMovement.playerOffset < horizontalMovement.rightMovementThreshold)
            {
                horizontalMovement.playerOffset += horizontalMovement.movementSpeed * Time.deltaTime;
                this.transform.position += new Vector3(horizontalMovement.movementSpeed * Time.deltaTime, 0, 0);
            }
            else 
            {
                horizontalMovement.playerOffset = horizontalMovement.rightMovementThreshold;
            }
        }        
        //Debug.Log("Movement Offset: " + horizontalMovement.playerOffset);
    }
	
	//If we're below the ground, respawn us above it and (ahead) us a little.
	//	Ahead for the vertical slice since it's not possible to win if you don't
	//	have the triggers set properly.
	void CheckDead()
	{
		if (this.transform.position.y < -10)
		{
            this.rigidbody.velocity = Vector3.zero;
            this.transform.position = new Vector3(this.transform.position.x, 1, 0);
            this.rigidbody.AddForce(new Vector3(500, 0, 0));
            hitWallSideways = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
        if (!isActive) {return;}

        DoSideWaysMovement();
		DoXVelocity();
		DoJump();
		CheckDead();

        //print(rigidbody.velocity);
    }

    void OnCollisionEnter(Collision other) 
    {
        //Tags are precompiled into an enum, so they don't have to do string operations.
		//Check to make sure the thing we hit was below us	

        //Debug.Log(other.contacts[0].normal);

        if (other.contacts[0].normal.y > 0)
        {
			//print ("Can Jump Now "+Time.time);
            canJump = true;
            isJumping = false;
            hitWallSideways = false;
            if (isInAir && (Time.time - timeWhenLastJumped > 0.5f))
            {
                //print("Not in air anymore"+Time.time);
                isInAir = false;
            }
        }

        if (other.contacts[0].normal.x < -0.8 && other.gameObject.CompareTag("Terrain") && !hitWallSideways) // Then stop -- you hit the wall while jumping
        {
            hitWallSideways = true;
        }
		
		//Next level stuff: Persist some stuff for the next level
		if (other.gameObject.CompareTag("Ship") && Application.loadedLevelName == "test_scene_0")
		{
			_Loader_L0.loader.EndLevel();
		}
    }

    /*ADDERESS THIS LATER*/
    //void OnCollisionExit(Collision other)
    //{
    //    canJump = false; // Will be set to false when the player leaves a platform so a jump in midair cannot occur 
    //}
}

