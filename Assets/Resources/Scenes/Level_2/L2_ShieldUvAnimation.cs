using UnityEngine;
using System.Collections;

// For reference, I found this at: http://wiki.unity3d.com/index.php?title=Shield

public class L2_ShieldUvAnimation : MonoBehaviour
{
    public GameObject iShield;
    public float iSpeed;

    private Material mMaterial;
    private float mTime;

    // Animation specific stuff    
    public Color shieldHitColor = Color.white;
    [Range(0.01f, 1.0f)]
    public float flashTime = 0.01f;
    private Color initialColor;

    // Tag to compare to for the shield reflection
    public string tagToCompareFor = "";

    AudioSource[] audios;

    // Use this for initialization
    void Start()
    {
        mMaterial = iShield.renderer.material;

        mTime = 0.0f;

        initialColor = this.mMaterial.color;

        audios = this.gameObject.GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        mTime += Time.deltaTime * iSpeed;

        mMaterial.SetFloat("_Offset", Mathf.Repeat(mTime, 1.0f));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(tagToCompareFor))
        {
            if (other.gameObject.GetComponent<L2_Enemy_Shot_Script>().isNotCinematic)
            {
                if (other.gameObject.name.Contains("Homing"))
                {
                    if (!other.gameObject.name.Contains("Cinematic"))
                    {
                        other.gameObject.GetComponent<L2_Enemy_Shot_Homing_Script>().isDestroyed = true;
                        other.gameObject.GetComponent<L2_Enemy_Shot_Homing_Script>().GetExplosion().transform.parent = null;
                        other.gameObject.GetComponent<L2_Enemy_Shot_Homing_Script>().GetExplosion().Explode();
                        other.gameObject.transform.position = new Vector3(0, 0, -100);
                        other.gameObject.audio.enabled = false;
                        Object.Destroy(other.gameObject, 5.0f);
                    }
                }
                else
                {
                    Object.Destroy(other.gameObject);
                }
                StartCoroutine("RevertShieldColor");
            }            
        }
    }

    IEnumerator RevertShieldColor()
    {
        audios[0].Play();
        this.mMaterial.color = shieldHitColor;
        yield return new WaitForSeconds(flashTime);
        this.mMaterial.color = initialColor;
    }

    public void PlayShieldUp()
    {
        audios[1].Play();
    }

    public void PlayShieldDown()
    {
        audios[2].Play();
    }    
}