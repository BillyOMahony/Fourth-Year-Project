using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour {

    public ParticleSystem Explosion;

	// Use this for initialization
	void Start () {
		
	}
    

    public void PlayExplosion()
    {
        Explosion.Play();
    }

}
