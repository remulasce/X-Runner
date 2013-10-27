using UnityEngine;
using System.Collections;

public class Global_Gravity_Trigger_Script : MonoBehaviour {

    public float gravityMagnitude = 0.0f;

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
            UnityEngine.Physics.gravity = new Vector3(0, gravityMagnitude, 0);
        }
    }
}
