using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ColourTransition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{

    #region public variables

    public Color DefaultColour = new Color(182.0f/255.0f, 182.0f / 255.0f, 182.0f / 255.0f, 1.0f);
    public Color MouseOverColour = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    #endregion

    #region private variables

    Text _text;

    #endregion

    void Start()
    {
        _text = gameObject.GetComponent<Text>();
        _text.color = DefaultColour;
    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
       _text.color = MouseOverColour;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       _text.color = DefaultColour;
    }
    

}
