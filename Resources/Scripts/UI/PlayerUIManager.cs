using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUIManager : Photon.PunBehaviour {

    public RectTransform HealthBar;

    public bool mine;

    //public PlayerController _PC;
    public PlayerControllerRB _PC;
    public PlayerManager _PM;
    public PhotonView _PV;

    public Color color;

    GameObject TeamScorePanel;

    // Use this for initialization
    void Start () {

        _PV = GetComponent<PhotonView>();
        _PM = GetComponent<PlayerManager>();
        _PC = GetComponent<PlayerControllerRB>();

        mine = _PC.GetComponent<PhotonView>().isMine;

        TeamScorePanel = GameObject.Find("TeamScores Panel");

        if (mine)
        {
            HealthBar = Instantiate(HealthBar);
            HealthBar.transform.SetParent(GameObject.Find("GameUI").transform, false);
            HealthBar = HealthBar.transform.GetChild(0).GetComponent<RectTransform>();

            if (_PM.team == "red")
            {
                color = new Color(0.453f, 0.102f, 0f, .4f);
            }
            else
            {
                color = new Color(0.012f, 0.191f, 0.289f, .4f);
            }

            TeamScorePanel.GetComponent<Image>().color = color;
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
