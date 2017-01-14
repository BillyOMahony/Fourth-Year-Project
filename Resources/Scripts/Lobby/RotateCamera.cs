using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {

    #region public variables

    public float speed = 0.2f;

    #endregion

    #region private variables

    float timeRotating = 0;

    #endregion

    void Update () {

        transform.Rotate(Vector3.right * Time.deltaTime * speed);
        timeRotating += Time.deltaTime;  

        if(timeRotating >= 600)
        {
            speed = speed * -1;
            timeRotating = 0;
        }

	}
}
