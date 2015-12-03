using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public float sensitivity = 2.0f;
    public GameObject player;

    private float yRotation = 0.0f;
    private Transform playerTransform;
    private Transform theTransform;
    private bool using360Controller = false;
    public bool paused = false;

    void Start()
    {
        playerTransform = player.GetComponent<Transform>();
        theTransform = GetComponent<Transform>();
        foreach (string s in Input.GetJoystickNames())
        {
            using360Controller = !string.IsNullOrEmpty(s);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (paused)
        {
            return;
        }
        float xRotation = 0.0f;
        if (using360Controller)
        {
            xRotation = theTransform.localEulerAngles.y + (Input.GetAxis("RightJoystickX") * sensitivity / 2);
            yRotation += -Input.GetAxis("RightJoystickY") * sensitivity / 2;
            yRotation = Mathf.Clamp(yRotation, -90.0f, 90.0f);
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

    void LateUpdate()
    {
        transform.position = player.transform.position;
    }
}
