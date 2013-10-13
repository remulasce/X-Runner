using UnityEngine;
using System.Collections;

public class Elite_Ship_Laser_Script : MonoBehaviour {

    // Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void FireLaserAt(GameObject target, Vector3 offSet, float laserSpeed, GameObject postCollisionParticleSystem)
    {
        GameObject laser = (GameObject) Instantiate(Resources.Load("Prefabs/Level_1/Elite_Laser"), this.gameObject.transform.position, Quaternion.identity);
        laser.GetComponent<Elite_Laser_Script>().Initialize(target, offSet, laserSpeed, postCollisionParticleSystem);
    }
}
