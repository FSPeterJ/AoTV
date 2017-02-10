using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveController : MonoBehaviour {

    [SerializeField] bool shockwaveActive = false;
    CapsuleCollider cCollider;
    [SerializeField] GameObject pillarOne;
    float velosity = 2;
    float newScale = 0;
    float yPos = -3;


    float pillarOneFinalZ=-2.6f;
    float pillarOneFinalX=-6.13f;
    float pillarOneFinalY=0;
    float pillarTwoFinalZ=5.05f;
    float pillarTwoFinalX=4.86f;
    float pillarTwoFinalY=0;
    float pillarThreeFinalZ = -2.04f;
    float pillarThreeFinalX= 6.26f;
    float pillarThreeFinalY=0;
    float pillarFourFinalZ=-6.04f;
    float pillarFourFinalX=-0.24f;
    float pillarFourFinalY=0;
    float pillarFiveFinalZ=5.52f;
    float pillarFiveFinalX=-3.88f;
    float pillarFiveFinalY=0;
    // Use this for initialization
    void Start () {
        cCollider = GetComponent<CapsuleCollider>();
        cCollider.transform.localPosition= new Vector3(0,yPos,0);
        pillarOne.transform.transform.localPosition = new Vector3(0, yPos, 0);
      //pillarOneFinalZ;
      //pillarOneFinalX;
      //pillarOneFinalY;
      //pillarTwoFinalZ;
      //pillarTwoFinalX;
      //pillarTwoFinalY;
      //pillarThreeFinalZ;
      //pillarThreeFinalX;
      //pillarThreeFinalY;
      //pillarFourFinalZ;
      //pillarFourFinalX;
      //pillarFourFinalY;
      //pillarFiveFinalZ;
      //pillarFiveFinalX;
      //pillarFiveFinalY;
    }
	
	// Update is called once per frame
	void Update () {
		if(shockwaveActive==true)
        {
            if (cCollider.transform.localScale.x <1)
            {
                newScale += velosity * Time.deltaTime;
                cCollider.transform.localScale = new Vector3(newScale, 1, newScale);
                pillarOne.transform.transform.localScale = new Vector3(newScale, newScale, newScale);
            }
        }
	}
}
