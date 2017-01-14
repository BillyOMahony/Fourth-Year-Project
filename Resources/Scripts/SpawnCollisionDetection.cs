using UnityEngine;
using System.Collections;

public class SpawnCollisionDetection : MonoBehaviour {

    public bool CanSpawn = true;
	
	// Update is called once per frame
	void OnTriggerEnter(Collider col)
    {
        CanSpawn = false;
    }

    void OnTriggerExit(Collider col)
    {
        CanSpawn = true;
    }
}
