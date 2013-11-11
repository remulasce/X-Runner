using UnityEngine;
using System.Collections;

public class L2_Enemy_Shot_Script : MonoBehaviour
{
	
	public float speed;

    public bool isNotCinematic = true; // If NOT checked, it will not be destroyed when it goes OB

    public bool shootsLeft = false;

	// Use this for initialization
	protected void Start () {      
        
        this.transform.forward = new Vector3(0, -1, 0);
        
        this.rigidbody.velocity = transform.forward * speed;
	}
	
	//Don't keep drifting forever
	void killIfOutBounds()
	{
        if (!GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), this.collider.bounds) && isNotCinematic)
        {
            Destroy(this.gameObject);
        }
	}
	
	
	// Update is called once per frame
	protected void Update () {
		//The RigidBody velocity handles all our movement nicely
		killIfOutBounds();
	}
	
	//If we hit an enemy, kill ourself.
	//Leave the enemy to check if it should die.
    protected void OnCollisionEnter(Collision col)
	{
		
		/*
		if (col.gameObject.CompareTag("Player"))
		{
			Destroy(this.gameObject);
		}
		*/
		
		//Basically, there should be nothing that this actuall collides with
		// that does not destroy it.
		//Anything it shouldn't collide with should have been eliminated in the
		// editor layers.
        if (col.gameObject.CompareTag("Trench_Wall"))
        {
            Instantiate(Resources.Load("Prefabs/Level_2/Explosions/L2_Asteroid_Impact_Explosion"), col.contacts[0].point, Quaternion.Euler(0, 0, 0));
        }

		Object.Destroy (this.gameObject);
	}
}
