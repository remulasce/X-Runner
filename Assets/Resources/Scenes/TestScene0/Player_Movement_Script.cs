using UnityEngine;
using System.Collections;
using System;

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

    // Variable to prevent bug with player getting stuck to the wall.
    private const int maxWallFrameHits = 10;
    private int numberOfFramesHit = 0;

    // Make a reference to the camera
    public CanabaltCamera mainCamera;

    // ray cast distance for ground check
    private float rayCastDistance = 3.0f;

    // Reference to gravity script
    private Player_Gravity_Script playerGravityScript;

    // Reference to all of the spawners
    public GameObject[] spawners;

    // Reference to Stats
    Stat_Counter_Script stats = null;

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

        [Range(-100.0f, 0.0f)]
        public float accelerationPushOffWall = 0.0f;

        public bool isSliding = false;
    }

    public HorizontalMovementData horizontalMovement = new HorizontalMovementData();

    private float timeWhenLastJumped = 0.0f; // Will check when the player last jumped
    private bool onWall = false; //For wall-jumping. If you're good, you can wall jump indefinitely.
    public int wallJumpsAllowed = 3;
	public int wallJumpsLeft = 3;

    // Used for Spawning
    public bool isDead = false;
    public float spawnTime = 1.0f;

    private bool isWaitingToSpawn = false;
	
	// Use this for initialization
	void Start () {
		//We should persist for the next level transition
		//DontDestroyOnLoad(this);		not any more, because that was bogus.
        playerGravityScript = this.gameObject.GetComponent<Player_Gravity_Script>();
        wallJumpsLeft = wallJumpsAllowed;

        spawners = GameObject.FindGameObjectsWithTag("Spawnpoint");

        GameObject[] tempArray = new GameObject[spawners.Length];
        for (int i = 0; i < tempArray.Length; i++)
        {
            for (int j = 0; j < spawners.Length; j++)
            {                
                if (spawners[j].name.Contains((i + 1).ToString()))
                {
                    /*SpawnPoint_ = Length 11 + the number of numerals afterward */
                    if ((spawners[j].name.Length == 14 && (i + 1).ToString().Length == 3)
                        || (spawners[j].name.Length == 13 && (i + 1).ToString().Length == 2)
                        || (spawners[j].name.Length == 12 && (i + 1).ToString().Length == 1))
                    {
                        //print((i + 1) + " " + spawners[j].name);
                        tempArray[i] = spawners[j];
                        break;
                    }                    
                }
            }
        }

        spawners = tempArray;

        stats = GameObject.FindGameObjectWithTag("Stats").GetComponent<Stat_Counter_Script>();
	}
	
	void DoXVelocity()
	{
        if (isDead) // Don't move the player if dead
        {
            return;
        }
        
        if (!onWall)
        {
            //this.transform.position += new Vector3(movementSpeed * Time.deltaTime, 0, 0);
			this.rigidbody.MovePosition(this.transform.position + new Vector3(movementSpeed * Time.deltaTime, 0, 0));
        }

        RaycastHit hit;

        Ray[] rays = new Ray[3];
        rays[0] = new Ray(this.transform.position, new Vector3(0, -1, 0));
        rays[1] = new Ray(new Vector3(this.transform.position.x - 0.75f, this.transform.position.y, this.transform.position.z), new Vector3(0, -1, 0));
        rays[2] = new Ray(new Vector3(this.transform.position.x + 0.75f, this.transform.position.y, this.transform.position.z), new Vector3(0, -1, 0));

        if (playerGravityScript.isGravityInverted) // Make sure to invert the direction of the rays if gravity is reversed, because the ground will be above the player
        {
            for (int i = 0; i < rays.Length; i++)
            {
                rays[i].direction *= -1;
            }
        }
		
		
        int numberOfRaysHitGround = 0;

        // Check to see if there is a floor below the player for all of the rays
        for (int i = 0; i < rays.Length; i++)
        {
            if (!Physics.Raycast(rays[i], out hit, rayCastDistance))
            {
                Debug.DrawRay(rays[i].origin, rays[i].direction * rayCastDistance, Color.red, 10.0f);
            }
            else
            {
                Debug.DrawRay(rays[i].origin, rays[i].direction * rayCastDistance, Color.cyan, 10.0f);
                numberOfRaysHitGround++;
            }
        }

        if (numberOfRaysHitGround == 0)
        {
            isInAir = true;
            canJump = false;
        }
        else
        {
            ////print ("Can Jump Now "+Time.time);
            //canJump = true;
            //isJumping = false;
            //onWall = false;
            //wallJumpsLeft = 3;
            //if (isInAir && (Time.time - timeWhenLastJumped > 0.25f))
            //{
            //    //print("Not in air anymore"+Time.time);
            //    isInAir = false;
            //}

            //if (isJetPackActive)
            //{
            //    this.GetComponentInChildren<ParticleSystem>().Stop();
            //}
        }
	}
	
	void DoJump()
	{
		//Save ourselves with a wall jump
		if (!canJump && onWall && wallJumpsLeft >= 1 && (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space)))
		{
			canJump = true;
			wallJumpsLeft--;
			onWall = false;
		}
		if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space)) && canJump)
        {
            if (!horizontalMovement.isSliding)
            {
                this.rigidbody.velocity = Vector3.zero;
            }
            else
            {
                horizontalMovement.isSliding = false;
            }
            if (!playerGravityScript.isGravityInverted)
            {
                this.rigidbody.AddForce(new Vector3(0, forceValuePreJump, 0));
            }
            else
            {
                this.rigidbody.AddForce(new Vector3(0, -forceValuePreJump, 0));
            }
            isJumping = true;
			canJump = false;
            isInAir = true;
            timeWhenLastJumped = Time.time;

            //if (!isJetPackActive)
            //{
            //    this.GetComponentInChildren<ParticleSystem>().startSpeed = 4.5f;
            //}
            //else
            //{
            //    this.GetComponentInChildren<ParticleSystem>().startSpeed = 9.0f;
            //}

            this.GetComponentInChildren<ParticleSystem>().Play();
			
			//Application.LoadLevelAdditive("test_add_scene");
			//Application.LoadLevel("test_add_scene");
        }
		if ((Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.Space)) && isJumping)
        {
            isJumping = false;
            //if (isJetPackActive)
            //{
            this.GetComponentInChildren<ParticleSystem>().Stop();
            //}
        }
		
		//Jump farther if we keep the space bar held down longer.
		if (isJumping)
		{
            if (!playerGravityScript.isGravityInverted)
            {
                this.rigidbody.AddForce(new Vector3(0, forceValuePostJump * Time.deltaTime, 0));
            }
            else
            {
                this.rigidbody.AddForce(new Vector3(0, -forceValuePostJump * Time.deltaTime, 0));
            }
            if (isJetPackActive) // Add in the extra jetpack force if the player has attained it
            {
                if (!playerGravityScript.isGravityInverted)
                {
                    this.rigidbody.AddForce(new Vector3(0, jetPackFloatingForce * Time.deltaTime, 0));
                }
                else
                {
                    this.rigidbody.AddForce(new Vector3(0, -jetPackFloatingForce * Time.deltaTime, 0));
                }
            }
		}
	}

    void DoSideWaysMovement()
    {
        if ((isJumping || isInAir) && !isJumping) // Player cannot move while in jump, unless the jetpack has been attained
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

    // Respawn Function
    IEnumerator Respawn()
    {
        isWaitingToSpawn = true;
        yield return new WaitForSeconds(spawnTime);
        isWaitingToSpawn = false;
        this.rigidbody.velocity = Vector3.zero;
        this.transform.position = new Vector3(this.transform.position.x, 1, 0);
        // Run through all of the spawners and see where the correct one to spawn is
        for (int i = 0; i < spawners.Length; i++)
        {
            if ((spawners[i].transform.position.x > this.transform.position.x) && spawners[i].GetComponent<Spawn_Point_Script>().checkForGround())
            {
                this.transform.position = spawners[i].transform.position;
                break;
            }
        }
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);
        onWall = false;
        isInAir = true;
        isDead = false;

        if (!this.renderer.enabled)
        {
            this.renderer.enabled = true;
        }
    }
	
	//If we're below the ground, respawn us above it and (ahead) us a little.
	//	Ahead for the vertical slice since it's not possible to win if you don't
	//	have the triggers set properly.
	void CheckDead()
	{
        if (isDead && !isWaitingToSpawn) // Make a better condition
		{
            StartCoroutine("Respawn");
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

        // Think about maybe putting this into the raycast code, but for right now, this seems to be working

        if (!playerGravityScript.isGravityInverted)
        {
            if (other.contacts[0].normal.y > 0.25)
            {
                //print ("Can Jump Now "+Time.time);
                canJump = true;
                isJumping = false;
                onWall = false;
                wallJumpsLeft = wallJumpsAllowed;
                if (isInAir && (Time.time - timeWhenLastJumped > 0.25f))
                {
                    //print("Not in air anymore"+Time.time);
                    isInAir = false;
                }

                if (isJetPackActive)
                {
                    this.GetComponentInChildren<ParticleSystem>().Stop();
                }
            }
        }
        else
        {
            if (other.contacts[0].normal.y < 0.25)
            {
                //print ("Can Jump Now "+Time.time);
                canJump = true;
                isJumping = false;
                onWall = false;
                wallJumpsLeft = wallJumpsAllowed;
                if (isInAir && (Time.time - timeWhenLastJumped > 0.25f))
                {
                    //print("Not in air anymore"+Time.time);
                    isInAir = false;
                }

                if (isJetPackActive)
                {
                    this.GetComponentInChildren<ParticleSystem>().Stop();
                }
            }
        }

        if (other.contacts[0].normal.x < -0.8 && other.gameObject.CompareTag("Terrain") && !onWall) // Wall Jump Test
        {
            numberOfFramesHit = 0;
            onWall = true;
            this.rigidbody.AddForce(new Vector3(horizontalMovement.accelerationPushOffWall, 0, 0), ForceMode.VelocityChange);			
        }

        if (other.gameObject.CompareTag("L1_Elite_Laser") || other.gameObject.CompareTag("L1_Elite_Missile")) // Then respawn player & reset camera position
        {
            if (stats)
            {
                stats.numberOfDeaths++;
            }
            isDead = true;
            this.renderer.enabled = false;
            StartCoroutine("Respawn");            
        }
		
		//Next level stuff: Persist some stuff for the next level
		if (other.gameObject.CompareTag("Ship") && Application.loadedLevelName == "test_scene_0")
		{
			_Loader_L0.loader.EndLevel();
		}
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Terrain"))
        {
            if (numberOfFramesHit < maxWallFrameHits)
            {
                numberOfFramesHit++;
                //print(numberOfFramesHit);
            }
            else
            {
                numberOfFramesHit = 0;
                if (!isJumping)
                {
                    if (!playerGravityScript.isGravityInverted)
                    {
                        if (other.contacts[0].normal.y > 0.25)
                        {
                            //print ("Can Jump Now "+Time.time);
                            canJump = true;
                            isJumping = false;
                            onWall = false;
                            wallJumpsLeft = wallJumpsAllowed;
                            if (isInAir && (Time.time - timeWhenLastJumped > 0.25f))
                            {
                                //print("Not in air anymore"+Time.time);
                                isInAir = false;
                            }

                            if (isJetPackActive)
                            {
                                this.GetComponentInChildren<ParticleSystem>().Stop();
                            }
                        }
                    }
                    else
                    {
                        if (other.contacts[0].normal.y < 0.25)
                        {
                            //print ("Can Jump Now "+Time.time);
                            canJump = true;
                            isJumping = false;
                            onWall = false;
                            wallJumpsLeft = wallJumpsAllowed;
                            if (isInAir && (Time.time - timeWhenLastJumped > 0.25f))
                            {
                                //print("Not in air anymore"+Time.time);
                                isInAir = false;
                            }

                            if (isJetPackActive)
                            {
                                this.GetComponentInChildren<ParticleSystem>().Stop();
                            }
                        }
                    }
                }

                if (other.contacts[0].normal.x < -0.8 && other.gameObject.CompareTag("Terrain") && !onWall) // Wall Jump Test
                {
                    onWall = true;
                    this.rigidbody.AddForce(new Vector3(horizontalMovement.accelerationPushOffWall, 0, 0), ForceMode.VelocityChange);
                }
            }
        }
    }

    /*ADDERESS THIS LATER*/
    //void OnCollisionExit(Collision other)
    //{
    //    canJump = false; // Will be set to false when the player leaves a platform so a jump in midair cannot occur 
    //}
}

