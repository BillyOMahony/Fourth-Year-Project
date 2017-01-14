using UnityEngine;
using System.Collections;

public class Shoot : Photon.PunBehaviour{

    public float RateOfFire = 0.2f;

    public string owner;

    public GameObject projectile;
    public GameObject projectileSpawner;

    GameObject NewProjectile;
    Vector3 SpawnPosition;
    GameObject Gun;
    bool CanShoot = true;
    float timer;
    AudioSource audio;

	// Use this for initialization
	void Start () {
        audio = projectileSpawner.GetComponent<AudioSource>();
        owner = GetComponent<PhotonView>().owner.NickName;
    }
	
	// Update is called once per frame
	void Update () {

        if (photonView.isMine == false && PhotonNetwork.connected == true)
        {
            return;
        }

        FireProjectile();
    }

    void FireProjectile()
    {
        if (Input.GetButton("Fire1") && CanShoot)
        {
            //audio.Play();
            NewProjectile = PhotonNetwork.Instantiate("Bullet", projectileSpawner.transform.position, projectileSpawner.transform.rotation, 0) as GameObject; //This line for photon
            //NewProjectile.GetComponent<Bullet>().owner = owner;
            //NewProjectile = Instantiate(projectile, projectileSpawner.transform.position, projectileSpawner.transform.rotation) as GameObject; // use this line for engine test

            CanShoot = false;
            timer = RateOfFire;
        }
        ShootTimer();
    }

    void ShootTimer()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            CanShoot = true;
        }
    }
}
