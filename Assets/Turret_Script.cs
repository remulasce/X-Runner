using UnityEngine;
using System.Collections;

public class Turret_Script : MonoBehaviour {

    public int randomValue = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.transform.LookAt(other.gameObject.transform);
            if (Random.Range(0, randomValue) == 0)
            {
                GameObject laser = (GameObject) Instantiate(Resources.Load("Prefabs/Level_2/L2_Enemy_Shot_Target"), this.transform.position, Quaternion.Euler(0, 0, 0));
                laser.GetComponent<L2_Enemy_Shot_Target_Script>().speed = 30;
            }
        }
    }
}
