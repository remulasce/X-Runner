using UnityEngine;
using System.Collections;

public class Sliding_Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player_Movement_Script>().horizontalMovement.isSliding = true;
        }
    }
}
