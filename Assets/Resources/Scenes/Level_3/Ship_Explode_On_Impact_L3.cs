using UnityEngine;
using System.Collections;

public class Ship_Explode_On_Impact_L3 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        this.GetComponent<Detonator>().Explode();
        this.gameObject.renderer.enabled = false;
    }
}
