using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    { 
        if(col.tag == "Player")
        {
            col.transform.parent.GetComponent<PlayerManager>().OutOfBounds();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.tag == "Player")
        {
            col.transform.parent.GetComponent<PlayerManager>().BackInBounds();
        }
    }
}
