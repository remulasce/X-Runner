using UnityEngine;
using System.Collections;

public class L2_Player_Shot_Script : MonoBehaviour {
	
	public float speed;
	
	// Use this for initialization
	void Start () {
		//Negative because it somehow works better.
		this.rigidbody.velocity = this.transform.rotation * new Vector3(0, speed, 0); 
	}
	
	//Don't keep drifting forever
	void killIfOutBounds()
	{
		if (!GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), this.collider.bounds))
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
		if (col.gameObject.CompareTag("L2_Enemy"))
		{
			Destroy(this.gameObject);
		}

        if (col.gameObject.CompareTag("Trench_Wall") || col.gameObject.CompareTag("Trench_Turret"))
        {
            GameObject g = (GameObject) Instantiate(Resources.Load("Prefabs/Level_2/Explosions/L2_Asteroid_Impact_Explosion"), col.contacts[0].point, Quaternion.Euler(0, 0, 0));
            g.transform.parent = col.gameObject.transform.parent;
            Object.Destroy(this.gameObject);
        }        
	}
}
