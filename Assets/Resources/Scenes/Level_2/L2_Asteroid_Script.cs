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

    // Special bool to prevent the elite from shooting at an asteroid a zillion times
    public bool targetedByEnemy = false;

    // Grab References to the player and the elite.
    public Vector3 elitePosition = Vector3.zero;
    public GameObject player = null;

    public bool isCinematic = false;

    AudioSource[] audios;
    /*
     even # = player pings
     odd # = enemy pings
     */

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");

        audios = this.gameObject.GetComponents<AudioSource>();
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
                targetedByEnemy = false;
                if (lastHit != LAST_HIT.PLAYER)
                {
                    if (!player.GetComponent<L2_Ship_Script>().isDead) // Fixes a bug where asteroids just sit still in space
                    {
                        if (!isCinematic)
                        {
                            // Make sure to instantiate the prefab object for this
                            GameObject sound = (GameObject)Instantiate(Resources.Load("Prefabs/Cross_Level/Audio_SFX_Object"));
                            sound.GetComponent<Audio_SFX_Object_Script>().StartSound(audios[Mathf.Clamp(numberOfTimesHit, 0, 8)]);
                            sound.transform.position = this.transform.position;
                        

                            if ((((((this.rigidbody.velocity.x > 0 && player.transform.position.x < elitePosition.x)
                                || this.rigidbody.velocity.x < 0 && player.transform.position.x > elitePosition.x)))
                                || numberOfTimesHit > 2) && this.transform.position.y < (elitePosition.y + 1)
                                ) // Do Reflecting always if # of hits > 2 && the y position is not too high over the elite
                            {
                                this.rigidbody.velocity = Vector3.Normalize(elitePosition - player.transform.position) * (maxVelocity + extraHitVelocity);
                            }
                        }
                    }

                    numberOfTimesHit++;
                    lastHit = LAST_HIT.PLAYER;
                    print(lastHit);
                }
                Instantiate(Resources.Load("Prefabs/Level_2/Explosions/L2_Asteroid_Impact_Explosion"), other.transform.position, Quaternion.Euler(0, 0, 0));
                Object.Destroy(other.gameObject);
                hasReflectedOffPlayer = true;
            }

            if (other.gameObject.CompareTag("L2_EnemyShot"))
            {
                targetedByEnemy = false;
                if (lastHit != LAST_HIT.ENEMY)
                {
                    if (lastHit == LAST_HIT.PLAYER)
                    {
                        if (numberOfTimesHit > 0)
                        {
                            // Make sure to instantiate the prefab object for this
                            GameObject sound = (GameObject)Instantiate(Resources.Load("Prefabs/Cross_Level/Audio_SFX_Object"));
                            sound.GetComponent<Audio_SFX_Object_Script>().StartSound(audios[Mathf.Clamp(numberOfTimesHit, 1, 7)]);
                            sound.transform.position = this.transform.position;
                        }
                        if ((((((this.rigidbody.velocity.x > 0 && player.transform.position.x > elitePosition.x)
                            || this.rigidbody.velocity.x < 0 && player.transform.position.x < elitePosition.x)))
                            || numberOfTimesHit > 2) && this.transform.position.y < (elitePosition.y + 1)
                            ) // Do Reflecting always if # of hits > 2 && the y position is not too high over the elite
                        {
                            this.rigidbody.velocity = Vector3.Normalize(player.transform.position - elitePosition) * (maxVelocity + extraHitVelocity);
                        }
                        numberOfTimesHit++;
                    }

                    lastHit = LAST_HIT.ENEMY;
                    print(lastHit);
                }
                if (other.gameObject.name.Contains("Homing"))
                {
                    other.gameObject.GetComponent<L2_Enemy_Shot_Homing_Script>().GetExplosion().transform.parent = this.gameObject.transform;
                    other.gameObject.GetComponent<L2_Enemy_Shot_Homing_Script>().GetExplosion().Explode();
                }
                else
                {
                    GameObject gDetonator = (GameObject)Instantiate(Resources.Load("Prefabs/Level_2/Explosions/L2_Asteroid_Impact_Explosion"), other.transform.position, Quaternion.Euler(0, 0, 0));
                    gDetonator.transform.parent = this.gameObject.transform;
                }
                Object.Destroy(other.gameObject);
                hasReflectedOffPlayer = false;
            }
        
    }
}
