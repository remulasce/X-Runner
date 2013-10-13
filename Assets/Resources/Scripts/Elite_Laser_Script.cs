using UnityEngine;
using System.Collections;

public class Elite_Laser_Script : MonoBehaviour {

    public GameObject target;
    [Range(1.0f, 10000.0f)]
    public float laserSpeed = 50.0f;
    public Vector3 targetOffset = Vector3.zero;
    public Vector3 nonTargetDirection = new Vector3(0, 0, 1);
    public GameObject postCollisionParticleSystem; // Sometimes, if the laser hits something, a particle system should go off.

	// Use this for initialization
	void Start () {
        if (target)
        {
            this.transform.LookAt(target.transform.position + targetOffset, this.transform.up); // Set the direction here
        }
        else
        {
            this.transform.LookAt(nonTargetDirection, this.transform.up); // Set the direction here
        }
        Debug.Log(this.transform.forward);
	}

    // Use this for initialization from another object
    public void Initialize(GameObject target, Vector3 offSet, float laserSpeed, GameObject postCollisionParticleSystem)
    {
        this.target = target;
        this.targetOffset = offSet;
        this.laserSpeed = laserSpeed;
        this.postCollisionParticleSystem = postCollisionParticleSystem;
        
        if (target)
        {
            this.transform.LookAt(target.transform.position + targetOffset, this.transform.up); // Set the direction here
        }
        else
        {
            this.transform.LookAt(nonTargetDirection, this.transform.up); // Set the direction here
        }
        Debug.Log(this.transform.forward);
    }

	// Update is called once per frame
	void Update () {
        if (target)
        {
            this.transform.position += this.transform.forward * Time.deltaTime * laserSpeed;
        }
        else
        {
            this.transform.position += this.nonTargetDirection * Time.deltaTime * laserSpeed;
        }
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("L1_Elite_Laser")) // Do not want enemy lasers to hit and destroy each other
        {
            return;
        }
        
        if (postCollisionParticleSystem)
        {
            postCollisionParticleSystem.transform.position = other.contacts[0].point; // Set the particle system to the collision point
            postCollisionParticleSystem.particleSystem.Play();
            postCollisionParticleSystem.transform.parent = other.gameObject.transform;
        }        

        Object.Destroy(this.gameObject);
    }
}
