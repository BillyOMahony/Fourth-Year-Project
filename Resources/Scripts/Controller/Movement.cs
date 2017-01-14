using UnityEngine;
using System.Collections;

public class Movement : Photon.PunBehaviour {

    #region public variables

    public float maxSpeed = 10.0f;
    public float speed = 0f;

    public float turnSpeed = 10f;
    public Vector3 turn;

    public float engine = 0f;
    public float engineAcceleration = 0.2f;

    public bool disabled = false;

    #endregion

    #region private variables

    GameObject _spaceship;

    #endregion

    #region MonoBehaviour Methods

    // Use this for initialization
    void Start () {
        PhotonNetwork.sendRate = 40;
        PhotonNetwork.sendRateOnSerialize = 20;

        _spaceship = gameObject;
	}
	
	// Update is called once per frame
	void Update () {

        if (disabled)
        {
            return;
        }

        if (photonView.isMine) {
            GetThrottle();
            GetTurn();
        }
        
        ApplyThrottle();
        ApplyTurn();
           
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
        speed = maxSpeed * engine;
    }

    void ApplyThrottle()
    {
        _spaceship.transform.position += transform.forward * speed * Time.deltaTime;
    }

    void GetTurn()
    {
        turn = Vector3.up * Time.deltaTime * Input.GetAxis("Horizontal") * turnSpeed;//Vector3.up = right?!?! These things sort themselves out...
    }

    void ApplyTurn()
    {
        _spaceship.transform.Rotate(turn);
    }

    #endregion

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting == true)
        {
            stream.SendNext(speed);
            stream.SendNext(turn);
        }
        else
        {
            speed = (float)stream.ReceiveNext();
            turn = (Vector3)stream.ReceiveNext();
        }
    }
}