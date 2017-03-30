using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinTeam : MonoBehaviour {


    public void SelectRedTeam()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().CallJoinRedTeam();

        GameObject.Find("GameManager").GetComponent<GameManager>().team = "red";
        GameObject.Find("GameManager").GetComponent<GameManager>().ready = true;
        GameObject.Find("LobbyManager").GetComponent<LobbyManager>().UpdateGUI();
    }

    public void SelectBlueTeam()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().CallJoinBlueTeam();

        GameObject.Find("GameManager").GetComponent<GameManager>().team = "blue";
        GameObject.Find("GameManager").GetComponent<GameManager>().ready = true;
        GameObject.Find("LobbyManager").GetComponent<LobbyManager>().UpdateGUI();
    }

    // Use this for initialization

}
