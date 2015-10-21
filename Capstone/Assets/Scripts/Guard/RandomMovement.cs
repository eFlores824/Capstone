using UnityEngine;
using System.Collections;

public class RandomMovement : MonoBehaviour {

    public float speed;
    public float correctRotation = 55.0f;

    public GameObject objective;
    private Transform theTransform;

	// Use this for initialization
	void Start () {
        theTransform = GetComponent<Transform>();
        objective = objective.GetComponent<Node>().randomNode();
        checkDirection();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 newPosition = Vector3.MoveTowards(theTransform.position, objective.transform.position, speed * Time.deltaTime);
        theTransform.position = newPosition;
        if (theTransform.position == objective.transform.position)
        {
            objective = objective.GetComponent<Node>().randomNode();
            checkDirection();
        }
	}

    private void checkDirection()
    {
        Vector3 objectivePosition = objective.transform.position;
        if (theTransform.position.x < objectivePosition.x)
        {
            theTransform.localEulerAngles = new Vector3(0, correctRotation, 0);
        }
        else if (theTransform.position.x > objectivePosition.x)
        {
            theTransform.localEulerAngles = new Vector3(0, 180 + correctRotation, 0);
        }
        else if (theTransform.position.z < objectivePosition.z)
        {
            theTransform.localEulerAngles = new Vector3(0, 270 + correctRotation, 0);
        }
        else if (theTransform.position.z > objectivePosition.z)
        {
            theTransform.localEulerAngles = new Vector3(0, 90 + correctRotation, 0);
        }
    }

}
