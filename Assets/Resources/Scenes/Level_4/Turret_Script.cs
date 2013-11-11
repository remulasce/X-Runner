using UnityEngine;
using System.Collections;

public class Turret_Script : MonoBehaviour {

    public float shootTime = 0.5f;

    private float startTime = 0.0f;

	// Use this for initialization
	void Start () {
        startTime = shootTime;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.transform.LookAt(other.gameObject.transform);
            if ((Time.time - startTime) > shootTime)
            {
                GameObject laser = (GameObject) Instantiate(Resources.Load("Prefabs/Level_2/L2_Enemy_Shot_Target"), this.transform.position, Quaternion.Euler(0, 0, 0));
                laser.GetComponent<L2_Enemy_Shot_Target_Script>().speed = 30;
                startTime = Time.time;
            }
        }
    }
}
