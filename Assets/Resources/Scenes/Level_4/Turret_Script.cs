using UnityEngine;
using System.Collections;

public class Turret_Script : MonoBehaviour {

    public float shootTime = 0.5f;
    public float laserSpeed = 30.0f;

    public float homeInTime = 0.35f;
    private bool isLockedOn = false;

    public bool isHoming = false;

    private float startTime = 0.0f;

    public int totalHealth = 5;
    private int currentHealth = 0;    

    public GameObject explosionPrefab;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
        currentHealth = totalHealth;
	}
	
	// Update is called once per frame
	void Update () {
        if (currentHealth <= 0)
        {
            explosionPrefab = (GameObject)Instantiate(explosionPrefab);
            explosionPrefab.transform.position = this.transform.position;
            explosionPrefab.transform.parent = this.transform.parent;
            explosionPrefab.GetComponent<Detonator>().Explode();
            Object.Destroy(this.gameObject);
        }
	}

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Time.time - startTime < homeInTime && !isLockedOn)
            {
                return;
            }
            else
            {
                isLockedOn = true;                
            }
            this.transform.LookAt(other.gameObject.transform);
            if ((Time.time - startTime) > shootTime)
            {
                GameObject laser;
                if (!isHoming)
                {
                    laser = (GameObject)Instantiate(Resources.Load("Prefabs/Level_2/L2_Enemy_Shot_Target"), this.transform.position, Quaternion.Euler(0, 0, 0));
                    laser.GetComponent<L2_Enemy_Shot_Target_Script>().speed = laserSpeed;                    
                    startTime = Time.time;
                }
                else
                {
                    laser = (GameObject)Instantiate(Resources.Load("Prefabs/Level_2/L2_Enemy_Shot_Homing"), this.transform.position, Quaternion.Euler(0, 0, 0));
                    laser.GetComponent<L2_Enemy_Shot_Target_Script>().speed = laserSpeed;
                    startTime = Time.time;
                }
                laser.transform.parent = this.transform.parent;
            }            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            startTime = Time.time;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("L2_PlayerShot"))
        {
            this.currentHealth--;
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            this.currentHealth = 0;            
        }
    }
}
