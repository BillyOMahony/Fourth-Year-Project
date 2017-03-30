using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class MatchManager : Photon.PunBehaviour {

    #region public variables

    static public MatchManager Instance;
    
    public GameObject redPrefab;
    public GameObject bluePrefab;

    public GameObject[] spawners = new GameObject[2];

    public GameObject spawnPoint;

    public GameObject GameManager;

    public int RedScore = 0;
    public int BlueScore = 0;

    public int scoreToWin = 5;

    public string team;

    #endregion

    #region private variables

    int _spawnCounter = 0;
    GameObject _redTeamText;
    GameObject _blueTeamText;

    #endregion

    #region MonoBehaviour Methods

    void Start()
    {

        GameManager = GameObject.Find("GameManager");

        Cursor.visible = false;

        team = GameManager.GetComponent<GameManager>().GetMyTeam();

        if (redPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> redPrefab reference. Set it Up!");
        }
        else if(bluePrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> bluePrefab reference. Set it Up!");
        }
        else if (spawners.Any(n=>n == null))
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> spawners reference contains one or more nulls. Fix This!");
        }

        Spawn();

    }

    #endregion

    // Update is called once per frame
    void Update() {

    }

    void Spawn()
    {
        GameObject spawnPoint = SelectSpawner();
        Debug.LogWarning("MatchManager: Spawn() called");

        if (team == "red")
        {
            PhotonNetwork.Instantiate(redPrefab.name, spawnPoint.transform.position, spawnPoint.transform.rotation, 0);
        }
        else if(team == "blue")
        {
            PhotonNetwork.Instantiate(bluePrefab.name, spawnPoint.transform.position, spawnPoint.transform.rotation, 0);
        }else
        {
            Debug.LogError("MatchManager: Spawn() could not spawn player");
        }
        _redTeamText = GameObject.Find("RedTeamScore Text");
        _blueTeamText = GameObject.Find("BlueTeamScore Text");

        _redTeamText.GetComponent<Text>().text = "Red Team " + RedScore;
        _blueTeamText.GetComponent<Text>().text = "BlueTeam " + BlueScore;
    }

    /// <summary>
    /// Selects the Spawn the player will appear at. 
    /// </summary>
    /// <returns>GameObject spawnPoint: The spawner the player appears at</returns>
    public GameObject SelectSpawner()
    {
        GameObject spawnPoint;
        int random = Random.Range(0, spawners.Length);

        spawnPoint = spawners[random];
        
        if (!spawnPoint.GetComponent<SpawnCollisionDetection>().CanSpawn)
        {
            return SelectSpawner();
        }
        return spawnPoint;
    }

    #region RPCs
    
    [PunRPC]
    public void redTeamScored(int score)
    {
        RedScore += score;
        UpdateScoreText();
    }

    [PunRPC]
    public void blueTeamScored(int score)
    {
        BlueScore += score;
        UpdateScoreText();
    }

    [PunRPC]
    public void GameOver()
    {
        //Stuff that happens when the game ends

        PhotonNetwork.Destroy(GameManager);
        SceneManager.LoadScene(0);
        
    }
    
    #endregion

    public void UpdateScore(string team, int addScore)
    {
        if(team == "red")
        {
            GetComponent<PhotonView>().RPC("redTeamScored", PhotonTargets.All, addScore);
        }
        else if(team == "blue")
        {
            GetComponent<PhotonView>().RPC("blueTeamScored", PhotonTargets.All, addScore);
        }

        if(BlueScore >= scoreToWin || RedScore >= scoreToWin)
        {
            GetComponent<PhotonView>().RPC("GameOver", PhotonTargets.All);
        }

        //Stuff here to update GUI and such
    }

    void UpdateScoreText()
    {
        _redTeamText.GetComponent<Text>().text = "Red Team " + RedScore;
        _blueTeamText.GetComponent<Text>().text = "BlueTeam " + BlueScore;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { }
}