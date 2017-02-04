using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyManager : Photon.PunBehaviour
{

    #region public variables

    public string playerName;
    public GameObject playerPanel;
    public GameObject canvas;
    public RectTransform individualPlayerPanel;
    public float countdown;
    public Text countdownText;
    public int playersForGameToBegin = 4;

    #endregion

    #region private variables

    GameObject panel;
    Teams teams;
    public string team; 
    public bool _enoughPlayers = false;

    Color red = new Color(0.447f, 0.255f, 0.18f, 1.0f);
    Color blue = new Color(0.18f, 0.255f, 0.447f, 1.0f);

    #endregion

    #region MonoBehaviour Methods

    void Start()
    {
        PhotonNetwork.sendRate = 10;
        PhotonNetwork.sendRateOnSerialize = 5;

        countdownText = Instantiate(countdownText) as Text;
        countdownText.transform.SetParent(canvas.transform, false);
        playerName = PhotonNetwork.player.NickName;

        teams = GameObject.Find("GameManager").GetComponent<Teams>();
        team = teams.GetTeam(playerName);

        if (!PhotonNetwork.isMasterClient)
        {
            PlayerCountChangeOnStartPause();
        }
        else
        {
            PlayerCountChange();
        }

    }

    void Update()
    {
        float currentTime = Mathf.Floor(countdown);
        // if enoughPlayers = True, countdown begins.
        // when countdown <= 0, Game begins.
        if (_enoughPlayers)
        {
            if (PhotonNetwork.isMasterClient)
            {
                countdown -= Time.deltaTime;
            }

            if(currentTime < 0)
            {
                currentTime = 0;
            }
            countdownText.text = "Game will begin in " + currentTime.ToString();

            if (countdown <= 0.0f)
            {
                BeginGame();
            }
            
        }
        
    }

    #endregion


    #region Photon Messages

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.isWriting == true)
        {
            stream.SendNext(countdown);
        }
        else
        {
            countdown = (float)stream.ReceiveNext();
        }

    }

    // Called when a player connects. 
    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerConnected(): " + other.NickName);

        //Test Method
        PlayerCountChangeOnStartPause();
        
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
            Debug.Log("PhotonNetwork: Trying to load level, but not master client.");
        }
        Debug.Log("PhotonNetwork: Loading Level: Lobby");
        //levels can be loaded by name(string), or build number (int)
        PhotonNetwork.LoadLevel("Lobby");
    }

    /// <summary>
    /// Called every time the number of players changes. It destroys the player UI and rebuilds it
    /// </summary>
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
            RectTransform indvPlayerPanel = Instantiate(individualPlayerPanel);
            indvPlayerPanel.transform.SetParent(panel.transform, false);
            indvPlayerPanel.GetChild(0).GetComponent<Text>().text = player.name;
            indvPlayerPanel.localPosition += Vector3.up * offset * -1;
            //each new text is 30 pixels below previous text. 
            offset += 60;

            team = teams.GetTeam(player.NickName);
            Debug.Log(player.NickName + " is on team: " + team);
            if (team == "red")
            {
                indvPlayerPanel.GetComponent<Image>().color = red;
            }
            else if (team == "blue")
            {
                indvPlayerPanel.GetComponent<Image>().color = blue;
            }
        }

        if (PhotonNetwork.room.PlayerCount >= playersForGameToBegin && !_enoughPlayers)
        {
            Debug.Log("Lobby: Calling Countdown()");
            Countdown();
        }
    }

    void PlayerCountChangeOnStartPause()
    {
        StartCoroutine(PlayerCountChangeIEnumerator(1));
    }

    void Countdown()
    {
        _enoughPlayers = !_enoughPlayers;
        countdown = 10.0f;
    }

    void BeginGame()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            Debug.LogError("PhotonNetwork: Trying to load level, but not master client.");
        }
        Debug.Log("PhotonNetwork: Loading Level: Level_AsteroidField");
        //levels can be loaded by name(string), or build number (int)
        PhotonNetwork.room.IsOpen = false;
        Object.DontDestroyOnLoad(GameObject.Find("GameManager"));
        PhotonNetwork.LoadLevel("Level_AsteroidField");
    }

    #endregion

    IEnumerator PlayerCountChangeIEnumerator(int seconds)
    {
        Debug.Log("Before Waiting 2 seconds");
        yield return new WaitForSeconds(seconds);
        PlayerCountChange();
        Debug.Log("After Waiting 2 Seconds");
    }
}
