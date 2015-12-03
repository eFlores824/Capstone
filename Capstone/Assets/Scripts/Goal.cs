using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

    public int id;
    public float rotationSpeed;

    private Transform theTransform;

    void Start()
    {
        theTransform = GetComponent<Transform>();
    }

    void Update()
    {
        theTransform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
