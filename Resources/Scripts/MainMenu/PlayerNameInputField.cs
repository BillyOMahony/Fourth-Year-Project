using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(InputField))]
public class PlayerNameInputField : MonoBehaviour {

    #region private variables
    
    // Store the PlayerPref key to avoid typos
    static string playerNamePrefKey = "PlayerName";

    #endregion

    #region MonoBehaviour methods

    // Use this for initialization
    void Start () {
        string defaultName = "";
        InputField _inputField = this.GetComponent<InputField>();
        if (_inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                _inputField.text = defaultName;
            }
        }

        PhotonNetwork.playerName = defaultName;
	}

    #endregion

    #region Public Methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public void SetPlayerName(string value)
    {
        Debug.Log("PlayerNameInputField: Name Changed");
        // #Important
        PhotonNetwork.playerName = value + " "; //force a trailing space in case value is an empty string, else playerName would not be updated

        PlayerPrefs.SetString(playerNamePrefKey, value);
    }

    #endregion
}
