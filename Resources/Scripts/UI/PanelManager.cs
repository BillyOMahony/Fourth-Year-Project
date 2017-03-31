using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PanelManager : MonoBehaviour {

    public GameObject GameOverlay;
    public GameObject Scoreboard;
    public GameObject GameMenu;

    public bool uiActive = false;

    private bool _menuState = false;
    CursorStates _cs;

	// Use this for initialization
	void Start () {
	    if(GameOverlay == null || Scoreboard == null || GameMenu == null)
        {
            Debug.LogError("PanelManager: A panel is not assigned");
        }
        GameOverlay.SetActive(true);
        Scoreboard.SetActive(false);
        GameMenu.SetActive(false);

        _cs = GameObject.Find("CursorStates").GetComponent<CursorStates>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Cancel"))
        {
            if (_menuState == false)
            {
                GameOverlay.SetActive(false);
                Scoreboard.SetActive(false);
                GameMenu.SetActive(true);
                uiActive = true;
                _cs.UnlockCursor();
                _menuState = true;
            }
            else
            {
                Resume();
            }

        }

        if (Input.GetButtonDown("Scoreboard") && !GameMenu.GetActive())
        {
            GameOverlay.SetActive(false);
            Scoreboard.SetActive(true);
            GameMenu.SetActive(false);
        }

        if (Input.GetButtonUp("Scoreboard") && !GameMenu.GetActive())
        {
            GameOverlay.SetActive(true);
            Scoreboard.SetActive(false);
            GameMenu.SetActive(false);
        }

    }

    public void Resume()
    {
        GameOverlay.SetActive(true);
        Scoreboard.SetActive(false);
        GameMenu.SetActive(false);
        _cs.UnlockCursor();
        uiActive = false;
        _menuState = false;
    }
}
