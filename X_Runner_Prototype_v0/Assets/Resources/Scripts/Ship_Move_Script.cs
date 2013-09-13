using UnityEngine;
using System.Collections;

public class Ship_Move_Script : MonoBehaviour {

    public float movementSpeed = 1.0f;
    public float evasiveSpeed = 1.0f;

    public bool isActive = false;

    // Use this for initialization
    void Start()
    {

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
        tempVector.x += movementSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
        {
            tempVector.y += evasiveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            tempVector.y -= evasiveSpeed * Time.deltaTime;
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
