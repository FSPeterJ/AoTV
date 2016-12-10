using UnityEngine;
using System.Collections;

public class StompParticleEffect : MonoBehaviour {

    Vector3 targetscale = new Vector3(1.1f, 1.1f, 1.1f);
    [SerializeField]
    public  ParticleSystem InnerRing, OuterRing,Bubbles,Blur;
	// Use this for initialization
	void Start () {
        
       
    }
	
	// Update is called once per frame
	void Update () {
        InnerRing.transform.localScale=GetComponent<ParticleSystem>().transform.localScale;

        if (InnerRing.transform.localScale ==targetscale )
        {
            InnerRing.transform.localScale = GetComponent<ParticleSystem>().transform.localScale;
            if (GetComponent<ParticleSystem>().transform.localScale.x > 1.1)
            {
                GetComponent<ParticleSystem>().transform.localScale = targetscale;
            }
        }
        
	
	}
}
