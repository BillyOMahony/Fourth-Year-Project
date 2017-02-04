using UnityEngine;
using System.Collections;

public class MovementRigidBody : Photon.PunBehaviour
{

    #region public variables

    public float maxSpeed = 10.0f;
    public float acceleration = 0f;

    public float turnSpeed = 10f;
    public float turn;
    //public Vector3 turn;

    public float engine = 0f;
    public float engineAcceleration = 0.2f;

    public bool disabled = false;

    #endregion

    #region private variables

    GameObject _spaceship;
    Rigidbody _body;

    #endregion

    #region MonoBehaviour Methods

    // Use this for initialization
    void Start()
    {
        PhotonNetwork.sendRate = 40;
        PhotonNetwork.sendRateOnSerialize = 20;

        _spaceship = gameObject;
        _body = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (disabled)
        {
            return;
        }

        if (photonView.isMine)
        {
            GetThrottle();
            GetTurn();

            ApplyThrottle();
            ApplyTurn();
        }
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

    void GetThrottle()
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

        float xVel = transform.InverseTransformDirection(_body.velocity).x;
        float yVel = transform.InverseTransformDirection(_body.velocity).y;
        float zVel = transform.InverseTransformDirection(_body.velocity).z;
        
        if(xVel < maxSpeed * engine)
        {
            acceleration = engine * maxSpeed * Time.deltaTime;
        }
        else if(xVel > maxSpeed)
        {
            float newVel = xVel - maxSpeed;
            acceleration = newVel * -1;
        }

        //acceleration = maxSpeed * engine;
    }

    void ApplyThrottle()
    {
        _body.AddForce(_spaceship.transform.forward * acceleration);
        //_spaceship.transform.position += transform.forward * speed * Time.deltaTime;
    }

    void GetTurn()
    {
        turn = turnSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        //turn = Vector3.up * Time.deltaTime * Input.GetAxis("Horizontal") * turnSpeed;//Vector3.up = right?!?! These things sort themselves out...
    }

    void ApplyTurn()
    {
        _body.AddTorque(gameObject.transform.up * turn);
        //_spaceship.transform.Rotate(turn);
    }

    #endregion

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting == true)
        {
            //stream.SendNext(speed);
            //stream.SendNext(turn);
            stream.SendNext(_spaceship.transform.position);
            stream.SendNext(_spaceship.transform.rotation);
        }
        else
        {
            _spaceship.transform.position = (Vector3)stream.ReceiveNext();
            _spaceship.transform.rotation = (Quaternion)stream.ReceiveNext();
            //speed = (float)stream.ReceiveNext();
            //turn = (Vector3)stream.ReceiveNext();
        }
    }
}