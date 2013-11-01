using UnityEngine;
using System.Collections;

public class Elite_Laser_Homing_Script : MonoBehaviour {

    public GameObject target;
    [Range(1.0f, 10000.0f)]
    public float laserSpeed = 50.0f;
    public Vector3 targetOffset = Vector3.zero;
    public Vector3 nonTargetDirection = new Vector3(0, 0, 1);
    public GameObject postCollisionParticleSystem; // Sometimes, if the laser hits something, a particle system should go off.
    public GameObject detonatorPrefab; // For the Detonator Explosions

    // Laser Values specific to homing onto a target
    public Vector3 initialHomingOffset = Vector3.zero;
    [Range(0.0f, 0.9999999f)]
    public float percentToCloseOffset = 0.0f; // How fast the distance between the initial homing offset will be covered from 0 to 0.9999999
    public float closingMagnitude = 1.0f; // If the mangitude of the distance between the target reaches this distance, the missile will no longer home onto the target

    private bool isHoming = true; // When this is set to false, the homing ability of the missile turns off.

    // Use this for initialization
    void Start()
    {
        if (target)
        {
            this.transform.LookAt(target.transform.position + targetOffset + initialHomingOffset, new Vector3(0,1,0)); // Set the direction here
        }
        else
        {
            this.transform.LookAt(nonTargetDirection + initialHomingOffset, new Vector3(0,1,0)); // Set the direction here
        }
        //Debug.Log(this.transform.forward);
    }

    // Use this for initialization from another object
    public void Initialize(GameObject target, Vector3 offSet, float laserSpeed, GameObject postCollisionParticleSystem, Elite_Laser_Trigger_Script.homingTargetInformation hm)
    {
        this.target = target;
        this.targetOffset = offSet;
        this.laserSpeed = laserSpeed;
        this.postCollisionParticleSystem = postCollisionParticleSystem;

        // Special Homing Laser Values
        this.initialHomingOffset = hm.initialHomingOffset;
        this.closingMagnitude = hm.closingMagnitude;
        this.percentToCloseOffset = hm.percentToCloseOffset;

        if (target)
        {
            this.transform.LookAt(target.transform.position + targetOffset + initialHomingOffset, new Vector3(0,1,0)); // Set the direction here
        }
        else
        {
            this.transform.LookAt(nonTargetDirection + initialHomingOffset, new Vector3(0,1,0)); // Set the direction here
        }
        //Debug.Log(this.transform.forward);
    }

    public void InitializeWithDetonator(GameObject target, Vector3 offSet, float laserSpeed, GameObject postCollisionParticleSystem, Elite_Laser_Trigger_Script.homingTargetInformation hm)
    {
        this.target = target;
        this.targetOffset = offSet;
        this.laserSpeed = laserSpeed;
        this.detonatorPrefab = (GameObject)Instantiate(postCollisionParticleSystem);

        // Special Homing Laser Values
        this.initialHomingOffset = hm.initialHomingOffset;
        this.closingMagnitude = hm.closingMagnitude;
        this.percentToCloseOffset = hm.percentToCloseOffset;

        if (target)
        {
            this.transform.LookAt(target.transform.position + targetOffset + initialHomingOffset, new Vector3(0, 1, 0)); // Set the direction here
        }
        else
        {
            this.transform.LookAt(nonTargetDirection + initialHomingOffset, new Vector3(0, 1, 0)); // Set the direction here
        }
        //Debug.Log(this.transform.forward);
    }

    // Update is called once per frame
    void Update()
    {
        // Homing Follow Code
        
        // Close the gap for the initial offset
        initialHomingOffset *= percentToCloseOffset;

        if (closingMagnitude < Vector3.Distance(target.transform.position, this.transform.position) && isHoming) // Point the missile towards the target
        {
            // Create a new lookat if the target has moved
            if (target)
            {
                this.transform.LookAt(target.transform.position + targetOffset + initialHomingOffset, new Vector3(0, 1, 0)); // Set the direction here
            }
            else
            {
                this.transform.LookAt(nonTargetDirection + initialHomingOffset, new Vector3(0, 1, 0)); // Set the direction here
            }
        }
        else
        {
            isHoming = false;
        }

        // Now calculate the movement        
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

        Debug.Log("Homing Laser Hit: " + other.gameObject.name);

        Object.Destroy(this.gameObject);
    }
}
