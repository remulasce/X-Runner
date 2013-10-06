using UnityEngine;
using System.Collections;


//Movement and control for the L2 TopDownShooter
public class L2_Ship_Script : MonoBehaviour {
	
	//Maximumu speed in each direction
	float maxSpeed = 40;
	float acceleration = 100;
	
	//When we're far from maxvelocity, we add some extra force to get us going
	float startupBoost = 40;
	//At what speed we should stop applying the startup boost.
	float startupCutoff = 40;
	
	float frictionConstant = 8f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	//Add the arrow-key primary movement
	void addControl()
	{
		float dx = Input.GetAxis("Horizontal");
		float dy = Input.GetAxis("Vertical");
		
		this.rigidbody.AddForce(acceleration * new Vector3(dx, dy, 0));
	}
	
	//Adds extra boost when we're accelerating from low speeds, to add
	// responsiveness
	void addStartupBoost()
	{
		float vx = this.rigidbody.velocity.x;
		float vy = this.rigidbody.velocity.y;
		
		float signX = Mathf.Sign (vx);
		float signY = Mathf.Sign (vy);
		
		//Tricksie absolute value
		if (signX * vx < startupCutoff && Input.GetAxis("Horizontal") != 0) 
		{
			this.rigidbody.AddForce(startupBoost * signX,0,0);
		}
		if (signY * vy < startupCutoff && Input.GetAxis("Vertical") != 0)
		{
			this.rigidbody.AddForce(0,startupBoost * signY,0);
		}
	}
	
	//Apply a bit of zeroing friction
	void slowDown()
	{
		this.rigidbody.AddForce(-frictionConstant * this.rigidbody.velocity);
	}
	
	//Enforce max speed limit
	void limitSpeed()
	{
		if (Mathf.Abs(this.rigidbody.velocity.x) > maxSpeed)
		{
			this.rigidbody.velocity = new Vector3(maxSpeed*Mathf.Sign(this.rigidbody.velocity.x),0,0);
		}
		if (Mathf.Abs (this.rigidbody.velocity.y) > maxSpeed)
		{
			this.rigidbody.velocity = new Vector3(0,maxSpeed*Mathf.Sign(this.rigidbody.velocity.y),0);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		addControl();
		addStartupBoost();
		slowDown();
		limitSpeed();
	
		
		
	}
}
