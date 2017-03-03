using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public int initialSpeed = 4000;
    public int damage = 10;
    public string owner;

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
                DamageInfo damageInfo = new DamageInfo(owner, damage);

                col.transform.SendMessage("Damage", damageInfo, SendMessageOptions.DontRequireReceiver);
                Debug.Log("Bullet from " + owner + " has hit " + col.transform.gameObject.GetComponent<PhotonView>().owner.NickName);

                if (GetComponent<PhotonView>().isMine)
                {
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

    [PunRPC]
    void SetOwner(string ownr)
    {
        owner = ownr;
    }

}


public class DamageInfo : MonoBehaviour
{
    string _hitBy;
    float _damage;

    public DamageInfo(string hitBy, float damage)
    {
        _hitBy = hitBy;
        _damage = damage;
    }

    public string getHitBy()
    {
        return _hitBy;
    }

    public float getDamage()
    {
        return _damage;
    }

}