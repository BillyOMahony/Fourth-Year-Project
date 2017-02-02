using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public int initialSpeed = 4000;
    public string owner;

    float timer = 10;

    void Start()
    {
        gameObject.transform.Rotate(Vector3.forward);
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * initialSpeed);
        owner = GetComponent<PhotonView>().owner.NickName;
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
            if (col.transform.parent != null && col.transform.parent.gameObject.GetComponent<PhotonView>() as PhotonView != null)
            {
                if (col.transform.parent.gameObject.GetComponent<PhotonView>().owner.NickName != owner)
                {
                    col.transform.SendMessage("Damage", 10, SendMessageOptions.DontRequireReceiver);
                    Debug.Log("Bullet from " + owner + " has hit " + col.transform.parent.gameObject.GetComponent<PhotonView>().owner.NickName);
                    PhotonNetwork.Destroy(gameObject);     
                }
            }
            
        }
    }

    void Timer()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
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
