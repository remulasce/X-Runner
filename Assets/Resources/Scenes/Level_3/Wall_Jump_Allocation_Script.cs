using UnityEngine;
using System.Collections;

public class Wall_Jump_Allocation_Script : MonoBehaviour {

    public int numberOfWallJumpsAllowed = 0;
    public bool unlimitedWallJumps = false;

    private bool isAllocated = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && (!isAllocated || unlimitedWallJumps))
        {
            other.gameObject.GetComponent<Player_Movement_Script>().wallJumpsAllowed = numberOfWallJumpsAllowed;
            other.gameObject.GetComponent<Player_Movement_Script>().wallJumpsLeft = numberOfWallJumpsAllowed;
            isAllocated = true;
        }
    }
}
