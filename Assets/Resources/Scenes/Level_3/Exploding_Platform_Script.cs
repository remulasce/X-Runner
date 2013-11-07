using UnityEngine;
using System.Collections;

public class Exploding_Platform_Script : MonoBehaviour {

    public GameObject DetonatorPrefab = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && other.contacts[0].normal.y <= 0 && DetonatorPrefab)
        {
            GameObject explosion = (GameObject)Instantiate(DetonatorPrefab);
            explosion.transform.position = this.transform.position;
            explosion.GetComponent<Detonator>().size *= this.transform.localScale.x;
            Object.Destroy(this.gameObject);
        }
    }
}
