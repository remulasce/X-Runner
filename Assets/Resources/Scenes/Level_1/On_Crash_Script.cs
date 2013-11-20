using UnityEngine;
using System.Collections;

public class On_Crash_Script : MonoBehaviour {

    public GameObject explosionPrefab;
    public GameObject missileImpactPrefab;

	// Use this for initialization
	void Start () {
        if (explosionPrefab)
        {
            explosionPrefab = (GameObject)Instantiate(explosionPrefab);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FireMissile()
    {
        GameObject laser = (GameObject)Instantiate(Resources.Load("Prefabs/Level_1/Frendly_Laser_Homing"), this.transform.position + new Vector3(0, 0, 0), Quaternion.identity);
        Elite_Laser_Trigger_Script.homingTargetInformation hm = new Elite_Laser_Trigger_Script.homingTargetInformation();
        hm.closingMagnitude = 0;
        hm.initialHomingOffset = new Vector3(550, 0, 0);
        hm.percentToCloseOffset = 0.975f;
        laser.GetComponent<Elite_Laser_Homing_Script>().InitializeWithDetonator(GameObject.FindGameObjectWithTag("L1_Elite"), Vector3.zero, 192.5f, missileImpactPrefab, hm, true);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Terrain"))
        {
            if (explosionPrefab.GetComponent<Detonator>())
            {
                explosionPrefab.transform.position = this.transform.position;
                explosionPrefab.GetComponent<Detonator>().Explode();
                Destroy(this.gameObject);
            }
        }
    }

    void Explode()
    {
        if (explosionPrefab.GetComponent<Detonator>())
        {
            explosionPrefab.transform.position = this.transform.position;
            explosionPrefab.GetComponent<Detonator>().Explode();
            Destroy(this.gameObject);
        }
    }
}
