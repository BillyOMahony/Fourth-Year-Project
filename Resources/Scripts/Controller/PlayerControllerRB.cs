using UnityEngine;
using System.Collections;

public class PlayerControllerRB : Photon.PunBehaviour
{

    public bool disabled = false;
    public string owner;

    #region public variables movement

    //Network variables
    public float turn;
    public float pitch;
    public float roll;

    //public 

    public float maxSpeed = 10.0f;
    public float maxHorizontalSpeed = 3.0f;
    public float maxVerticalSpeed = 3.0f;

    public float accelerationMultiplier = 10f;

    public float turnSpeed = 10f;
    public float maxTurn = 20f;

    public float engine = 0f;
    public float engineAcceleration = 0.2f;

    public float xVel;
    public float yVel;
    public float zVel;

    public Vector3 pointVelocity;

    public float maxAllowedSpeed;

    #endregion

    #region public variables shoot

    public float RateOfFire = 0.2f;

    public GameObject projectile;
    public GameObject projectileSpawner;

    #endregion

    #region private variables

    float newVel;
    float _acceleration = 0f;
    float _horizontalAcceleration = 0f;
    float _verticalAcceleration = 0f;

    GameObject _spaceship;
    Rigidbody _body;

    GameObject NewProjectile;
    Vector3 SpawnPosition;
    bool CanShoot = true;
    float timer;
    AudioSource audio;

    ScoreManager _SM;

    public float _massMultiplier;

    public Vector3 AngularVelocity;

    #endregion

    #region MonoBehaviour Methods

    // Use this for initialization
    void Start()
    {
        PhotonNetwork.sendRate = 20;
        PhotonNetwork.sendRateOnSerialize = 10;

        _spaceship = gameObject;
        _body = _spaceship.GetComponent<Rigidbody>();

        audio = projectileSpawner.GetComponent<AudioSource>();
        owner = GetComponent<PhotonView>().owner.NickName;

        _massMultiplier = _body.mass;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AngularVelocity = _body.angularVelocity;

        if (disabled)
        {
            return;
        }

        if (photonView.isMine)
        {
            _body.AddTorque(-_body.angularVelocity * _massMultiplier);

            xVel = transform.InverseTransformDirection(_body.velocity).x;
            yVel = transform.InverseTransformDirection(_body.velocity).y;
            zVel = transform.InverseTransformDirection(_body.velocity).z;

            SetThrottle();
            HorizontalMovement();
            VerticalMovement();

            SetTurn();
            Pitch();
            Roll();

            FireProjectile();

            ApplyThrottle();
            ApplyTurn();

            Stabilization();
        }

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

    public void Killed()
    {
        _body.velocity = Vector3.zero;
        _body.angularVelocity = Vector3.zero;
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

        maxAllowedSpeed = maxSpeed * engine;

        if (zVel < maxAllowedSpeed)
        {
            _acceleration = engineAcceleration * maxSpeed * Time.deltaTime * accelerationMultiplier * _massMultiplier;
        }
        else if (zVel > maxAllowedSpeed)
        {
            newVel = zVel - maxAllowedSpeed;
            _acceleration = newVel * -1;
        }
    }

    void HorizontalMovement()
    {

        _horizontalAcceleration = Input.GetAxis("Horizontal") * Time.deltaTime * 50 * _massMultiplier;

        if(xVel > maxHorizontalSpeed)
        {
            newVel = xVel - maxHorizontalSpeed;
            _horizontalAcceleration = newVel * -1;
        }else if(xVel < maxHorizontalSpeed * -1)
        {
            newVel = xVel * -1 - maxHorizontalSpeed;
            _horizontalAcceleration = newVel;
        } 
    }

    void VerticalMovement() {

        _verticalAcceleration = Input.GetAxis("Vertical") * Time.deltaTime * 50 * _massMultiplier;

        if (yVel > maxVerticalSpeed)
        {
            newVel = yVel - maxVerticalSpeed;
            _verticalAcceleration = newVel * -1;
        }
        else if (yVel < maxVerticalSpeed * -1)
        {
            newVel = yVel * -1 - maxVerticalSpeed;
            _verticalAcceleration = newVel;
        }
    }

    void ApplyThrottle()
    {
        _body.AddForce(_spaceship.transform.forward * _acceleration);
        _body.AddForce(_spaceship.transform.up * _verticalAcceleration);
        _body.AddForce(_spaceship.transform.right * _horizontalAcceleration);
    }

    void SetTurn()
    {
        turn = turnSpeed * Time.deltaTime * Input.GetAxis("Yaw") * _massMultiplier * 2;
    }

    void Pitch()
    {
        pitch = turnSpeed * Time.deltaTime * Input.GetAxis("Mouse Y") * -1 * _massMultiplier * 2;
    }

    void Roll()
    {
        roll = turnSpeed * Time.deltaTime * Input.GetAxis("Mouse X") * -1 * _massMultiplier * 2;
    }

    void ApplyTurn()
    {
        _body.AddTorque(gameObject.transform.up * turn);
        _body.AddTorque(gameObject.transform.forward * roll);
        _body.AddTorque(gameObject.transform.right * pitch);
    }



    void FireProjectile()
    {
        if (Input.GetButton("Fire1") && CanShoot)
        {
            //audio.Play();
            NewProjectile = PhotonNetwork.Instantiate("Bullet", projectileSpawner.transform.position, projectileSpawner.transform.rotation, 0) as GameObject; //This line for photon
            NewProjectile.GetComponent<PhotonView>().RPC("SetOwner", PhotonTargets.All, owner);
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

    void Stabilization()
    {
        if (!Input.GetButton("Vertical"))
        {
            _body.AddForce(_spaceship.transform.up * yVel * -1 * _massMultiplier);
        }
        if (!Input.GetButton("Horizontal"))
        {
            _body.AddForce(_spaceship.transform.right * xVel * -1 * _massMultiplier);
        }
    }

    #endregion

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting == true)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}