using UnityEngine;
using System.Collections;

public class Player_Movement_Script : MonoBehaviour {

    public float movementSpeed = 1.0f;
    public float forceValuePostJump = 1.0f;
    public float forceValuePreJump = 1.0f;

    public bool isJumping = true;
    public bool isAllowedToJump = true;

    public bool isActive = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!isActive)
        {
            return;
        }
        
        Vector3 tempVector = transform.position;
        tempVector.x += movementSpeed * Time.deltaTime;
        transform.position = tempVector;

        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space)) && isAllowedToJump)
        {
            this.rigidbody.AddForce(new Vector3(0, 1, 0) * forceValuePreJump);
            isJumping = true;
        }

        else if ((Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.Space)))
        {
            isAllowedToJump = false;
            isJumping = false;
        }

        //Debug.Log(Time.time - startTime);

        if ((Input.GetButton("Jump") || Input.GetKey(KeyCode.Space)) && isJumping && isAllowedToJump)
        {
            this.rigidbody.AddForce(new Vector3(0, 1, 0) * forceValuePostJump);

        }
    }

    void OnCollisionStay(Collision other) 
    {

        if (other.gameObject.name == "Platform" && this.rigidbody.velocity.y < 0)
        {
            Debug.Log(other.gameObject.name);
            isAllowedToJump = true;
            isJumping = true;
        }
    }
}
