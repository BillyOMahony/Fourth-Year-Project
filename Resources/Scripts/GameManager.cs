using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Photon.PunBehaviour {

    #region public variables
    //Any other script can now call GameManager.Instance.method()
    static public GameManager Instance;
    public GameObject playerInstance;
    public bool ready = false;

    LobbyManager _lm;

    #endregion

    #region private variables

    public Teams teams;
    public string team = "NA";

    #endregion

    void Start()
    {
        _lm = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
        teams = gameObject.GetComponent<Teams>();
        PhotonNetwork.Instantiate(playerInstance.name, playerInstance.transform.position, playerInstance.transform.rotation, 0);
    }


    #region public methods

    public void LeaveRoom()
    {
        Debug.Log("GameManager: LeaveRoom() called");
        PhotonNetwork.LeaveRoom();
        Destroy(GameObject.Find("CursorStates"));
        Destroy(GameObject.Find("PlayerIcon"));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public string GetMyTeam()
    {
        return team;
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        if (PhotonNetwork.isMasterClient)
        {
            teams.RemovePlayer(other.NickName);
        }
    }
    
    #endregion

    #region Photon Messages

    /// <summary>
    /// Called when the local player leaves the room. Loads the launcher scene.
    /// </summary>
    public void OnLeftRoom()
    {
        Debug.Log("GameManager: Loading Scene 0 (Main Menu)");
        SceneManager.LoadScene(0);
    }

    #endregion

    public void CallJoinRedTeam()
    {
        GetComponent<PhotonView>().RPC("JoinRedTeam", PhotonTargets.All, PhotonNetwork.player.NickName);
    }

    public void CallJoinBlueTeam()
    {
        GetComponent<PhotonView>().RPC("JoinBlueTeam", PhotonTargets.All, PhotonNetwork.player.NickName);
    }

    [PunRPC]
    public void JoinRedTeam(string name)
    {
        if (PhotonNetwork.isMasterClient)
        {
            teams.JoinTeam(name, "red");
        }
    }
    

    [PunRPC]
    public void JoinBlueTeam(string name)
    {
        if (PhotonNetwork.isMasterClient)
        {
            teams.JoinTeam(name, "blue");
        }
    }

    void OnPhotonSerializeView()
    {
        //leave empty, must be declared for PunRPCs to work
    }

}