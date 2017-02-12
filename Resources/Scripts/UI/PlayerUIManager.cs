using UnityEngine;
using System.Collections;

public class PlayerUIManager : Photon.PunBehaviour {

    public RectTransform HealthBar;

    public bool mine;

    //public PlayerController _PC;
    public PlayerControllerRB _PC;
    public PlayerManager _PM;
    public PhotonView _PV;

    // Use this for initialization
    void Start () {

        _PV = GetComponent<PhotonView>();
        _PM = GetComponent<PlayerManager>();
        _PC = GetComponent<PlayerControllerRB>();

        mine = _PC.GetComponent<PhotonView>().isMine;

        if (mine)
        {
            HealthBar = Instantiate(HealthBar);
            HealthBar.transform.SetParent(GameObject.Find("GameUI").transform, false);
            HealthBar = HealthBar.transform.GetChild(0).GetComponent<RectTransform>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        //Debug.Log("UpdateHealthBar() called");
        HealthBar.sizeDelta = new Vector2(_PM.Health * 3, 40);
    }
}
