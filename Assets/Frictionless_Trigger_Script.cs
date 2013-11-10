using UnityEngine;
using System.Collections;

public class Frictionless_Trigger_Script : MonoBehaviour {

    public bool turnOnFrictionLessJump = false;

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
            other.gameObject.GetComponent<Player_Movement_Script>().isFrictionLess = turnOnFrictionLessJump;
        }
    }
}
