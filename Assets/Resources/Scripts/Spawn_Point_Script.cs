using UnityEngine;
using System.Collections;

public class Spawn_Point_Script : MonoBehaviour {

    public float rayCastDistance = 1.0f;
    public bool invertedGravity = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //checkForGround();
	}

    public bool checkForGround()
    {
        Ray ray = new Ray(this.transform.position, new Vector3(0, -1, 0));
        RaycastHit hit;

        if (invertedGravity)
        {
            ray.direction *= -1;
        }

        if (!Physics.Raycast(ray, out hit, rayCastDistance))
        {
            Debug.DrawRay(ray.origin, ray.direction * rayCastDistance, Color.red, 10.0f);
            return false;
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * rayCastDistance, Color.green, 10.0f);
            return true;
        }
    }
}
