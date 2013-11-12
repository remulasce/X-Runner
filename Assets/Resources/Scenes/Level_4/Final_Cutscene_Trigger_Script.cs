using UnityEngine;
using System.Collections;

public class Final_Cutscene_Trigger_Script : MonoBehaviour {

    private bool isTriggered = false;
    public GameObject explosionPrefab = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isTriggered)
        {
            GameObject missile = (GameObject)Instantiate(Resources.Load("Prefabs/Level_4/L4_Elite_Laser_Homing"));
            Elite_Laser_Trigger_Script.homingTargetInformation hm = new Elite_Laser_Trigger_Script.homingTargetInformation();
            hm.closingMagnitude = 0;
            hm.initialHomingOffset = new Vector3(-100, -20, 0);
            hm.percentToCloseOffset = 0.97f;
            missile.transform.position = other.transform.position + new Vector3(0, 0, 0.01f);
            missile.GetComponent<Elite_Laser_Homing_Script>().InitializeWithDetonator(GameObject.FindGameObjectWithTag("ReactorWeakPoint"), Vector3.zero, 90, explosionPrefab, hm);
            
            isTriggered = true;
        }
    }
}
