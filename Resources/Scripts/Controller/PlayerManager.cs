using UnityEngine;
using System.Collections;

public class PlayerManager : Photon.PunBehaviour {

    #region public variables
    [Tooltip("The current Health of our player")]
    public float OriginalHealth = 100f;
    public float Health;
    public GameObject DeathCamera;

    #endregion

    #region private variables

    string owner;

    MatchManager matchManager;
    bool dead = false;

    PlayerController controller;

    PhotonView _pv;

    float timer = 5f;

    #endregion

    // Use this for initialization
    void Start () {

        _pv = GetComponent<PhotonView>();

        Health = OriginalHealth;

        controller = gameObject.GetComponent<PlayerController>();
        owner = _pv.owner.NickName;
        matchManager = GameObject.Find("MatchManager").GetComponent<MatchManager>();
        if(matchManager == null)
        {
            Debug.Log("<Color=Red>Cannot Find</Color> MatchManager GameObject");
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (dead)
        {
            Respawn();
        }
	}

    public void DamageTaken(float damage)
    {
        if (PhotonNetwork.isMasterClient)
        {
            Health -= damage;

            GetComponent<PhotonView>().RPC("ReceiveDamage", PhotonTargets.Others, damage);

            Debug.Log("Damage Taken, calling _PUIM.UpdateHealthBar()");

            if (Health <= 0)
            {
                GetComponent<PhotonView>().RPC("DeActivate", PhotonTargets.All);
                int i = 0;

                // Fix this shit, there's probably a way to get object owner ID aside from this
                // needs to be fixed because two players can have the same name.
                foreach (PhotonPlayer player in PhotonNetwork.playerList)
                {
                    if (player.NickName == owner)
                    {
                        break;
                    }
                    i++;
                }

                GetComponent<PhotonView>().RPC("DisableClient", PhotonNetwork.playerList[i]);
            }
        }
    }

    [PunRPC]
    void DeActivate()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    [PunRPC]
    void Activate()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        // Health is set here because currently when Activate is called, it's used for respawning.
        Health = 100f;
    }

    [PunRPC]
    void DisableClient()
    {
        controller.Disabled();
        dead = true;
    }

    [PunRPC]
    void ReceiveDamage(float dmg)
    {
        Health -= dmg;
    }

    void Respawn()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            SpawnPlayer();
            dead = false;
            timer = 5;
        }
    }

    void SpawnPlayer()
    {
        GameObject spawnPoint = matchManager.SelectSpawner();
        transform.position = spawnPoint.transform.position;
        transform.rotation = spawnPoint.transform.rotation;
        GetComponent<PhotonView>().RPC("Activate", PhotonTargets.All);
        controller.Enabled();
    }

}