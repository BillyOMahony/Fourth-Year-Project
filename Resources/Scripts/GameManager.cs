using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Photon.PunBehaviour {

    #region public variables
    //Any other script can now call GameManager.Instance.method()
    static public GameManager Instance;

    #endregion

    #region private variables

    Teams teams;

    #endregion

    void Start()
    {
        teams = gameObject.GetComponent<Teams>();
    }


    #region public methods

    public void LeaveRoom()
    {
        Debug.Log("GameManager: LeaveRoom() called");
        PhotonNetwork.LeaveRoom();    
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        if (PhotonNetwork.isMasterClient)
        {
            teams.AddPlayer(other);
        }
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        if (PhotonNetwork.isMasterClient)
        {
            teams.RemovePlayer(other);
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
}