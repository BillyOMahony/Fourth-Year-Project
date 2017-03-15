using UnityEngine;
using System.Collections;

public class CameraWorks : Photon.PunBehaviour {

    #region private variables

    public Transform _cameraTransform;
    public Canvas canvas;

    #endregion

    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        _cameraTransform = transform.GetChild(1);
        if (!photonView.isMine && PhotonNetwork.connected)
        {
            _cameraTransform.gameObject.SetActive(false);
        }
        if (photonView.isMine)
        {
            canvas.worldCamera = _cameraTransform.gameObject.GetComponent<Camera>();
            canvas.planeDistance = 1;
        }
    }
   
}
