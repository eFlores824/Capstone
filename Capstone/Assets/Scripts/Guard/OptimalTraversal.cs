using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(GuardDetection))]
public class OptimalTraversal : MonoBehaviour {

    public Node destination;
    public Node start;
    public float speed;
    public float correctRotation;
    public float maintainedDistance;

    private Node[] path;
    private int currentNode = 0;
    private Vector3 currentObjective;
    private Transform theTransform;
    private GuardDetection detector;
    private GameObject objective;

	// Use this for initialization
	void Start () {
        detector = GetComponent<GuardDetection>();
        path = optimalPath(start, destination);
        currentObjective = path[currentNode].transform.position;
        theTransform = GetComponent<Transform>();
        checkDirection(currentObjective);
	}
	
	// Update is called once per frame
	void Update () {
        if (!detector.gameOver)
        {
            if (currentNode < path.Length)
            {
                theTransform.position = Vector3.MoveTowards(theTransform.position, currentObjective, Time.deltaTime * speed);
                if (theTransform.position == currentObjective)
                {
                    if (currentNode < path.Length - 1)
                    {
                        currentObjective = path[++currentNode].transform.position;
                        checkDirection(currentObjective);
                    }
                    else
                    {
                        objective = path[currentNode].nodeWithinDistance(currentObjective, maintainedDistance);
                        checkDirection(objective.transform.position);
                        ++currentNode;
                    }
                }
            }
            else
            {
                theTransform.position = Vector3.MoveTowards(theTransform.position, objective.transform.position, Time.deltaTime * speed);
                if (theTransform.position == objective.transform.position)
                {
                    objective = objective.GetComponent<Node>().nodeWithinDistance(currentObjective, maintainedDistance);
                    checkDirection(objective.transform.position);
                }
            }
        }
	}

    private void changePath(Node[] newPath)
    {
        path = newPath;
        currentNode = 0;
        currentObjective = path[0].transform.position;
        checkDirection(currentObjective);
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
            int counter = 0;
            foreach (GameObject g in optimal.connections)
            {
                if (g != null)
                {
                    Node n = g.GetComponent<Node>();
                    if (!closed.Contains(n))
                    {
                        //float distanceBetween = (optimal.transform.position - n.transform.position).magnitude;
                        float distanceBetween = n.connectionLengths[counter];
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
                ++counter;
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

    private Node[] mostGoalsPath(Node start, Node goal)
    {
        Node[] bestPath = optimalPath(start, goal);
        int numPaths = 3;
        if (bestPath.Length - 1 < numPaths)
        {
            numPaths = bestPath.Length - 1;
        }
        Node[][] paths = new Node[numPaths][];
        paths[0] = bestPath;
        for (int i = 1; i < numPaths; ++i)
        {
            Node changing = bestPath[i - 1];
            int pathChanging = checkConnection(changing, bestPath[i]);
            bestPath[i - 1].connectionLengths[pathChanging] = float.MaxValue;
            Node[] nextOptimal = optimalPath(start, goal);
            paths[i] = nextOptimal;
            changing.connectionLengths[pathChanging] = (changing.transform.position - bestPath[i].transform.position).magnitude;
        }
        Node[] optimal = null;
        float highestWeight = float.MinValue;
        foreach (Node[] thePath in paths)
        {
            float totalWeight = 0.0f;
            foreach (Node n in thePath)
            {
                totalWeight += n.Weight;
            }
            if (totalWeight < highestWeight)
            {
                optimal = thePath;
            }
        }
        return optimal;
    }

    private int checkConnection(Node a, Node b)
    {
        int nodeConnection = 0;
        for (int i = 0; i < a.NumConnections; ++i) {
            Node connection = a.connections[i].GetComponent<Node>();
            if (connection.Equals(b)) { 
                nodeConnection = i;
                break;
            }
        }
        return nodeConnection;
    }

    private void checkDirection(Vector3 destination)
    {
        if (theTransform.position.x < destination.x)
        {
            theTransform.localEulerAngles = new Vector3(0, correctRotation, 0);
        }
        else if (theTransform.position.x > destination.x)
        {
            theTransform.localEulerAngles = new Vector3(0, 180 + correctRotation, 0);
        }
        else if (theTransform.position.z < destination.z)
        {
            theTransform.localEulerAngles = new Vector3(0, 270 + correctRotation, 0);
        }
        else if (theTransform.position.z > destination.z)
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
