using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconScript : MonoBehaviour {

    int iconInt = 0;
    public Sprite[] icons;
    public Image iconImage;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.HasKey("PlayerIcon"))
        {
            iconInt = PlayerPrefs.GetInt("PlayerIcon");
        }
        else
        {
            PlayerPrefs.SetInt("PlayerIcon", iconInt);
        }
        UpdatePlayerIcon();
	}
	
	// This displays the player's icon
    void UpdatePlayerIcon()
    {
        iconImage.sprite = icons[PlayerPrefs.GetInt("PlayerIcon")];
    }

    public void ChangeIcon(int num)
    {
        if (PlayerPrefs.HasKey("PlayerIcon"))
        {
            PlayerPrefs.SetInt("PlayerIcon", num);
            UpdatePlayerIcon();
        }
        else
        {
            Debug.LogError("ChangeIcon(): PlayerPrefs does not contain key");
        }
    }

    public Sprite GetIcon(int num)
    {
        return icons[num];
    }
}
