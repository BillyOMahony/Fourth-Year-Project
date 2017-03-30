using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLobbyManager : MonoBehaviour {

    public string Name;
    public int icon = 0;
    public string team = "NA";
    public bool ready = false;

    PhotonView _pv;
    GameManager _gm;
    LobbyManager _lm;

	// Use this for initialization
	void Start () {
        _pv = GetComponent<PhotonView>();
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        _lm = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();

        transform.SetParent(GameObject.Find("PlayerInstance Container").transform);
        Name = _pv.owner.NickName;
        if (_pv.isMine)
        {
            //icon = PlayerPrefs.GetInt("PlayerIcon");
        }

        _lm.UpdateGUI();
	}

    void Update()
    {
        if (_pv.isMine)
        {
            team = _gm.team;
            ready = _gm.ready;
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting == true && _pv.isMine)
        {
            stream.SendNext(Name);
            stream.SendNext(icon);
            stream.SendNext(team);
            stream.SendNext(ready);
        }
        else
        {
            Name = (string)stream.ReceiveNext();
            icon = (int)stream.ReceiveNext();
            team = (string)stream.ReceiveNext();
            ready = (bool)stream.ReceiveNext();
        }
    }
}
