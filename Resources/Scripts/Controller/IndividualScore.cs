using UnityEngine;
using System.Collections;

public class IndividualScore : Photon.PunBehaviour {

    public int kills = 0;
    public int deaths = 0;

    // Use this for initialization
    void Start () {

	}
	
    [PunRPC]
    public void UpdateKills(int scoreAdd)
    {
        kills += scoreAdd;
    }

    public void CallUpdateKills(int i, int scoreAdd)
    {
        GetComponent<PhotonView>().RPC("UpdateKills", PhotonNetwork.playerList[i], scoreAdd);
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
