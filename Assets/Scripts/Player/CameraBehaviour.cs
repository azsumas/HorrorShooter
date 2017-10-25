using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    private float rotationX;
    private float rotationY;
    public float smooth = 10;

    public float maxAngle;

    private Transform cameraTransform;
    private Quaternion cameraRotation;

    private Transform playerTransform;
    private Quaternion playerRotation;

    // Use this for initialization
    void Start()
    {
        cameraTransform = this.transform;
        cameraRotation = cameraTransform.rotation;

        playerTransform = this.transform.parent;
        playerRotation = playerTransform.localRotation;

    }

    // Update is called once per frame
    void Update()
    {
        cameraRotation *= Quaternion.Euler(-rotationX, 0, 0);

        if (cameraRotation.eulerAngles.x > 180)
        {
            if (cameraRotation.eulerAngles.x <= 360 - maxAngle)
            {
                cameraRotation = Quaternion.Euler(360 - maxAngle, cameraRotation.eulerAngles.y, 0);
            }
        }
        if (cameraRotation.eulerAngles.x < 180)
        {
            if (cameraRotation.eulerAngles.x >= maxAngle)
            {
                cameraRotation = Quaternion.Euler(maxAngle, cameraRotation.eulerAngles.y, 0);
            }
        }

        cameraTransform.localRotation = Quaternion.Lerp(cameraTransform.localRotation, cameraRotation, Time.deltaTime * smooth);

        playerRotation *= Quaternion.Euler(0, rotationY, 0);

        playerTransform.localRotation = Quaternion.Lerp(playerTransform.localRotation, playerRotation, Time.deltaTime * smooth);

    }

    //up
    public void SetRotationX(float y)
    {
        rotationX = y;
    }

    //left
    public void SetRotationY(float x)
    {
        rotationY = x;
    }

}
