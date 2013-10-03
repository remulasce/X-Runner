using UnityEngine;
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

    private float timeWhenLastJumped = 0.0f; // Will check when the player last jumped
    private bool hitWallSideways = false;

	// Use this for initialization
	void Start () {
		//We should persist for the next level transition
		DontDestroyOnLoad(this);		
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
		}
	}
	
	// Update is called once per frame
	void Update () {
        if (!isActive) {return;}
        
		
		DoXVelocity();
		DoJump();

        //print(rigidbody.velocity);
    }

    void OnCollisionEnter(Collision other) 
    {
        if (other.contacts[0].normal.x < -0.95 && other.gameObject.CompareTag("Terrain")) // Then stop -- you hit the wall while jumping
        {
            hitWallSideways = true;
        }
        //Tags are precompiled into an enum, so they don't have to do string operations.
		//Check to make sure the thing we hit was below us		
        else if (other.contacts[0].normal.y > 0)
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
		
		//Next level stuff: Persist some stuff for the next level
		if (other.gameObject.CompareTag("Ship") && Application.loadedLevelName == "test_scene_0")
		{
			_Loader_L0.loader.EndLevel();
		}
    }
}

