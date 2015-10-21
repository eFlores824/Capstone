using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public float sensitivity = 5.0f;
    public GameObject player;

    private float yRotation = 0.0f;
    private Transform playerTransform;
    private Transform theTransform;

    void Start()
    {
        playerTransform = player.GetComponent<Transform>();
        theTransform = GetComponent<Transform>();
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
        if (Input.GetMouseButton(0))
        {
            float xRotation = theTransform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
            yRotation += Input.GetAxis("Mouse Y") * sensitivity;
            yRotation = Mathf.Clamp(yRotation, -360.0f, 360.0f);
            theTransform.localEulerAngles = new Vector3(-yRotation, xRotation, 0);
            Vector3 formerEulerAngles = playerTransform.localEulerAngles;
            playerTransform.localEulerAngles = new Vector3(formerEulerAngles.x, theTransform.localEulerAngles.y, formerEulerAngles.z);
        }
    }
}
