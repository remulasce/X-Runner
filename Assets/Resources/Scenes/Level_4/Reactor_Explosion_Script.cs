using UnityEngine;
using System.Collections;

public class Reactor_Explosion_Script : MonoBehaviour {

    [System.Serializable]
    public class ExplosionData
    {
        public GameObject explosion;
        public Vector3 offset;
        public float delay;
    }
    
    public ExplosionData[] explosions = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator SetOffExplosion(ExplosionData e)
    {
        yield return new WaitForSeconds(e.delay);
        GameObject explosion = (GameObject) Instantiate(e.explosion);
        explosion.transform.position = this.transform.position + e.offset;
        explosion.GetComponent<Detonator>().Explode();
    }

    void OnCollisionEnter(Collision other)
    {        
        if (other.gameObject.CompareTag("L1_Elite_Missile"))
        {            
            foreach (ExplosionData e in explosions)
            {
                StartCoroutine("SetOffExplosion", e);
            }
        }
    }
}
