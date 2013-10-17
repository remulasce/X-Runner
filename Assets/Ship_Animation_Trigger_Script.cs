using UnityEngine;
using System.Collections;

public class Ship_Animation_Trigger_Script : MonoBehaviour {

    public GameObject eliteShip;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (eliteShip && other.gameObject.CompareTag("Player"))
        {
            eliteShip.animation.Play();
        }
    }
}
