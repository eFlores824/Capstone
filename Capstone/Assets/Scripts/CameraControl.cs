using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public float sensitivity = 5.0f;
    public GameObject left;
    public GameObject Forward;

    private float yRotation = 0.0f;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float xRotation = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
            yRotation += Input.GetAxis("Mouse Y") * sensitivity;
            yRotation = Mathf.Clamp(yRotation, -360.0f, 360.0f);
            transform.localEulerAngles = new Vector3(-yRotation, xRotation, 0);
        }
    }
}
