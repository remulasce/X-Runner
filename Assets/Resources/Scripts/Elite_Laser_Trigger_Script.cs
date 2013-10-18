using UnityEngine;
using System.Collections;

public class Elite_Laser_Trigger_Script : MonoBehaviour {

    /*Values for the elite laser are cloned here*/
    /*
    public GameObject target;
    public Vector3 targetOffset;
    [Range(1.0f, 10000.0f)]
    public float laserSpeed = 50.0f;
    public GameObject postCollisionParticleSystem; // Sometimes, if the laser hits something, a particle system should go off.
     * */

    public enum laserType { REGULAR, HOMING };

    [System.Serializable]
    public class homingTargetInformation
    {
        public Vector3 initialHomingOffset = Vector3.zero;
        [Range(0.0f, 0.9999999f)]
        public float percentToCloseOffset = 0.0f; // How fast the distance between the initial homing offset will be covered from 0 to 0.9999999
        public float closingMagnitude = 1.0f; // If the mangitude of the distance between the target reaches this distance, the missile will no longer home onto the target
    }

    [System.Serializable]
    public class targetInformation
    {
        public GameObject target;
        public Vector3 targetOffset;
        [Range(1.0f, 10000.0f)]
        public float laserSpeed;
        public GameObject postCollisionParticleSystem; // Sometimes, if the laser hits something, a particle system should go off.
        public float delay;
        public laserType laserType;

        // Special Values for if the laserType is homing
        public homingTargetInformation homingMissileProperties;    
    };

    public targetInformation[] targets; // Will be settable from the inspector

    private Elite_Ship_Laser_Script eliteShipLaserScript;
    // Use this for initialization
	void Start () {
        eliteShipLaserScript = GameObject.FindGameObjectWithTag("L1_Elite").GetComponent<Elite_Ship_Laser_Script>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].laserType == laserType.REGULAR)
                {
                    StartCoroutine("DelayLaserShot", targets[i]);
                }
                else if (targets[i].laserType == laserType.HOMING)
                {
                    StartCoroutine("DelayHomingLaserShot", targets[i]);
                }
            }
        }
    }

    public IEnumerator DelayLaserShot(targetInformation t)
    {
        yield return new WaitForSeconds(t.delay);
        eliteShipLaserScript.FireLaserAt(t.target, t.targetOffset, t.laserSpeed, t.postCollisionParticleSystem);
    }

    public IEnumerator DelayHomingLaserShot(targetInformation t)
    {
        yield return new WaitForSeconds(t.delay);
        eliteShipLaserScript.FireHomingLaserAt(t.target, t.targetOffset, t.laserSpeed, t.postCollisionParticleSystem, t.homingMissileProperties);
    }
}
