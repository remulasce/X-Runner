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
        //Debug.Log("Object Destroyed:" + other.gameObject.name);

        // HACK ALERT -- particle system I do not want destroyed are getting destroyed when they are parented to these objects say, after a laser shot.  Will transform trick to not destroy entire object
        // Object.Destroy(other.gameObject);

        other.gameObject.rigidbody.detectCollisions = false;
    }
}
