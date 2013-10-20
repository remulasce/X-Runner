using UnityEngine;
using System.Collections;

public class GUI_Trigger_Script : MonoBehaviour {

    // Image has to have the tutorial script on it to work
    public GUITexture image;

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
            image.GetComponent<Tutorial_GUI_Script>().setOn();
        }
    }
}
