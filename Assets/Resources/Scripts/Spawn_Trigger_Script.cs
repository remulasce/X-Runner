using UnityEngine;
using System.Collections;

public class Spawn_Trigger_Script : MonoBehaviour {

    public Stat_Counter_Script stats = null;

	// Use this for initialization
	void Start () {
        stats = GameObject.FindGameObjectWithTag("Stats").GetComponent<Stat_Counter_Script>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player_Movement_Script>().isInvincible = false;
            other.gameObject.GetComponent<Player_Movement_Script>().isDead = true;
            if (stats)
            {
                stats.numberOfDeaths++;
            }
        }
    }
}
