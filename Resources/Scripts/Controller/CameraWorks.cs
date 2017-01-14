using UnityEngine;
using System.Collections;

public class CameraWorks : Photon.PunBehaviour {

    #region private variables

    public Transform _cameraTransform;

    #endregion

    void Start()
    {
        _cameraTransform = transform.GetChild(1);
        if (photonView.isMine == false && PhotonNetwork.connected == true)
        {
            _cameraTransform.gameObject.SetActive(false);
        }
    }
   
}
