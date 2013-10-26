using UnityEngine;
using System.Collections;

public class Player_Gravity_Trigger_Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Player_Gravity_Script>().isGravityInverted = !other.GetComponent<Player_Gravity_Script>().isGravityInverted;
            other.GetComponent<Player_Movement_Script>().isInAir = true;
            other.GetComponent<Player_Movement_Script>().canJump = false;
            other.GetComponent<Player_Movement_Script>().isJumping = false;
            other.GetComponentInChildren<ParticleSystem>().Stop();
        }
    }
}
