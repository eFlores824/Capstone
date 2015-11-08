using UnityEngine;
using System.Collections;

public class GuardManager : MonoBehaviour {

    private GuardDetection[] guards;

	// Use this for initialization
	void Start () {
        guards = FindObjectsOfType<GuardDetection>();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void distributeOnSound(Node soundHeard)
    {
        float weightOfNode = soundHeard.Weight;
        if (weightOfNode > 5)
        {

        }
        else if (weightOfNode > 3)
        {

        }
        else if (weightOfNode > 1)
        {

        }
        else
        {

        }
    }
}
