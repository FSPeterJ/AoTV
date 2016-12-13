using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogebaSquishedHandler : MonoBehaviour {
    public GameObject _this;
    void OnTriggerEnter(Collider col)
    {
        if (col.tag=="Player")
        {
            Debug.Log("Player kill dogeba");
            
            Destroy(_this.gameObject);
           
        }
    }
}
