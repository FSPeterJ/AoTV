using UnityEngine;
using System.Collections;

public class StompEffect : MonoBehaviour {

    public ParticleSystem InnerRing;
    public ParticleSystem OuterRing;
    public ParticleSystem Bubbles;
    public ParticleSystem Blur;
   public  float startingSize;
   public  float endingSize;
    private Vector3 parent;
    float currentscale = 0.1f;
    bool enabled;
	// Use this for initialization
	void Start ()
    {
        InnerRing.scalingMode = ParticleSystemScalingMode.Shape;
        OuterRing.scalingMode = ParticleSystemScalingMode.Shape;
        Bubbles.scalingMode = ParticleSystemScalingMode.Shape;
        Blur.scalingMode = ParticleSystemScalingMode.Shape;
        parent = GetComponent<ParticleSystem>().transform.localScale;
        ResetStartingSize();
        enabled = InnerRing.enableEmission;
    }

    private void ResetStartingSize()
    {
        
        GetComponent<ParticleSystem>().transform.localScale *= .5f;
        InnerRing.transform.localScale = parent;
        OuterRing.transform.localScale = parent;
        Bubbles.transform.localScale = parent;
        Blur.transform.localScale = parent;
        currentscale = 0.0f;
    }
    public void ToggleEmission()
    {
        
        {
            InnerRing.enableEmission = !enabled;
            OuterRing.enableEmission = !enabled;
            Bubbles.enableEmission = !enabled;
            Blur.enableEmission = !enabled;

        }
    }
    // Update is called once per frame
    void Update () {
        if (enabled == true)
        {
            if (currentscale <= 1.1)
            {
                InnerRing.scalingMode = ParticleSystemScalingMode.Shape;
                InnerRing.transform.localScale *= 1.25f;
                OuterRing.transform.localScale *= 1.25f;
                Bubbles.transform.localScale *=   1.25f;
                Blur.transform.localScale *= 1.25f;
                currentscale *= 1.25f;
            }
        }
        else
        {
            ResetStartingSize();
        }
        
}
}
