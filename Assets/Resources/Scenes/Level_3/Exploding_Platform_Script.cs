using UnityEngine;
using System.Collections;

public class Exploding_Platform_Script : MonoBehaviour {

    public GameObject DetonatorPrefab = null;

    public Vector3 explosionVector = new Vector3(0, 1, 0);

    [Range(-1, 1)]
    public float explosionThreshold = 0.9f;

	// Use this for initialization
	void Start () {
        explosionVector.Normalize();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionExit(Collision other)
    {        
        Vector3 augmentedNorm = other.contacts[0].normal;
        augmentedNorm.y *= -1;
        if (other.gameObject.CompareTag("Player") && Vector3.Dot(augmentedNorm, explosionVector) >= explosionThreshold && DetonatorPrefab)
        {
            GameObject explosion = (GameObject)Instantiate(DetonatorPrefab);
            explosion.transform.position = this.transform.position;
            explosion.GetComponent<Detonator>().size *= this.transform.localScale.x;
            Object.Destroy(this.gameObject);
        }
    }
}
