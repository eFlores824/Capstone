using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public float sensitivity = 5.0f;
    public GameObject player;

    private float yRotation = 0.0f;
    private Transform playerTransform;
    private Transform theTransform;
    private bool using360Controller = false;

    void Start()
    {
        if (OVRManager.isHmdPresent)
        {
            Destroy(gameObject);
            return;
        }
        playerTransform = player.GetComponent<Transform>();
        theTransform = GetComponent<Transform>();
        using360Controller = !string.IsNullOrEmpty(Input.GetJoystickNames()[0]);
    }
    
    // Update is called once per frame
    void Update()
    {
        float xRotation = 0.0f;
        transform.position = player.transform.position;
        if (using360Controller)
        {
            xRotation = theTransform.localEulerAngles.y + (Input.GetAxis("RightJoystickX") * sensitivity / 2);
            yRotation += -Input.GetAxis("RightJoystickY") * sensitivity / 2;
            yRotation = Mathf.Clamp(yRotation, -360.0f, 360.0f);
            theTransform.localEulerAngles = new Vector3(-yRotation, xRotation, 0);
            Vector3 formerEulerAngles = playerTransform.localEulerAngles;
            playerTransform.localEulerAngles = new Vector3(formerEulerAngles.x, theTransform.localEulerAngles.y, formerEulerAngles.z);
        }
        else if (Input.GetMouseButton(0))
        {
            xRotation = theTransform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
            yRotation += Input.GetAxis("Mouse Y") * sensitivity;
            yRotation = Mathf.Clamp(yRotation, -360.0f, 360.0f);
            theTransform.localEulerAngles = new Vector3(-yRotation, xRotation, 0);
            Vector3 formerEulerAngles = playerTransform.localEulerAngles;
            playerTransform.localEulerAngles = new Vector3(formerEulerAngles.x, theTransform.localEulerAngles.y, formerEulerAngles.z);
        }
    }
}
