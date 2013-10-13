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

    [System.Serializable]
    public class targetInformation
    {
        public GameObject target;
        public Vector3 targetOffset;
        [Range(1.0f, 10000.0f)]
        public float laserSpeed;
        public GameObject postCollisionParticleSystem; // Sometimes, if the laser hits something, a particle system should go off.
        public float delay;
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
                StartCoroutine("DelayLaserShot", targets[i]);                
            }
        }
    }

    public IEnumerator DelayLaserShot(targetInformation t)
    {
        yield return new WaitForSeconds(t.delay);
        eliteShipLaserScript.FireLaserAt(t.target, t.targetOffset, t.laserSpeed, t.postCollisionParticleSystem);
    }
}
