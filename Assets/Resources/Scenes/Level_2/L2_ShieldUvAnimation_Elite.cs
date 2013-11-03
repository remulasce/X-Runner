﻿using UnityEngine;
using System.Collections;

// For reference, I found this at: http://wiki.unity3d.com/index.php?title=Shield

public class L2_ShieldUvAnimation_Elite : MonoBehaviour
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

    [Range(0,15)]
    public float timeToRegenerateShields = 3.0f;

    // Tag to compare to for the shield reflection
    public string tagToCompareFor = "";

    // Use this for initialization
    void Start()
    {
        mMaterial = iShield.renderer.material;

        mTime = 0.0f;

        initialColor = this.mMaterial.color;
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
            Object.Destroy(other.gameObject);
            StartCoroutine("RevertShieldColor");
        }

        // Special Case for the Elite
        if (other.gameObject.CompareTag("L2_Asteroid"))
        {
            GameObject gDetonator = (GameObject)Instantiate(Resources.Load("Prefabs/Level_2/Explosions/L2_Asteroid_Impact_Explosion"), other.transform.position, Quaternion.Euler(0, 0, 0));
            gDetonator.GetComponent<Detonator>().size = 5;
            Object.Destroy(other.gameObject);
            this.animation.Play("Shield_Collapse_Elite");
            StartCoroutine("BringBackShield");
        }
    }

    IEnumerator RevertShieldColor()
    {        
        this.mMaterial.color = shieldHitColor;
        yield return new WaitForSeconds(flashTime);
        this.mMaterial.color = initialColor;
    }

    IEnumerator BringBackShield()
    {        
        yield return new WaitForSeconds(timeToRegenerateShields);
        this.animation.Play("Shield_Regen_Elite");
    }
}