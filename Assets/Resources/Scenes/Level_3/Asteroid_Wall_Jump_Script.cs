using UnityEngine;
using System.Collections;

public class Asteroid_Wall_Jump_Script : MonoBehaviour {

    public float forceMagnitude = 100.0f;

    public float maxVecloityMagnitude = 5.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (this.rigidbody.velocity.magnitude > maxVecloityMagnitude)
        {
            this.rigidbody.velocity = this.rigidbody.velocity.normalized * maxVecloityMagnitude;
        }
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.rigidbody.AddForce(other.contacts[0].normal * forceMagnitude);
        }
    }
}
