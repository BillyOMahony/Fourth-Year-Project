using UnityEngine;
using System.Collections;

/// <summary>
/// This class keeps track of each players individual kills and deaths. 
/// It also helps generate the scoreboard.
/// </summary>
[System.Serializable]
public class ScoreManager : MonoBehaviour {

    public int kills = 0;
    public int deaths = 0;
    public string owner;

    IndividualScore _is;

    void Start()
    {
        PhotonNetwork.sendRate = 20;
        PhotonNetwork.sendRateOnSerialize = 10;
        owner = GetComponent<PhotonView>().owner.NickName;
        _is = GameObject.Find("IndividualScore").GetComponent<IndividualScore>();
    }

    void Update()
    {
        if (GetComponent<PhotonView>().isMine)
        {
            kills = _is.kills;
            deaths = _is.deaths;
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting == true)
        {
            stream.SendNext(kills);
            stream.SendNext(deaths);
        }
        else
        {
            kills = (int)stream.ReceiveNext();
            deaths = (int)stream.ReceiveNext();
        }
    }
}