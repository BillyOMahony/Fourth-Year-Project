using UnityEngine;
using System.Collections;

public class Launcher : Photon.PunBehaviour {

    #region public variables
    /// <summary>
    /// PUN Log level
    /// </summary>
    public PhotonLogLevel LogLevel = PhotonLogLevel.Informational;

    /// <summary>
    /// The Maximum number of players per room. When a room is full, it can't be joined by more players
    /// </summary>
    [Tooltip("The Maximum number of players per room. When a room is full, it can't be joined by more players")]
    public byte MaxPlayersPerRoom = 8;

    [Tooltip("The UI Panel to let the user enter name, connect and play")]
    public GameObject controlPanel;

    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    public GameObject progressLabel;

    #endregion

    #region private variables
    /// <summary>
    /// This client's version number. Users are seperated from each other by gameversion
    /// </summary>
    string _gameVersion = "1";

    /// <summary>
    /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon, 
    /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
    /// Typically this is used for the OnConnectedToMaster() callback.
    /// </summary>
    bool isConnecting;

    #endregion

    #region MonoBehaviour Methods

    void Awake()
    {

        PhotonNetwork.logLevel = LogLevel;

        // We do not need to join a lobby to get a list of rooms.
        PhotonNetwork.autoJoinLobby = false;

        // #Critical
        // This makes sure we can use PhotonNetwork.LoadLevel() on the master client and all other clients in the same room sync their level automatically.
        PhotonNetwork.automaticallySyncScene = true;
    }
	
    void Start()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);  
    }

    #endregion

    #region Public Methods

    public void Connect()
    {
        isConnecting = true;

        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

        // check if we are connected or not, join a room if we are, else we initiate the connection to the server.
        if (PhotonNetwork.connected)
        {
            // #Critical we need at this point to attempt joining a random room, if it fails, we'll get notified in OnPhotonRandomJoinFailed() and we create a new room
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // #Critical we must first and foremost connect to Photon Online Server
            PhotonNetwork.ConnectUsingSettings(_gameVersion);
        }
    }

    #endregion

    #region Photon.PunBehaviour Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("Launcher: OnConnectedToMaster() was called by PUN");

        //We don't want to do anything if not trying to connect to a room.
        if (isConnecting)
        {
            // #Critical the first thing we try to do is to join a prexisting room. If there isn't one we'll be called back with OnPhotonRandomJoinFailed()
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnectedFromPhoton()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        Debug.Log("Launcher: OnDisconnectedFromPhoton() was called by PUN");
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("Launcher: OnPhotonRandomJoinFailed() was called by PUN. No random room available.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions(){maxPlayers = MaxPlayersPerRoom, null};");

        // #Critical joining random room failed, assume they're all full, create new room
        PhotonNetwork.CreateRoom(null, new RoomOptions() { maxPlayers = MaxPlayersPerRoom }, null);

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Launcher: OnJoinedRoom() called by PUN. This client is in a room.");

        //#Critical we only load if we are the first player, else we rely on PhotonNetwork.automaticallySyncScene to sync out instance scene.
        if(PhotonNetwork.room.PlayerCount == 1)
        {
            Debug.Log("We load the 'Lobby'");

            //#Critical
            //Load the lobby
            PhotonNetwork.LoadLevel("Lobby");
        }
    }
    
    #endregion


}
