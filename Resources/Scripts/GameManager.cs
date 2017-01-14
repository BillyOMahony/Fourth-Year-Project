using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : Photon.PunBehaviour {

    #region public variables

    static public GameManager Instance;
    //Any other script can now call GameManager.Instance.method()

    
    #endregion



    #region public methods

    public void LeaveRoom()
    {
        Debug.Log("GameManager: LeaveRoom() called");
        PhotonNetwork.LeaveRoom();    
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