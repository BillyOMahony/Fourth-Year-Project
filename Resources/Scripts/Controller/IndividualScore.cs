using UnityEngine;
using System.Collections;

public class IndividualScore : Photon.PunBehaviour {

    public int kills = 0;
    public int deaths = 0;

    // Use this for initialization
    void Start () {

	}
	
    [PunRPC]
    public void UpdateKills()
    {
        kills++;
    }

    public void CallUpdateKills(int i)
    {
        GetComponent<PhotonView>().RPC("UpdateKills", PhotonNetwork.playerList[i]);
    }

    [PunRPC]
    public void UpdateDeaths()
    {
        deaths++;
    }

    public void CallUpdateDeaths(int i)
    {
        GetComponent<PhotonView>().RPC("UpdateDeaths", PhotonNetwork.playerList[i]);
    }
}
