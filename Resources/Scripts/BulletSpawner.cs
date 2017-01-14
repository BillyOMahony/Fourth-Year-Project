using UnityEngine;
using System.Collections;

/// <summary>
/// This is a test script for bullet damage.
/// </summary>
public class BulletSpawner : MonoBehaviour {

    public GameObject projectile;

    GameObject NewProjectile;
    bool CanShoot = true;
    float timer = 0.2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (CanShoot)
        {
            NewProjectile = PhotonNetwork.Instantiate("Bullet", transform.position, transform.rotation, 0) as GameObject;
            CanShoot = false;
            timer = .2f;
        }
        ShootTimer();
    }

    void ShootTimer()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            CanShoot = true;
        }
    }
}