using UnityEngine;
using System.Collections;

public class PlayerManager : Photon.PunBehaviour {

    #region public variables
    [Tooltip("The current Health of our player")]
    public float Health = 100f;
    public GameObject DeathCamera;

    #endregion

    #region private variables

    string owner;

    public MatchManager matchManager;
    bool dead = false;

    Movement movement;
    float timer = 5f;

    #endregion

    // Use this for initialization
    void Start () {

        movement = gameObject.GetComponent<Movement>();
        owner = GetComponent<PhotonView>().owner.NickName;
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
        movement.Disabled();
        dead = true;
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
        movement.Enabled();
    }

}