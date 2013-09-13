using UnityEngine;
using System.Collections;

public class Player_Follow_Script : MonoBehaviour {

    private GameObject player;
    private GameObject ship;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        ship = GameObject.FindGameObjectWithTag("Ship");
	}
	
	// Update is called once per frame
	void Update () {
        if (player.GetComponent<Player_Movement_Script>().isActive)
        {
            Vector3 tempVector = transform.position;
            tempVector.x = player.transform.position.x;
            tempVector.y = player.transform.position.y;
            transform.position = tempVector;
        }
        else if (ship.GetComponent<Ship_Move_Script>().isActive)
        {
            Vector3 tempVector = transform.position;
            tempVector.x = ship.transform.position.x;
            tempVector.y = ship.transform.position.y;
            transform.position = tempVector;
        }
	}
}
