using UnityEngine;
using System.Collections;

public class Explosion_Trigger_Script : MonoBehaviour {

    public GameObject explodingPlatform;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnTriggerEnter(Collider other)
    {
        if (explodingPlatform != null)
        {
            Debug.Log("Explosion Trigger Hit!");
            explodingPlatform.rigidbody.constraints = RigidbodyConstraints.None;
            explodingPlatform.GetComponent<Destructive_Platform_Script>().ApplyStagedForce();
        }
    }
}
