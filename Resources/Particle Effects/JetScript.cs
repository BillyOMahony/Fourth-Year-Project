using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetScript : MonoBehaviour {

    public GameObject Jet;
    public GameObject Smoke;
    public GameObject InnerLight;
    public GameObject OuterLight;
    public int JetRate = 500;
    public int SmokeRate = 150;
    public float OuterLightBrightness = 2.0f;
    public float InnerLightBrightness = 8.0f;

    PlayerControllerRB _PC;
    ParticleSystem.EmissionModule _JetEM;
    ParticleSystem.EmissionModule _SmokeEM;


    // Use this for initialization
    void Start () {
        _PC = gameObject.GetComponent<PlayerControllerRB>();

        _JetEM = Jet.GetComponent<ParticleSystem>().emission;
        _SmokeEM = Smoke.GetComponent<ParticleSystem>().emission;


    }
	
	// Update is called once per frame
	void Update () {
        UpdateJet();
        UpdateSmoke();
        UpdateLights();
	}

    public void UpdateJet()
    {
        _JetEM.rateOverTime = _PC.engine * JetRate;
    }

    public void UpdateSmoke()
    {
        _SmokeEM.rateOverTime = _PC.engine * SmokeRate;
    }

    public void UpdateLights()
    {
        InnerLight.GetComponent<Light>().intensity = InnerLightBrightness * _PC.engine;
        OuterLight.GetComponent<Light>().intensity = OuterLightBrightness * _PC.engine;
    }

}
