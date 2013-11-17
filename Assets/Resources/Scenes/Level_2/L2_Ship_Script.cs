using UnityEngine;
using System.Collections;

//Movement and control for the L2 TopDownShooter
public class L2_Ship_Script : MonoBehaviour, IPlayer
{

    [System.Serializable]
    public class MovementData
    {
        //Maximumu speed in each direction
        public float maxSpeed = 40;
        public float acceleration = 100;

        //When we're far from maxvelocity, we add some extra force to get us going
        public float startupBoost = 00;
		//If we want to change directions, we bring ourselves to zero really fast
		public float haltBoost = 00;
        //At what speed we should stop applying the startup boost.
        public float startupCutoff = 00;
		//Friction will only be applied when not accelerating in the current direction,
		//	so we don't have to change the acceleration & boost things every time we
		//	change this.
        public float frictionConstant = 0f;
    }

	public float reloadTime = .3f;
	
    [System.Serializable]
    public class BoundaryData
    {
        [Range(0.0f, 0.5f)]
        public float lowerHorizontalRange = 0.5f;

        [Range(0.5f, 1.0f)]
        public float upperHorizontalRange = 0.5f;        

        //-------------------------------

        [Range(0.0f, 0.5f)]
        public float lowerVerticalRange = 0.5f;

        [Range(0.5f, 1.0f)]
        public float upperVerticalRange = 0.5f;
    }

    public BoundaryData boundaryData = new BoundaryData();

    public MovementData movement = new MovementData();

    // How long it takes for the player to spawn after getting blown up
    public float spawnTime = 1.0f;

    // How long the player is invincible after re-spawning
    public float shieldTime = 1.0f;
    private float shieldSize;

    public Detonator explosion;

    // More stats stuff
    public Stat_Counter_Script stats = null;

    [HideInInspector]
    public bool isDead = false;
    
    private Vector3 startPosition = Vector3.zero;

    private bool isShielded = false;
	private float lastShot = 0;

    private bool aboutToDoTransition = false;
    private bool isShotDown = false;
	
    // Special case bool for elite
    private bool wasHitByElite = false;
	
	public bool IsDead() { return isDead; }
    public Vector3 GetPosition() { return transform.position; }

    // Variables used to control how many bullets are fired at any given time
    public int numShotsFiredThreshold = 50;
    [Range(1, 2)]
    public float reloadTimeMultiplier = 1f;
    private int numShotsFired = 0;
    private float baseReloadTime = 0;
    [Range(0, 0.5f)]
    public float maxReloadOffset = 0.0f;

    Music_Manager_Script musicManager;

    AudioSource[] audios;

	// Use this for initialization
	void Start ()
    {        
        startPosition = this.transform.position;
        shieldSize = this.transform.GetChild(0).localScale.x;

        isShielded = true;
        StartCoroutine("ResetShield");

        baseReloadTime = reloadTime;

        audios = this.gameObject.GetComponents<AudioSource>();

        musicManager = GameObject.FindGameObjectWithTag("AudioSourceManager").GetComponent<Music_Manager_Script>();

        stats = GameObject.FindGameObjectWithTag("Stats").GetComponent<Stat_Counter_Script>();
	}

    private bool checkLowerBoundaryX()
    {
        Vector3 viewPortPos = Camera.main.WorldToViewportPoint(this.transform.position);

        if (viewPortPos.x < boundaryData.lowerHorizontalRange)
        {
            //Debug.Log("Past Lower Boundary X");            
            return false; // Then the player cannot move in this direction anymore
        }
        return true;
    }

    private bool checkUpperBoundaryX()
    {
        Vector3 viewPortPos = Camera.main.WorldToViewportPoint(this.transform.position);

        if (viewPortPos.x > boundaryData.upperHorizontalRange)
        {
            //Debug.Log("Past Upper Boundary X");
            return false; // Then the player cannot move in this direction anymore
        }
        return true;
    }

    private bool checkLowerBoundaryY()
    {
        Vector3 viewPortPos = Camera.main.WorldToViewportPoint(this.transform.position);

        if (viewPortPos.y < boundaryData.lowerVerticalRange)
        {
            //Debug.Log("Past Lower Boundary Y");
            return false; // Then the player cannot move in this direction anymore
        }

        return true;
    }

    private bool checkUpperBoundaryY()
    {
        Vector3 viewPortPos = Camera.main.WorldToViewportPoint(this.transform.position);

        if (viewPortPos.y > boundaryData.upperVerticalRange)
        {
            //Debug.Log("Past Upper Boundary Y");
            return false; // Then the player cannot move in this direction anymore
        }
        return true;
    }
	
	//Add the arrow-key primary movement
	void addControl ()
	{
		float dx = Input.GetAxisRaw ("Horizontal");
		float dy = Input.GetAxisRaw ("Vertical");

        //----------------------------------------

        // Check to see if the player is past the x boundaries.  If so, prohibit movement in the X direction
        bool canMoveX = true;

        if (dx < 0)
        {
            canMoveX = checkLowerBoundaryX();
        }
        else if (dx > 0)
        {
            canMoveX = checkUpperBoundaryX();
        }

        if (!canMoveX)
        {
            dx = 0;
        }

        //----------------------------------------

        // Check to see if the player is past the y boundaries.  If so, prohibit movement in the Y direction
        bool canMoveY = true;

        if (dy < 0)
        {
            canMoveY = checkLowerBoundaryY();
        }
        else if (dy > 0)
        {
            canMoveY = checkUpperBoundaryY();
        }

        if (!canMoveY)
        {
            dy = 0;
        }

        //----------------------------------------
		
		if (dx * rigidbody.velocity.x < 0)
		{
			this.rigidbody.AddForce(movement.haltBoost * new Vector3(Mathf.Sign(dx), 0, 0) * Time.deltaTime);
		}
		if (dy * rigidbody.velocity.y < 0)
		{
			this.rigidbody.AddForce(movement.haltBoost * new Vector3(0, Mathf.Sign(dy), 0) * Time.deltaTime);
		}

        if ((Mathf.Abs(dx) > 0 || Mathf.Abs(dy) > 0) && !audios[0].isPlaying)
        {
            audios[0].Play();
        }
        else if ((dx == 0 && dy == 0) && audios[0].isPlaying)
        {
            audios[0].Stop();
        }		
		
        this.rigidbody.AddForce(movement.acceleration * new Vector3(dx, dy, 0) * Time.deltaTime);
	}
	
	//Adds extra boost when we're accelerating from low speeds, to add
	// responsiveness
	void addStartupBoost ()
	{
        float dx = Input.GetAxisRaw ("Horizontal");
		float dy = Input.GetAxisRaw ("Vertical");
		
		float vx = this.rigidbody.velocity.x;
		float vy = this.rigidbody.velocity.y;
		
		float signX = Mathf.Sign (vx);
		float signY = Mathf.Sign (vy);
		
		//Tricksie absolute value
        if (signX * vx < movement.startupCutoff && dx != 0)
        {
            // Check to see if the player is past the x boundaries.  If so, prohibit movement in the X direction
            bool canMoveX = true;

            if (dx < 0)
            {
                canMoveX = checkLowerBoundaryX();
            }
            else if (dx > 0)
            {
                canMoveX = checkUpperBoundaryX();
            }

            //----------------------------------------

            if (canMoveX)
            {
                this.rigidbody.AddForce(movement.startupBoost * dx * Time.deltaTime, 0, 0);
            }
            else
            {
                this.rigidbody.velocity = new Vector3(0, this.rigidbody.velocity.y, 0);
            }
		}
        if (signY * vy < movement.startupCutoff && dy != 0)
        {
            // Check to see if the player is past the y boundaries.  If so, prohibit movement in the Y direction
            bool canMoveY = true;

            if (dy < 0)
            {
                canMoveY = checkLowerBoundaryY();
            }
            else if (dy > 0)
            {
                canMoveY = checkUpperBoundaryY();
            }

            //----------------------------------------

            if (canMoveY)
            {
                this.rigidbody.AddForce(0, movement.startupBoost * dy * Time.deltaTime, 0);
            }
            else
            {
                this.rigidbody.velocity = new Vector3(this.rigidbody.velocity.x, 0, 0);
            }
		}
	}
	
	//Apply a bit of zeroing friction
	void slowDown ()
	{
        // Check to see if the player is past the X/Y boundaries.  If so, stop the player at the appropriate place.
        if (!checkLowerBoundaryX() || !checkUpperBoundaryX())
        {
            this.rigidbody.velocity = new Vector3(0, this.rigidbody.velocity.y, 0);
        }

        if (!checkLowerBoundaryY() || !checkUpperBoundaryY())
        {
            this.rigidbody.velocity = new Vector3(this.rigidbody.velocity.x, 0, 0);
        }
        
		//Control
		float dx = Input.GetAxisRaw ("Horizontal");
		float dy = Input.GetAxisRaw ("Vertical");

        //Debug.Log(dx + " " + dy);
		
        
		//Friction will only be responsible for bringing a ship to a halt with no control-
		//  regular acceleration should ensure its curve is sufficient for a ship travelling
		//	full speed in the other direction.
		if (dx == 0)
		{
			float boost = 1;
			if (Mathf.Abs(this.rigidbody.velocity.x) < 1) { boost = 2 - Mathf.Abs(this.rigidbody.velocity.x); }
			this.rigidbody.AddForce(new Vector3(-movement.frictionConstant * boost * this.rigidbody.velocity.x * Time.deltaTime, 0, 0), ForceMode.VelocityChange);
		}
		if (dy == 0)
		{
			this.rigidbody.AddForce(new Vector3(0, -movement.frictionConstant * this.rigidbody.velocity.y * Time.deltaTime, 0), ForceMode.VelocityChange);
		}
	}
	
	//Enforce max speed limit
	void limitSpeed ()
	{
        if (Mathf.Abs(this.rigidbody.velocity.x) > movement.maxSpeed)
        {
            this.rigidbody.velocity = new Vector3(movement.maxSpeed * Mathf.Sign(this.rigidbody.velocity.x), this.rigidbody.velocity.y, 0);
		}
        if (Mathf.Abs(this.rigidbody.velocity.y) > movement.maxSpeed)
        {
            this.rigidbody.velocity = new Vector3(this.rigidbody.velocity.x, movement.maxSpeed * Mathf.Sign(this.rigidbody.velocity.y), 0);
		}

        if (Vector3.Magnitude(this.rigidbody.velocity) > movement.maxSpeed)
        {
            this.rigidbody.velocity = this.rigidbody.velocity.normalized * movement.maxSpeed;
        }
	}
	
	//Make a shot if we're shooting
	void doShooting ()
	{
        if (!isDead)
        {
            if (Input.GetButton("Jump") && Time.time > lastShot + reloadTime)
            {
                if (numShotsFired > numShotsFiredThreshold)
                {
                    if (reloadTime < maxReloadOffset)
                    {
                        reloadTime *= reloadTimeMultiplier;
                    }
                    else
                    {
                        reloadTime = maxReloadOffset;
                    }
                }
                GameObject g = (GameObject)Instantiate(Resources.Load("Prefabs/Level_2/L2_Player_Shot"), this.transform.position, Quaternion.Euler(0, 90, 00));
                lastShot = Time.time;
                numShotsFired++;
            }
            if (Input.GetButtonUp("Jump"))
            {
                numShotsFired = 0;
                reloadTime = baseReloadTime;
            }
        }
	}
	// Update is called once per frame
	void Update ()
	{
        if (!isShotDown)
        {
            doShooting();
            addControl();
            addStartupBoost();
            slowDown();
            limitSpeed();
        }
	}

    IEnumerator TransitionToL3()
    {
        yield return new WaitForSeconds(4.0f);
        Application.LoadLevel("Level_3_Graybox");
    }

    void DoTransition()
    {
        isShotDown = true;

        animation.Play();

        StartCoroutine("TransitionToL3");

        musicManager.FadeOutSongs(2, new int[] { 6 });
    }

    void OnCollisionEnter(Collision col)
    {
        if ((col.gameObject.CompareTag("L2_EnemyShot") && (!isShielded || aboutToDoTransition)))
        {
            if (!aboutToDoTransition)
            {                
                isDead = true;
                explosion.transform.position = this.transform.position;
                explosion.Explode();
                this.transform.position = new Vector3(0, 0, 1000);
                StartCoroutine("Respawn");
            }
            else
            {
                explosion.transform.position = this.transform.position;
                explosion.Explode();
                DoTransition();
            }

            col.gameObject.transform.DetachChildren();
            Destroy(col.gameObject);
        }

        if (col.gameObject.CompareTag("L2_Enemy"))
        {

            if (!col.gameObject.name.Contains("Elite"))
            {
                col.gameObject.GetComponent<L2_Enemy_Script>().GetExplosion().transform.parent = null;
                col.gameObject.GetComponent<L2_Enemy_Script>().GetExplosion().transform.position = col.gameObject.transform.position;
                col.gameObject.GetComponent<L2_Enemy_Script>().GetExplosion().Explode();
                Destroy(col.gameObject);
            }
            else
            {
                if (col.gameObject.GetComponent<L2_Elite_Script>().isShotDown)
                {
                    return;
                }
            }

            if (!this.isShielded || col.gameObject.name.Contains("Elite"))
            {
                explosion.transform.position = this.transform.position;
                explosion.Explode();
                isDead = true;
                this.transform.position = new Vector3(0, 0, 1000);
                if (col.gameObject.name.Contains("Elite"))
                {
                    wasHitByElite = true;
                }
                StartCoroutine("Respawn");
            }            
        }

        if (col.gameObject.CompareTag("L2_Asteroid"))
        {
            if (!this.isShielded)
            {
                explosion.transform.position = this.transform.position;
                explosion.Explode();
                isDead = true;
                this.transform.position = new Vector3(0, 0, 1000);
                StartCoroutine("Respawn");
            }
            else
            {
                if (!col.gameObject.GetComponent<L2_Asteroid_Script>().hasReflectedOffPlayer)
                {
                    col.rigidbody.velocity = Vector3.Reflect(col.rigidbody.velocity, col.contacts[0].normal);
                    col.gameObject.GetComponent<L2_Asteroid_Script>().hasReflectedOffPlayer = true;
                    col.gameObject.GetComponent<L2_Asteroid_Script>().lastHit = L2_Asteroid_Script.LAST_HIT.PLAYER;
                    col.gameObject.GetComponent<L2_Asteroid_Script>().numberOfTimesHit++;
                }
            }
        }        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name.Contains("Cinematic"))
        {
            aboutToDoTransition = true;
            return;
        }
        
        if (col.gameObject.CompareTag("Enemy_Shield"))
        {
            if (!col.gameObject.name.Contains("Elite"))
            {
                col.gameObject.GetComponent<L2_Enemy_Script>().GetExplosion().transform.parent = null;
                col.gameObject.GetComponent<L2_Enemy_Script>().GetExplosion().transform.position = col.gameObject.transform.position;
                col.gameObject.GetComponent<L2_Enemy_Script>().GetExplosion().Explode();
                Destroy(col.gameObject);
            }

            if (!this.isShielded && !col.gameObject.name.Contains("Elite"))
            {
                explosion.transform.position = this.transform.position;
                explosion.Explode();
                isDead = true;
                this.transform.position = new Vector3(0, 0, 1000);
                StartCoroutine("Respawn");
            }
        }
    }

    IEnumerator Respawn()
    {
        print("Respawning...");
        if (stats)
        {
            stats.numberOfDeaths++;
        }
        yield return new WaitForSeconds(spawnTime);
        this.transform.position = startPosition;
        this.rigidbody.velocity = Vector3.zero;
        isDead = false;
        print("Respawned!");
        isShielded = true;
        numShotsFired = 0;        
        StartCoroutine("ResetShield");
        audios[1].Play();
    }

    IEnumerator ResetShield()
    {
        this.transform.GetChild(0).localScale = new Vector3(shieldSize, shieldSize, shieldSize);
        print("Shielded!");
        yield return new WaitForSeconds(shieldTime);
        if (!wasHitByElite)
        {
            this.transform.FindChild("Shield_Dome").animation.Play("Shield_Collapse");
            print("NOT Shielded...");
            isShielded = false;
        }
        else
        {
            wasHitByElite = false;
        }
    }
}
