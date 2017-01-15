using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Teams : Photon.PunBehaviour{

    public Text text;

    public Dictionary<string, string> teams = new Dictionary<string, string>();
    public int redCount = 0;
    public int blueCount = 0;

    string _red = "red";
    string _blue = "blue";


    void Start()
    {
        PhotonNetwork.sendRate = 10;
        PhotonNetwork.sendRateOnSerialize = 5;

        if(redCount == 0 && blueCount == 0)
        {
            AddPlayer(PhotonNetwork.playerList[0]);
        }
    }

    void Update()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            RedCount();
            BlueCount();
        }
    }

    public void AddPlayer(PhotonPlayer other)
    {
        if(redCount <= blueCount)
        {
            Debug.Log("Adding " + other.NickName + " to Red team");
            teams.Add(other.NickName, _red);
        }
        else
        {
            Debug.Log("Adding " + other.NickName + " to Blue team");
            teams.Add(other.NickName, _blue);
        }

        RedCount();
        BlueCount();
    }

    public void RemovePlayer(PhotonPlayer other)
    {
        teams.Remove(other.NickName);

        RedCount();
        BlueCount();
    }


    void RedCount()
    {
        redCount = 0;
        foreach(KeyValuePair<string, string> player in teams)
        {
            if(player.Value == _red)
            {
                redCount++;
            }
        }
    }

    void BlueCount()
    {
        blueCount = 0;
        foreach (KeyValuePair<string, string> player in teams)
        {
            if (player.Value == _blue)
            {
                blueCount++;
            }
        }
    }

    public string GetTeam(string playerName)
    {
        string team;
        if (teams.TryGetValue(playerName, out team))
        {
            return team;
        }
        else
        {
            return "Error";
            //Debug.LogError("Teams: GetTeam() Could not find player: " + playerName);
            //return "ERROR";
        }
    }


    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.isWriting == true && PhotonNetwork.isMasterClient)
        {
            stream.SendNext(teams);
        }
        else
        {
            Debug.Log("Sending Team Info");
            teams = (Dictionary<string, string>)stream.ReceiveNext();
        }

    }
}
