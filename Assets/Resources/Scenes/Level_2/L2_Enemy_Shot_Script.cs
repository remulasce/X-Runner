using UnityEngine;
using System.Collections;

public class L2_Enemy_Shot_Script : MonoBehaviour
{
	
	public float speed;
	
	// Use this for initialization
	void Start () {
		this.rigidbody.velocity = new Vector3(0, speed, 0);
	}
	
	//Don't keep drifting forever
	void killIfOutBounds()
	{
		if (Mathf.Abs(this.transform.position.x)+Mathf.Abs(this.transform.position.y) > 1000)
		{
			Destroy (this.gameObject);
		}
	}
	
	
	// Update is called once per frame
	void Update () {
		//The RigidBody velocity handles all our movement nicely
		killIfOutBounds();
	}
	
	//If we hit an enemy, kill ourself.
	//Leave the enemy to check if it should die.
    void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.CompareTag("Player"))
		{
			Destroy(this.gameObject);
		}
	}
}
