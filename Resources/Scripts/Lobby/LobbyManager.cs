using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyManager : Photon.PunBehaviour
{

    #region public variables

    public GameObject playerPanel;
    public GameObject canvas;
    public RectTransform individualPlayerPanel;
    public Text countdownText;
    public int playersForGameToBegin = 4;

    #endregion

    #region private variables

    GameObject panel;
    public bool _enoughPlayers = false;
    public float _countdown;

    #endregion

    #region MonoBehaviour Methods

    void Start()
    {
        PlayerCountChange();
        countdownText = Instantiate(countdownText) as Text;
        countdownText.transform.SetParent(canvas.transform, false);
    }

    void Update()
    {
        // if enoughPlayers = True, countdown begins.
        // when countdown <= 0, Game begins.
        if (_enoughPlayers)
        {
            _countdown -= Time.deltaTime;
            if(_countdown <= 0.0f)
            {
                BeginGame();
            }
            countdownText.text = "Game will begin in " + Mathf.Floor(_countdown).ToString();
        }

    }

    #endregion


    #region Photon Messages

    // Called when a player connects. 
    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerConnected(): " + other.NickName);

        /*
         * This is how levels will be loaded. 
        if(PhotonNetwork.isMasterClient){
            LoadLevel();
        }
         *
         */

        //Test Method
        PlayerCountChange();
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerDisconnected(): " + other.NickName);

        //Test Method
        PlayerCountChange();
    }

    #endregion

    #region private methods

    /// <summary>
    /// This is how a level will be loaded in the game, I made "Lobby" a seperate level to learn how to do this
    /// </summary>
    void LoadLobby()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            Debug.LogError("PhotonNetwork: Trying to load level, but not master client.");
        }
        Debug.Log("PhotonNetwork: Loading Level: Lobby");
        //levels can be loaded by name(string), or build number (int)
        PhotonNetwork.LoadLevel("Lobby");
    }

    void PlayerCountChange()
    {
        //Destroys panel, rebuilds it
        Destroy(panel);
        panel = Instantiate(playerPanel) as GameObject;
        panel.transform.SetParent(canvas.transform, false);

        float offset = 0;
        //For each player creates text with player's name
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            RectTransform text = Instantiate(individualPlayerPanel);
            text.transform.SetParent(panel.transform, false);
            text.GetChild(0).GetComponent<Text>().text = player.name;
            text.localPosition += Vector3.up * offset * -1;
            //each new text is 30 pixels below previous text. 
            offset += 60;
        }

        if (PhotonNetwork.room.PlayerCount >= playersForGameToBegin && !_enoughPlayers)
        {
            Debug.Log("Lobby: Calling Countdown()");
            Countdown();
        }
    }

    void Countdown()
    {
        _enoughPlayers = !_enoughPlayers;
        _countdown = 10.0f;
    }

    void BeginGame()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            Debug.LogError("PhotonNetwork: Trying to load level, but not master client.");
        }
        Debug.Log("PhotonNetwork: Loading Level: Level_AsteroidField");
        //levels can be loaded by name(string), or build number (int)
        PhotonNetwork.LoadLevel("Level_AsteroidField");
    }

    #endregion
}
