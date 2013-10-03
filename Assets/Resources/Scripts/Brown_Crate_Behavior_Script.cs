using UnityEngine;
using System.Collections;

public class Brown_Crate_Behavior_Script : MonoBehaviour
{

    public float forceMultiplier = 200.0f;

	// Use this for initialization
	void Start () {
        this.rigidbody.Sleep();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && other.contacts[0].normal.y >= 0)
        {
            print("In Y");
            this.rigidbody.AddForce(new Vector3(0, forceMultiplier * 10, 0));
        }
        if (other.gameObject.CompareTag("Player") && other.contacts[0].normal.x > 0.95)
        {
            print("In X");
            this.rigidbody.AddForce(new Vector3(other.gameObject.GetComponent<Player_Movement_Script>().movementSpeed * other.rigidbody.mass * forceMultiplier, 0, 0));            
        }
    }
}
