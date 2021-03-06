﻿using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class PlayerManager : Photon.PunBehaviour {

    #region public variables
    [Tooltip("The current Health of our player")]
    public float OriginalHealth = 100f;
    public float Health;
    public GameObject DeathCamera;
    public string team;
    public float outOfBoundsTimer = 10.0f;
    #endregion

    #region private variables

    string owner;

    MatchManager matchManager;
    GameObject _gameManager;
    Teams _teams;

    bool dead = false;

    PlayerControllerRB controller;

    PhotonView _pv;

    PlayerUIManager _puim;
    ScoreManager _sm;
    IndividualScore _is;

    float timer = 5f;

    bool outOfBounds = false;

    ExplosionScript _es;
    #endregion

    // Use this for initialization
    void Start () {

        PhotonNetwork.sendRate = 20;
        PhotonNetwork.sendRateOnSerialize = 10;

        _pv = GetComponent<PhotonView>();
        _sm = GetComponent<ScoreManager>();
        _puim = GetComponent<PlayerUIManager>();
        _es = GetComponent<ExplosionScript>();

        Health = OriginalHealth;

        controller = gameObject.GetComponent<PlayerControllerRB>();
        owner = _pv.owner.NickName;
        matchManager = GameObject.Find("MatchManager").GetComponent<MatchManager>();
        _is = GameObject.Find("IndividualScore").GetComponent<IndividualScore>();

        _gameManager = GameObject.Find("GameManager");
        _teams = _gameManager.GetComponent<Teams>();

        Debug.LogWarning("Calling Teams.GetTeam(|" + owner + "|)");
        team = _gameManager.GetComponent<Teams>().GetTeam(owner);

        if (matchManager == null)
        {
            Debug.Log("<Color=Red>Cannot Find</Color> MatchManager GameObject");
        }

        foreach (KeyValuePair<string, string> player in _teams.teams)
        {
            Debug.LogWarning("|" + player.Key + "|");
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (dead)
        {
            Respawn();
        }

        if (outOfBounds)
        {
            outOfBoundsTimer -= Time.deltaTime;
            if(outOfBoundsTimer < 0)
            {
                GetComponent<PhotonView>().RPC("DeActivate", PhotonTargets.All);
                DisableClient();
                KillClient();
                outOfBounds = false;
                outOfBoundsTimer = 10.0f;
                _puim.BackInBounds();
            }
        }
	}

    public void DamageTaken(float damage, string hitBy)
    {
        if (PhotonNetwork.isMasterClient)
        {
            Health -= damage;

            GetComponent<PhotonView>().RPC("ReceiveDamage", PhotonTargets.Others, damage);

            Debug.Log("Damage Taken, calling _PUIM.UpdateHealthBar()");

            if (Health <= 0)
            {
                Debug.LogWarning("<color=red>Calling Teams.GetTeam("+ hitBy +")</color>");
                string hitByTeam = _teams.GetTeam(hitBy);
                Debug.LogWarning("Returned: " + hitByTeam);
                int scoreAdd = 1;
                if(hitByTeam == team)
                {
                    scoreAdd = -1;
                }
                Debug.Log("Player from " + hitByTeam + " killed player from " + team);


                GetComponent<PhotonView>().RPC("DeActivate", PhotonTargets.All);
                int i = 0;

                foreach (PhotonPlayer player in PhotonNetwork.playerList)
                {
                    if (player.NickName == owner)
                    {
                        break;
                    }
                    i++;
                }

                _is.CallUpdateDeaths(i);

                GetComponent<PhotonView>().RPC("DisableClient", PhotonNetwork.playerList[i]);
                GetComponent<PhotonView>().RPC("KillClient", PhotonNetwork.playerList[i]);

                i = 0;

                foreach (PhotonPlayer player in PhotonNetwork.playerList)
                {
                    if (player.NickName == hitBy)
                    {
                        break;
                    }
                    i++;
                }
                //Updates the kill count for the player who killed this player
                _is.CallUpdateKills(i, scoreAdd);

                Debug.LogWarning("Calling MatchManager.UpdateScore(|" + hitByTeam + "|,|" + scoreAdd + "|");
                matchManager.UpdateScore(hitByTeam, scoreAdd);
            }
        }
    }

    [PunRPC]
    void DeActivate()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        _es.PlayExplosion();
    }

    [PunRPC]
    void Activate()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(true);
        // Health is set here because currently when Activate is called, it's used for respawning.
        Health = 100f;
    }

    [PunRPC]
    void DisableClient()
    {
        controller.Disabled();
    }

    [PunRPC]
    void KillClient()
    {
        dead = true;
        controller.Killed();
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

    public void OutOfBounds()
    {
        outOfBounds = true;
        _puim.OutOfBounds();
    }

    public void BackInBounds()
    {
        outOfBounds = false;
        outOfBoundsTimer = 10.0f;
        _puim.BackInBounds();
    }


}