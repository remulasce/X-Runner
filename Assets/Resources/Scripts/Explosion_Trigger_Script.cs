using UnityEngine;
using System.Collections;

public class Explosion_Trigger_Script : MonoBehaviour {

    public GameObject[] explodingPlatforms;

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
            return;
        }
        
        Debug.Log("Explosion Trigger Hit!");
        foreach (GameObject explodingPlatform in explodingPlatforms)
        {
            if (explodingPlatform != null)
            {                
                explodingPlatform.GetComponent<Destructive_Platform_Script>().ApplyStagedForce();
            }
        }        
    }
}
