using UnityEngine;
using System.Collections;


public class L2_Enemy_Shot_Target_Script : L2_Enemy_Shot_Script
{
    protected GameObject player;

    // Specialized target
    public GameObject target = null;

	// Use this for initialization
	protected new void Start () {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        this.transform.LookAt(player.transform.position);
        this.rigidbody.velocity = this.transform.forward * speed;
	}
	
	// Update is called once per frame
    protected new void Update()
    {
        base.Update();
	}

    public void SetTarget(GameObject target)
    {
        this.target = target;
        this.transform.LookAt(target.transform.position);
        this.rigidbody.velocity = this.transform.forward * speed;
    }
}
