using UnityEngine;
using System.Collections;

public class ShieldUvAnimation : MonoBehaviour
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
    }

    IEnumerator RevertShieldColor()
    {
        this.mMaterial.color = shieldHitColor;
        yield return new WaitForSeconds(flashTime);
        this.mMaterial.color = initialColor;
    }
}