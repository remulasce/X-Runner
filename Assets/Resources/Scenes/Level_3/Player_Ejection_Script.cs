using UnityEngine;
using System.Collections;

public class Player_Ejection_Script : MonoBehaviour {

    public Vector3 forceToAddToPlayer = new Vector3(100, 100, 0);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Camera.main.gameObject.GetComponent<CanabaltCamera>().player = player;
            player.GetComponent<Player_Movement_Script>().isActive = true;
            player.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, 0);
            player.rigidbody.AddForce(forceToAddToPlayer);
        }
    }
}
