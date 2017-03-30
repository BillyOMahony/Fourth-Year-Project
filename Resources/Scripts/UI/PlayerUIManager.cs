using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUIManager : Photon.PunBehaviour {

    public bool mine;

    //public PlayerController _PC;
    public PlayerControllerRB _PC;
    public PlayerManager _PM;
    public PhotonView _PV;

    public Color color;

    GameObject outOfBounds;

    GameObject TeamScorePanel;
    Image HealthBar;
    Image EngineBar;
    Text Speedometer;

    // Use this for initialization
    void Start () {

        _PV = GetComponent<PhotonView>();
        _PM = GetComponent<PlayerManager>();
        _PC = GetComponent<PlayerControllerRB>();

        mine = _PC.GetComponent<PhotonView>().isMine;

        TeamScorePanel = GameObject.Find("TeamScores Panel");
        HealthBar = GameObject.Find("HealthBar").GetComponent<Image>();
        EngineBar = GameObject.Find("EngineBar").GetComponent<Image>();
        Speedometer = GameObject.Find("Speedometer Text").GetComponent<Text>();
        outOfBounds = GameObject.Find("OutOfBounds Text");

        if (mine)
        {
            outOfBounds.SetActive(false);

            if (_PM.team == "red")
            {
                color = new Color(0.453f, 0.102f, 0f, .4f);
            }
            else
            {
                color = new Color(0.012f, 0.191f, 0.289f, .4f);
            }

            TeamScorePanel.GetComponent<Image>().color = color;
            HealthBar.color = color;
            EngineBar.color = color;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (_PC.GetComponent<PhotonView>().isMine) {
            UpdateHealthBar();
            UpdateEngineBar();
            UpdateSpeedometer();

            outOfBounds.GetComponent<Text>().text = "Return to area, you have " + _PM.outOfBoundsTimer.ToString("0.0") + " seconds";
        }     
    }

    public void UpdateSpeedometer()
    {

        Speedometer.text = _PC.zVel.ToString("0.0") + "m/s";
    }

    public void UpdateEngineBar()
    {
        EngineBar.fillAmount = _PC.engine;
    }

    public void UpdateHealthBar()
    {
        HealthBar.fillAmount = _PM.Health / 100;
    }

    public void OutOfBounds()
    {
        if (mine)
        {
            outOfBounds.SetActive(true);
        }
    }

    public void BackInBounds()
    {
        if (mine)
        {
            outOfBounds.SetActive(false);
        }
    }

}
