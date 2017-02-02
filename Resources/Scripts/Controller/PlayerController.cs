using UnityEngine;
using System.Collections;

public class PlayerController : Photon.PunBehaviour {

    public bool disabled = false;
    public string owner;

    #region public variables movement

    //Network variables
    public float speed = 0f;
    public Vector3 turn;

    //public 

    public float maxSpeed = 10.0f;

    public float turnSpeed = 10f;

    public float engine = 0f;
    public float engineAcceleration = 0.2f;

    #endregion

    #region public variables shoot

    public float RateOfFire = 0.2f;

    public GameObject projectile;
    public GameObject projectileSpawner;

    #endregion

    #region private variables

    GameObject _spaceship;

    GameObject NewProjectile;
    Vector3 SpawnPosition;
    bool CanShoot = true;
    float timer;
    AudioSource audio;

    #endregion

    #region MonoBehaviour Methods

    // Use this for initialization
    void Start()
    {
        PhotonNetwork.sendRate = 60;
        PhotonNetwork.sendRateOnSerialize = 30;

        _spaceship = gameObject;
        audio = projectileSpawner.GetComponent<AudioSource>();
        owner = GetComponent<PhotonView>().owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {

        if (disabled)
        {
            return;
        }

        if (photonView.isMine)
        {
            SetThrottle();
            SetTurn();
            FireProjectile();

            ApplyThrottle();
            ApplyTurn();
        }
        /*
        ApplyThrottle();
        ApplyTurn();
        */

        ShootTimer();
    }

    #endregion

    #region public methods

    public void Disabled()
    {
        disabled = true;
        if (disabled)
        {
            engine = 0f;
        }
    }

    public void Enabled()
    {
        disabled = false;
    }

    #endregion

    #region private methods

    void SetThrottle()
    {
        if (Input.GetButton("Forward"))
        {
            engine += engineAcceleration * Time.deltaTime * Input.GetAxis("Forward");

            if (engine > 1)
            {
                engine = 1;
            }
            else if (engine < 0)
            {
                engine = 0;
            }
        }
        speed = maxSpeed * engine;
    }

    void ApplyThrottle()
    {
        _spaceship.transform.position += transform.forward * speed * Time.deltaTime;
    }

    void SetTurn()
    {
        turn = Vector3.up * Time.deltaTime * Input.GetAxis("Horizontal") * turnSpeed;//Vector3.up = right?!?! These things sort themselves out...
    }

    void ApplyTurn()
    {
        _spaceship.transform.Rotate(turn);
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
    }

    void ShootTimer()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            CanShoot = true;
        }
    }

    #endregion

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting == true)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        //    stream.SendNext(speed);
        //    stream.SendNext(turn);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        //    speed = (float)stream.ReceiveNext();
        //    turn = (Vector3)stream.ReceiveNext();
        }
    }
}
