﻿using UnityEngine;
using System.Collections;
using System.Linq;

public class MatchManager : Photon.PunBehaviour {

    #region public variables

    static public MatchManager Instance;
    
    public GameObject playerPrefab;
    public GameObject[] spawners = new GameObject[2];

    //Temp
    public GameObject spawnPoint;

    #endregion

    #region private variables

    int _spawnCounter = 0;

    #endregion

    #region MonoBehaviour Methods

    void Start()
    {
        if (playerPrefab == null)
        {
            Debug.Log("<Color=Red><a>Missing</a></Color> playerPrefab reference. Set it Up!");
        }
        else if (spawners.Any(n=>n == null))
        {
            Debug.Log("<Color=Red><a>Missing</a></Color> spawners reference contains one or more nulls. Fix This!");
        }
        else
        {
            Spawn();
        }
    }

    #endregion

    // Update is called once per frame
    void Update() {

    }

    void Spawn()
    {
        GameObject spawnPoint = SelectSpawner();
        Debug.Log("MatchManager: Spawn() called");

        PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPoint.transform.position, spawnPoint.transform.rotation, 0);
    }

    /// <summary>
    /// Selects the Spawn the player will appear at. 
    /// </summary>
    /// <returns>GameObject spawnPoint: The spawner the player appears at</returns>
    public GameObject SelectSpawner()
    {
        GameObject spawnPoint;
        int random = Random.Range(0, spawners.Length);

        spawnPoint = spawners[random];
        
        if (!spawnPoint.GetComponent<SpawnCollisionDetection>().CanSpawn)
        {
            return SelectSpawner();
        }
        return spawnPoint;
    }
}