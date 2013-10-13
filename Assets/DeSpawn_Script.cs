using UnityEngine;
using System.Collections;

public class DeSpawn_Script : MonoBehaviour {

    // Will be sued to destroy objects, that's it

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Object Destroyed:" + other.gameObject.name);
        
        Object.Destroy(other.gameObject);
    }
}
