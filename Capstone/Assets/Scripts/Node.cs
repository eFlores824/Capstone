using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {

    public GameObject[] connections = new GameObject[4];

    private int numConnections;

	// Use this for initialization
	void Start () {
        int counter = 0;
	    foreach (GameObject obj in connections) {
            if (obj != null)
            {
                ++counter;
            }
        }
        numConnections = counter;
	}

    public GameObject randomNode()
    {
        return connections[Random.Range(0, numConnections)];
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
