using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public int initialSpeed = 4000;

    float timer = 10;

    void Start()
    {
        gameObject.transform.Rotate(Vector3.forward);
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * initialSpeed);
    }

    void Update()
    {
        Timer();
    }

	// Use this for initialization
	void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "SpawnPoint")
        {
            if (col.transform.gameObject.GetComponent<PhotonView>() != null && col.transform.gameObject.GetComponent<PhotonView>().owner.NickName != GetComponent<PhotonView>().owner.NickName)
            {
                col.transform.SendMessage("Damage", 10, SendMessageOptions.DontRequireReceiver);
                Debug.Log("Bullet from " + GetComponent<PhotonView>().owner.NickName + " has hit " + col.transform.gameObject.GetComponent<PhotonView>().owner.NickName);
                PhotonNetwork.Destroy(gameObject);
            }
            
        }
    }

    void Timer()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            if (GetComponent<PhotonView>().isMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting == true)
        {
          //  stream.SendNext(speed);
          //  stream.SendNext(turn);
        }
        else
        {
          //  speed = (float)stream.ReceiveNext();
          //  turn = (Vector3)stream.ReceiveNext();
        }
    }
}
