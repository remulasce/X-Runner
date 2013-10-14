using UnityEngine;
using System.Collections;

public class Elite_Ship_Laser_Script : MonoBehaviour {

    // SpawnZones will be determined by a % distance away from the center -- that way lasers can fire faster
    private int currentCannon = 0;

    private GameObject[] cannons;

    // Use this for initialization
	void Start () {
        cannons = GameObject.FindGameObjectsWithTag("L1_Elite_Cannon");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void FireLaserAt(GameObject target, Vector3 offSet, float laserSpeed, GameObject postCollisionParticleSystem)
    {        
        
        
        GameObject laser = (GameObject) Instantiate(Resources.Load("Prefabs/Level_1/Elite_Laser"), cannons[currentCannon].transform.position, Quaternion.identity);
        laser.GetComponent<Elite_Laser_Script>().Initialize(target, offSet, laserSpeed, postCollisionParticleSystem);
        
        currentCannon++;
        if (currentCannon >= cannons.Length)
        {
            currentCannon = 0;
        }
    }
}
