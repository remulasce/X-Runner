using UnityEngine;
using System.Collections;

public class Jetpack_powerUp_Script : MonoBehaviour {

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
            other.gameObject.GetComponent<Player_Movement_Script>().isJetPackActive = true;
            Object.Destroy(this.gameObject);
        }
    }
}
