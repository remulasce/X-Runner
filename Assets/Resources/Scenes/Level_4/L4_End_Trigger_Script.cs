using UnityEngine;
using System.Collections;

public class L4_End_Trigger_Script : MonoBehaviour {

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
            Object.Destroy(GameObject.FindGameObjectWithTag("Stats"));
            Application.LoadLevel("Title_Screen");
        }
    }
}
