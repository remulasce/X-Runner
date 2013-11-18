using UnityEngine;
using System.Collections;

public class Destructive_Platform_Script : MonoBehaviour {

    public Vector3 forceToApply = Vector3.zero;
    public Vector3 pointToApplyForce = Vector3.zero;
    public float forceMagnitude = 0.0f;
    public Vector3 constantVectorForce = Vector3.zero;
    public float timeToApplyCF = 0.0f;
    public float delayToApplyForces = 0.0f;
    public RigidbodyConstraints postActivationConstraints = RigidbodyConstraints.None;

    public GameObject postActivationParticleSystem;
    public GameObject detonatorPrefab;

    public Vector3 detonatorOffset = new Vector3();

    private float startTime = 0.0f;

    private bool isPlayed = false;

	// Use this for initialization
	void Start () {
        forceToApply.Normalize();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Time.time - startTime < timeToApplyCF)
        {
            this.rigidbody.AddForce(constantVectorForce * -UnityEngine.Physics.gravity.y);
        }
	}

    public void ApplyStagedForce()
    {
        StartCoroutine("ApplyForceDelay");
    }

    public IEnumerator ApplyForceDelay()
    {        
        yield return new WaitForSeconds(delayToApplyForces);
        if (!isPlayed)
        {
            isPlayed = true;
            this.rigidbody.constraints = postActivationConstraints;
            this.rigidbody.AddForceAtPosition(forceToApply * forceMagnitude * -UnityEngine.Physics.gravity.y, pointToApplyForce + this.transform.position);
            startTime = Time.time;

            // If there is a particle System Attached, play it
            if (detonatorPrefab)
            {
                GameObject d = (GameObject)Instantiate(detonatorPrefab);
                d.transform.position = detonatorOffset + this.transform.position;
                d.GetComponent<Detonator>().Explode();
                print("here");
            }
            else if (postActivationParticleSystem)
            {
                if (postActivationParticleSystem.particleSystem)
                {
                    postActivationParticleSystem.particleSystem.Play();
                }
                else if (postActivationParticleSystem.particleEmitter)
                {
                    postActivationParticleSystem.particleEmitter.emit = true;
                }
            }
        }
        if (audio)
        {
            audio.Play();
        }
    }
}
