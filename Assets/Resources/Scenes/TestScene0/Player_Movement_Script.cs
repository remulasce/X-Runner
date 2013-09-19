using UnityEngine;
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
		//We should persist for the next level transition
		DontDestroyOnLoad(this);
		
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
		
		//Next level stuff: Persist some stuff for the next level
		if (other.gameObject.CompareTag("Ship") && Application.loadedLevelName == "test_scene_0")
		{
			print ("Hit goal, moving to next level");
			
			Transform ship = (Transform)(GameObject.FindGameObjectWithTag("Ship").GetComponent<Transform>());
			DontDestroyOnLoad(ship);
			LoadHandler.level0ship = ship;
			
			
			Application.LoadLevel("test_scene_1");
		}
    }
}

