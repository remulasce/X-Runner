using UnityEngine;
using System.Collections;

//Movement and control for the L2 TopDownShooter
public class L2_Ship_Script : MonoBehaviour
{

    [System.Serializable]
    public class MovementData
    {
        //Maximumu speed in each direction
        public float maxSpeed = 40;
        public float acceleration = 100;

        //When we're far from maxvelocity, we add some extra force to get us going
        public float startupBoost = 40;
        //At what speed we should stop applying the startup boost.
        public float startupCutoff = 40;
        public float frictionConstant = 8f;
    }

    public MovementData movement = new MovementData();
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	//Add the arrow-key primary movement
	void addControl ()
	{
		float dx = Input.GetAxis ("Horizontal");
		float dy = Input.GetAxis ("Vertical");

        this.rigidbody.AddForce(movement.acceleration * new Vector3(dx, dy, 0) * Time.deltaTime);
	}
	
	//Adds extra boost when we're accelerating from low speeds, to add
	// responsiveness
	void addStartupBoost ()
	{
		float dx = Input.GetAxis ("Horizontal");
		float dy = Input.GetAxis ("Vertical");
		
		float vx = this.rigidbody.velocity.x;
		float vy = this.rigidbody.velocity.y;
		
		float signX = Mathf.Sign (vx);
		float signY = Mathf.Sign (vy);
		
		//Tricksie absolute value
        if (signX * vx < movement.startupCutoff && dx != 0)
        {
            this.rigidbody.AddForce(movement.startupBoost * dx * Time.deltaTime, 0, 0);
		}
        if (signY * vy < movement.startupCutoff && dy != 0)
        {
            this.rigidbody.AddForce(0, movement.startupBoost * dy * Time.deltaTime, 0);
		}
	}
	
	//Apply a bit of zeroing friction
	void slowDown ()
	{
        this.rigidbody.AddForce(-movement.frictionConstant * this.rigidbody.velocity * Time.deltaTime);
	}
	
	//Enforce max speed limit
	void limitSpeed ()
	{
        if (Mathf.Abs(this.rigidbody.velocity.x) > movement.maxSpeed)
        {
            this.rigidbody.velocity = new Vector3(movement.maxSpeed * Mathf.Sign(this.rigidbody.velocity.x), 0, 0);
		}
        if (Mathf.Abs(this.rigidbody.velocity.y) > movement.maxSpeed)
        {
            this.rigidbody.velocity = new Vector3(0, movement.maxSpeed * Mathf.Sign(this.rigidbody.velocity.y), 0);
		}
	}
	
	//Make a shot if we're shooting
	void doShooting ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			Instantiate(Resources.Load("Prefabs/Level_2/L2_Player_Shot"), this.transform.position, Quaternion.Euler(0, 0, 180));
			
		}
	}
	// Update is called once per frame
	void Update ()
	{
		doShooting ();
		addControl ();
		addStartupBoost ();
		slowDown ();
		limitSpeed ();
	
		
		
	}
}
