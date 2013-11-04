using UnityEngine;
using System.Collections;

public class Transition_L3_L4_Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator LoadL4()
    {
        yield return new WaitForSeconds(10.0f);        
        Application.LoadLevel("Title_Screen");
    }

    IEnumerator StopFollowingShip()
    {
        yield return new WaitForSeconds(8.0f);
        Camera.main.GetComponent<CanabaltCamera>().player = null;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine("LoadL4");
            StartCoroutine("StopFollowingShip");
            animation.Play();
            other.gameObject.renderer.enabled = false;
            other.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            
            Camera.main.GetComponent<CanabaltCamera>().player = GameObject.FindGameObjectWithTag("L1_Elite");
        }
    }
}
