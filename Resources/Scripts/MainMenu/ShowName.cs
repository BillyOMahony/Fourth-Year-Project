using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowName : MonoBehaviour {

    #region private variables

    string _name;

    #endregion


    #region MonoBehaviour methods

    // Use this for initialization
    void Start () {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            _name = PlayerPrefs.GetString("PlayerName");
        }
        gameObject.GetComponent<Text>().text = "Welcome " + _name;
	}

    #endregion

}
