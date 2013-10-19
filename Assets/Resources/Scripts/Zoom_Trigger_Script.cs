using UnityEngine;
using System.Collections;

public class Zoom_Trigger_Script : MonoBehaviour {

    [Range(-5, -100)]
    public float zoomPosition = -10.0f;

    [Range(0.001f, 5.0f)]
    public float zoomSpeed = 1.0f;

    public CanabaltCamera mainCamera;

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
            mainCamera.ZoomToPosition(zoomPosition, zoomSpeed);
        }
    }
}
