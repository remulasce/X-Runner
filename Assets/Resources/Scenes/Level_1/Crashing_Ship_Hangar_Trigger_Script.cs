using UnityEngine;
using System.Collections;

public class Crashing_Ship_Hangar_Trigger_Script : MonoBehaviour {

    public GameObject[] crashingShips;

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
            foreach (GameObject ship in crashingShips)
            {
                ship.animation.Play();
            }
        }
    }
}
