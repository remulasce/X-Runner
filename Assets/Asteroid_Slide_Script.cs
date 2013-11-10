using UnityEngine;
using System.Collections;

public class Asteroid_Slide_Script : MonoBehaviour {

    public Vector3 specialForceToApply = Vector3.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player_Movement_Script>().isSliding = true;            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player_Movement_Script>().isSliding = false;            
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.rigidbody.AddForce(specialForceToApply);
        }
    }
}
