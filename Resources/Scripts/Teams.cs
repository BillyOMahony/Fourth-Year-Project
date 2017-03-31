using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Teams : Photon.PunBehaviour{

    public string text = "text";

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
            //AddPlayer(PhotonNetwork.playerList[0]);
        }
    }

    public void AddPlayer(string name)
    {
        if(redCount <= blueCount)
        {
            Debug.Log("Adding " + name + " to Red team");
            teams.Add(name, _red);
        }
        else
        {
            Debug.Log("Adding " + name + " to Blue team");
            teams.Add(name, _blue);
        }

    //    RedCount();
    //    BlueCount();
    }

    public void RemovePlayer(string name)
    {
        teams.Remove(name);
    }

    public void JoinTeam(string name, string team)
    {
        bool found = false;
        foreach (KeyValuePair<string,string> player in teams){
            if(player.Key == name)
            {
                Debug.Log("Key found, removing");
                found = true;
            }
        }
        if (found)
        {
            RemovePlayer(name);
        }
        teams.Add(name, team);
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
