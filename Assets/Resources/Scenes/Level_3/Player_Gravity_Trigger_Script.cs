using UnityEngine;
using System.Collections;

public class Player_Gravity_Trigger_Script : MonoBehaviour {

    [Range(0.001f, 5.0f)]
    public float cameraFlipSpeed = 0.001f;

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
            // Set all of the player values
            other.GetComponent<Player_Gravity_Script>().isGravityInverted = !other.GetComponent<Player_Gravity_Script>().isGravityInverted;
            other.GetComponent<Player_Movement_Script>().isInAir = true;
            other.GetComponent<Player_Movement_Script>().canJump = false;
            other.GetComponent<Player_Movement_Script>().isJumping = false;
            other.GetComponentInChildren<ParticleSystem>().Stop();

            // Make sure to start making the camera lerp towards the proper position.
            Camera.main.GetComponent<CanabaltCamera>().flipSpeed = this.cameraFlipSpeed;
            Camera.main.GetComponent<CanabaltCamera>().BeginFlipLerp();
        }
    }
}
