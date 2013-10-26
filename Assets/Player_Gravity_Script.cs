using UnityEngine;
using System.Collections;

public class Player_Gravity_Script : MonoBehaviour {

    public bool isGravityInverted = false;    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isGravityInverted)
        {
            this.rigidbody.useGravity = false;
            this.gameObject.rigidbody.AddForce(-UnityEngine.Physics.gravity, ForceMode.Acceleration);
        }
        else
        {
            this.rigidbody.useGravity = true;
        }
	}
}
