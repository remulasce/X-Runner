using UnityEngine;
using System.Collections;

public class Elite_Laser_Script : MonoBehaviour {

    public GameObject target;
    [Range(1.0f, 10000.0f)]
    public float laserSpeed = 50.0f;
    public Vector3 targetOffset = Vector3.zero;
    public Vector3 nonTargetDirection = new Vector3(0, 0, 1);

    public GameObject postCollisionParticleSystem; // Sometimes, if the laser hits something, a particle system should go off.

    public GameObject detonatorPrefab; // For the Detonator Explosions

	// Use this for initialization
	void Start () {
        if (target)
        {
            this.transform.LookAt(target.transform.position + targetOffset, new Vector3(0,1,0)); // Set the direction here
        }
        else
        {
            this.transform.LookAt(nonTargetDirection, new Vector3(0,1,0)); // Set the direction here
        }
        //Debug.Log(this.transform.forward);
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
            this.transform.LookAt(target.transform.position + targetOffset, new Vector3(0,1,0)); // Set the direction here
        }
        else
        {
            this.transform.LookAt(nonTargetDirection, new Vector3(0,1,0)); // Set the direction here
        }
        //Debug.Log(this.transform.forward);
    }

    // Use this for initialization from another object
    public void InitializeWithDetonator(GameObject target, Vector3 offSet, float laserSpeed, GameObject postCollisionParticleSystem)
    {
        this.target = target;
        this.targetOffset = offSet;
        this.laserSpeed = laserSpeed;
        if (postCollisionParticleSystem)
        {
            this.detonatorPrefab = (GameObject)Instantiate(postCollisionParticleSystem);
        }
        else
        {
            this.detonatorPrefab = (GameObject)Instantiate(this.detonatorPrefab);
        }

        if (target)
        {
            this.transform.LookAt(target.transform.position + targetOffset, new Vector3(0, 1, 0)); // Set the direction here
        }
        else
        {
            this.transform.LookAt(nonTargetDirection, new Vector3(0, 1, 0)); // Set the direction here
        }
        //Debug.Log(this.transform.forward);
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

        if (detonatorPrefab)
        {
            detonatorPrefab.transform.position = other.contacts[0].point;
            detonatorPrefab.GetComponent<Detonator>().Explode();
        }
        else if (postCollisionParticleSystem)
        {
            postCollisionParticleSystem.transform.position = other.contacts[0].point; // Set the particle system to the collision point
            postCollisionParticleSystem.particleSystem.Play();
            postCollisionParticleSystem.transform.parent = other.gameObject.transform;
        }

        //Debug.Log("Laser Hit: " + other.gameObject.name);

        Object.Destroy(this.gameObject);
    }
}
