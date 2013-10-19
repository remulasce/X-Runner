using UnityEngine;
using System.Collections;

//Movement and control for the L2 TopDownShooter
public class L2_Ship_Script : MonoBehaviour
{

    [System.Serializable]
    public class MovementData
    {
        //Maximumu speed in each direction
        public float maxSpeed = 40;
        public float acceleration = 100;

        //When we're far from maxvelocity, we add some extra force to get us going
        public float startupBoost = 40;
        //At what speed we should stop applying the startup boost.
        public float startupCutoff = 40;
        public float frictionConstant = 8f;
    }

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
	
	// Use this for initialization
	void Start ()
	{
	
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
		float dx = Input.GetAxis ("Horizontal");
		float dy = Input.GetAxis ("Vertical");

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

        this.rigidbody.AddForce(movement.acceleration * new Vector3(dx, dy, 0) * Time.deltaTime);
	}
	
	//Adds extra boost when we're accelerating from low speeds, to add
	// responsiveness
	void addStartupBoost ()
	{
		float dx = Input.GetAxis ("Horizontal");
		float dy = Input.GetAxis ("Vertical");
		
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
        
        this.rigidbody.AddForce(-movement.frictionConstant * this.rigidbody.velocity * Time.deltaTime);
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
	}
	
	//Make a shot if we're shooting
	void doShooting ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			Instantiate(Resources.Load("Prefabs/Level_2/L2_Player_Shot"), this.transform.position, Quaternion.Euler(0, 0, 180));
			
		}
	}
	// Update is called once per frame
	void Update ()
	{
		doShooting ();
		addControl ();
		addStartupBoost ();        
		slowDown ();
		limitSpeed ();		
	}
}
