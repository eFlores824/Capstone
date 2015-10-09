using UnityEngine;
using System.Collections;

public class FirstPersonController : MonoBehaviour {

    public GameObject forward;
    public GameObject left;
    public float speed;

    private Transform theTransform;
    private Transform forwardTransform;
    private Transform leftTransform;

	// Use this for initialization
	void Start () {
        theTransform = GetComponent<Transform>();
        forwardTransform = forward.GetComponent<Transform>();
        leftTransform = left.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {    
        if (Input.anyKey)
        {
            Vector3 forwardVector;
            Vector3 leftVector;
            forwardVector = (forwardTransform.position - theTransform.position).normalized * speed;
            leftVector = (leftTransform.position - theTransform.position).normalized * speed;
            if (Input.GetKey(KeyCode.W))
            {
                theTransform.position += forwardVector;
            }
            if (Input.GetKey(KeyCode.A))
            {
                theTransform.position += leftVector;
            }
            if (Input.GetKey(KeyCode.S))
            {
                theTransform.position += -forwardVector;
            }
            if (Input.GetKey(KeyCode.D))
            {
                theTransform.position += -leftVector;
            }
        }
	}
}
