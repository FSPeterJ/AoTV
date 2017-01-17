using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogebaSquishedHandler : MonoBehaviour {
    public GameObject _this;
    void OnTriggerEnter(Collider col)
    {
        if (col.tag=="Player")
        {
            //Debug.Log("Player kill dogeba");

            col.GetComponent<BasePlayer>().AddImpact(col.transform.position - transform.position, 150);

            //Debug.Log("Player pop");
            Destroy(_this.gameObject);
           
        }
    }
}
