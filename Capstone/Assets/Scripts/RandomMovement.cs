using UnityEngine;
using System.Collections;

public class RandomMovement : MonoBehaviour {

    private GameObject objective;
    private Transform theTransform;

	// Use this for initialization
	void Start () {
        theTransform = GetComponent<Transform>();
        newObjective();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void newObjective()
    {
        if (theTransform.position == objective.transform.position)
        {
            objective = objective.GetComponent<Node>().randomNode();
            changeDirections();
        }
    }

    private void changeDirections()
    {

    }
}
