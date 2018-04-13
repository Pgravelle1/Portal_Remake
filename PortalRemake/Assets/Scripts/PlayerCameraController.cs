using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    float mouseX, mouseY;
    public float horizontalSensitivity = 100f, verticalSensitivity = 100f;
    public GameObject playerModel;
    Vector3 worldAngles;

    public float downClampMax = 90f, upClampMax = 270f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = -Input.GetAxis("Mouse Y");

        // Left/Right movement
        playerModel.transform.Rotate(Vector3.up, mouseX * horizontalSensitivity * Time.deltaTime);


        transform.Rotate(Vector3.right, mouseY * verticalSensitivity * Time.deltaTime);

        worldAngles = transform.eulerAngles;

        #region Camera Clamping

        // Up Clamp
        if (worldAngles.x < upClampMax && worldAngles.x > 180)
        {
            worldAngles.x = upClampMax;
        }
        // Down Clamp
        else if (worldAngles.x > downClampMax && worldAngles.x <= 180)
        {
            worldAngles.x = downClampMax;
        }

        transform.localRotation = Quaternion.Euler(worldAngles.x, 0, 0);

        /*
        Looking forward is 0 degrees (360 degrees).
        Looking straight up is 270 degrees.
        Looking straight down is 90 degrees.
        Therefore we don't want to go bigger than 90 degrees,
            otherwise we are now upsidedown.
        So we need the angle to be <= 90...
        Therefore our if statements need to check and see if we're
            rotated >90 degrees. If we are then we went too far,
            so set our rotation back to 90.
        We also check if we're less than 180 degrees because if we don't
            then we're ALWAYS triggering the if statements...
            In other words, when we start at 0 degrees, we are smaller than 270
            so snap to 270 degrees...
            But now that we're in 270 degrees, we're bigger than 90 degrees
            so snap to 90 degrees...
            and it just KEEPS HAPPENING.
        Adding the 180 check makes it so we check in a range.
         */
        #endregion
    }

    public void ChangeMouseSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivity);
        verticalSensitivity = sensitivity;
        horizontalSensitivity = sensitivity;
    }
}
