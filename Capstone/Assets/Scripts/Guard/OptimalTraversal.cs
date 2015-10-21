using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OptimalTraversal : MonoBehaviour {

    public Node destination;
    public Node start;
    public float speed;
    public float correctRotation;

    private Node[] path;
    private int currentNode = 0;
    private Vector3 currentObjective;
    private Transform theTransform;

	// Use this for initialization
	void Start () {
        path = optimalPath(start, destination);
        currentObjective = path[currentNode].transform.position;
        theTransform = GetComponent<Transform>();
        checkDirection();
	}
	
	// Update is called once per frame
	void Update () {
        if (currentNode < path.Length)
        {
            theTransform.position = Vector3.MoveTowards(theTransform.position, currentObjective, Time.deltaTime * speed);
            if (theTransform.position == currentObjective)
            {
                if (currentNode < path.Length - 1)
                {
                    currentObjective = path[++currentNode].transform.position;
                    checkDirection();
                }                
            }
        }
	}

    private Node[] optimalPath(Node start, Node goal)
    {
        IList<Node> open = new List<Node>();
        IList<Node> closed = new List<Node>();
        closed.Add(start);
        //A* algorithm stuff
        foreach (GameObject g in start.connections)
        {
            if (g != null)
            {
                Node n = g.GetComponent<Node>();
                open.Add(n);
                n.distance = (start.transform.position - g.transform.position).magnitude;
                n.origin = start;
            }
        }
        while (!closed.Contains(goal))
        {
            float least = float.MaxValue;
            Node optimal = null;
            foreach (Node n in open)
            {
                float cost = n.distance + (n.transform.position - goal.transform.position).magnitude;
                if (cost < least)
                {
                    least = cost;
                    optimal = n;
                }
            }
            open.Remove(optimal);
            closed.Add(optimal);
            if (optimal == goal)
            {
                break;
            }
            foreach (GameObject g in optimal.connections)
            {
                if (g != null)
                {
                    Node n = g.GetComponent<Node>();
                    if (!closed.Contains(n))
                    {
                        float distanceBetween = (optimal.transform.position - n.transform.position).magnitude;
                        float possibleDistance = optimal.distance + distanceBetween;
                        if (!open.Contains(n))
                        {
                            n.origin = optimal;
                            n.distance = possibleDistance;
                            open.Add(n);
                        }
                        else
                        {
                            if (possibleDistance < n.distance)
                            {
                                n.distance = possibleDistance;
                                n.origin = optimal;
                            }
                        }
                    }
                }
            }
        }

        Stack<Node> thePath = new Stack<Node>();
        Node current = goal;
        while (current != null)
        {
            thePath.Push(current);
            current = current.origin;
        }
        resetOrigins();
        return thePath.ToArray();
    }

    private void checkDirection()
    {
        if (theTransform.position.x < currentObjective.x)
        {
            theTransform.localEulerAngles = new Vector3(0, correctRotation, 0);
        }
        else if (theTransform.position.x > currentObjective.x)
        {
            theTransform.localEulerAngles = new Vector3(0, 180 + correctRotation, 0);
        }
        else if (theTransform.position.z < currentObjective.z)
        {
            theTransform.localEulerAngles = new Vector3(0, 270 + correctRotation, 0);
        }
        else if (theTransform.position.z > currentObjective.z)
        {
            theTransform.localEulerAngles = new Vector3(0, 90 + correctRotation, 0);
        }
    }

    private void resetOrigins()
    {
        Node[] allNodes = FindObjectsOfType<Node>();
        foreach (Node n in allNodes)
        {
            n.origin = null;
        }
    }
}
