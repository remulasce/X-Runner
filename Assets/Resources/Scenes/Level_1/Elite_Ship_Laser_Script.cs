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

    public void FireHomingLaserAt(GameObject target, Vector3 offSet, float laserSpeed, GameObject postCollisionParticleSystem, Elite_Laser_Trigger_Script.homingTargetInformation hm)
    {
        GameObject laser = (GameObject)Instantiate(Resources.Load("Prefabs/Level_1/Elite_Laser_Homing"), cannons[currentCannon].transform.position, Quaternion.identity);
        laser.GetComponent<Elite_Laser_Homing_Script>().Initialize(target, offSet, laserSpeed, postCollisionParticleSystem, hm);
        currentCannon++;
        if (currentCannon >= cannons.Length)
        {
            currentCannon = 0;
        }
    }

    public void FireBigMissileAt(GameObject target, Vector3 offSet, float laserSpeed, GameObject postCollisionParticleSystem, Elite_Laser_Trigger_Script.homingTargetInformation hm)
    {
        GameObject laser = (GameObject)Instantiate(Resources.Load("Prefabs/Level_1/Elite_Missile_Mega"), cannons[currentCannon].transform.position, Quaternion.identity);
        laser.GetComponent<Elite_Laser_Homing_Script>().Initialize(target, offSet, laserSpeed, postCollisionParticleSystem, hm);
        currentCannon++;
        if (currentCannon >= cannons.Length)
        {
            currentCannon = 0;
        }
    }

    // ----- Special Detonator Functions
    public void FireLaserAtDetonator(GameObject target, Vector3 offSet, float laserSpeed, GameObject postCollisionParticleSystem)
    {
        GameObject laser = (GameObject)Instantiate(Resources.Load("Prefabs/Level_1/Elite_Laser"), cannons[currentCannon].transform.position, Quaternion.identity);
        laser.GetComponent<Elite_Laser_Script>().InitializeWithDetonator(target, offSet, laserSpeed, postCollisionParticleSystem);

        currentCannon++;
        if (currentCannon >= cannons.Length)
        {
            currentCannon = 0;
        }
    }

    public void FireHomingLaserAtDetonator(GameObject target, Vector3 offSet, float laserSpeed, GameObject postCollisionParticleSystem, Elite_Laser_Trigger_Script.homingTargetInformation hm)
    {
        GameObject laser = (GameObject)Instantiate(Resources.Load("Prefabs/Level_1/Elite_Laser_Homing"), cannons[currentCannon].transform.position, Quaternion.identity);
        laser.GetComponent<Elite_Laser_Homing_Script>().InitializeWithDetonator(target, offSet, laserSpeed, postCollisionParticleSystem, hm);
        currentCannon++;
        if (currentCannon >= cannons.Length)
        {
            currentCannon = 0;
        }
    }

    public void FireBigMissileAtDetonator(GameObject target, Vector3 offSet, float laserSpeed, GameObject postCollisionParticleSystem, Elite_Laser_Trigger_Script.homingTargetInformation hm)
    {
        GameObject laser = (GameObject)Instantiate(Resources.Load("Prefabs/Level_1/Elite_Missile_Mega"), cannons[currentCannon].transform.position, Quaternion.identity);
        laser.GetComponent<Elite_Laser_Homing_Script>().InitializeWithDetonator(target, offSet, laserSpeed, postCollisionParticleSystem, hm);
        currentCannon++;
        if (currentCannon >= cannons.Length)
        {
            currentCannon = 0;
        }
    }

    public void PlayLeave()
    {
        audio.Play();
    }
}
