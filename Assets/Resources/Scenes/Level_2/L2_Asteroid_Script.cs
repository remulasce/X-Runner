using UnityEngine;
using System.Collections;

public class L2_Asteroid_Script : MonoBehaviour {

    public float maxVelocity = 8.0f;
    public float extraHitVelocity = 0.2f;

    public bool hasReflectedOffPlayer = false; // Will be used to prevent the player from moving into an asteroid and causing it to reflect multiple times.

    public int numberOfTimesHit = 0;
    [HideInInspector]
    public enum LAST_HIT { NONE, PLAYER, ENEMY };
    public LAST_HIT lastHit = LAST_HIT.NONE;

	// Use this for initialization
	void Start () {
	
	}

    void killIfOutBounds()
    {
        if (Mathf.Abs(this.transform.position.magnitude) > 100)
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
        if (other.gameObject.CompareTag("L2_PlayerShot"))
        {
            if (lastHit != LAST_HIT.PLAYER)
            {
                numberOfTimesHit++;
                lastHit = LAST_HIT.PLAYER;
            }
            Instantiate(Resources.Load("Prefabs/Level_2/Explosions/L2_Asteroid_Impact_Explosion"), other.transform.position, Quaternion.Euler(0, 0, 0));
            Object.Destroy(other.gameObject);
            hasReflectedOffPlayer = true;
        }

        if (other.gameObject.CompareTag("L2_EnemyShot"))
        {
            if (lastHit != LAST_HIT.ENEMY)
            {
                numberOfTimesHit++;
                lastHit = LAST_HIT.ENEMY;
            }
            if (other.gameObject.name.Contains("Homing"))
            {
                other.gameObject.GetComponent<L2_Enemy_Shot_Homing_Script>().GetExplosion().transform.parent = this.gameObject.transform;
                other.gameObject.GetComponent<L2_Enemy_Shot_Homing_Script>().GetExplosion().Explode();
            }
            else
            {
                GameObject gDetonator = (GameObject) Instantiate(Resources.Load("Prefabs/Level_2/Explosions/L2_Asteroid_Impact_Explosion"), other.transform.position, Quaternion.Euler(0, 0, 0));
                gDetonator.transform.parent = this.gameObject.transform;
            }
            Object.Destroy(other.gameObject);
            hasReflectedOffPlayer = false;
        }
    }
}
