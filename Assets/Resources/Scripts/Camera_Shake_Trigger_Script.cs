using UnityEngine;
using System.Collections;

public class Camera_Shake_Trigger_Script : MonoBehaviour {

    public CanabaltCamera mainCamera;

    public GameObject objectToCompareTagWith;

    [Range(0.01f, 10.0f)]
    public float shakeDuration = 1.0f;

    [Range(0.1f, 5.0f)]
    public float shakePower = 1.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        //print(other.gameObject.tag);
        if (other.gameObject.CompareTag(objectToCompareTagWith.tag))
        {
            mainCamera.BeginShake(shakeDuration, shakePower);
        }
    }
}
