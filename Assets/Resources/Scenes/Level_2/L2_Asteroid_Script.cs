using UnityEngine;
using System.Collections;

public class L2_Asteroid_Script : MonoBehaviour {

    public float maxVelocity = 8.0f;
    public float extraHitVelocity = 1.0f;

    private int numberOfTimesHit = 0;

	// Use this for initialization
	void Start () {
	
	}

    void killIfOutBounds()
    {
        if (Mathf.Abs(this.transform.position.magnitude) > 60)
        {
            Destroy(this.gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
	    if (this.rigidbody.velocity.magnitude > (maxVelocity + (extraHitVelocity * numberOfTimesHit)))
        {
            this.rigidbody.velocity = rigidbody.velocity.normalized * (maxVelocity + (extraHitVelocity * numberOfTimesHit));
        }
        killIfOutBounds();
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("L2_PlayerShot") || other.gameObject.CompareTag("L2_EnemyShot"))
        {
            numberOfTimesHit++;
            if (other.gameObject.name.Contains("Homing"))
            {
                other.gameObject.GetComponent<L2_Enemy_Shot_Homing_Script>().GetExplosion().transform.parent = null;
                other.gameObject.GetComponent<L2_Enemy_Shot_Homing_Script>().GetExplosion().Explode();
            }
            Instantiate(Resources.Load("Prefabs/Level_2/L2_Asteroid_Impact_Explosion"), other.transform.position, Quaternion.Euler(0, 0, 0));
            Object.Destroy(other.gameObject);
        }
    }
}
