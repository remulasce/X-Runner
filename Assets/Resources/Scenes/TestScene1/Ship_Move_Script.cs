using UnityEngine;
using System.Collections;

public class Ship_Move_Script : MonoBehaviour {

    public float movementSpeed = 1.0f;
    public float evasiveSpeed = 1.0f;

    public bool isActive = false;

    // Use this for initialization
    void Start()
    {
		DontDestroyOnLoad (this);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.time - startTime);
        if (!isActive)
        {
            return;
        }

        Vector3 tempVector = transform.position;
        tempVector.y += movementSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            tempVector.x -= evasiveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            tempVector.x += evasiveSpeed * Time.deltaTime;
        }
        transform.position = tempVector;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Player")
        {
            other.gameObject.GetComponent<Player_Movement_Script>().isActive = false;
            this.isActive = true;
        }
    }
}
