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

    public void realStart()
    {
        detector = GetComponent<GuardDetection>();
        theTransform = GetComponent<Transform>();
    }

	// Use this for initialization
	void Start () {

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

    public void changeGoal(Node goal)
    {
        if (path == null)
        {
            changePath(mostGoalsPath(start, goal, 4));
            return;
        }
        Node beginning;
        float closestDistance;
        if (currentNode < path.Length)
        {
            beginning = path[currentNode];
            closestDistance = (beginning.transform.position - goal.transform.position).magnitude;
        }
        else
        {
            beginning = objective.GetComponent<Node>();
            closestDistance = (objective.transform.position - goal.transform.position).magnitude;
        }
        foreach (GameObject obj in beginning.connections)
        {
            float distanceFrom = (obj.transform.position - goal.transform.position).magnitude;
            if (distanceFrom < closestDistance)
            {
                beginning = obj.GetComponent<Node>();
            }
        }
        Node[] newPath = mostGoalsPath(beginning, goal, 3);
        changePath(newPath);
    }

    private void changePath(Node[] newPath)
    {
        path = newPath;
        currentNode = 0;
        currentObjective = path[0].transform.position;
        checkDirection(currentObjective);
    }

    private Node[] AStar(Node start, Node goal, out float totalDistance)
    {
        IList<Node> open = new List<Node>();
        IList<Node> closed = new List<Node>();
        closed.Add(start);
        int counter = 0;
        foreach (GameObject g in start.connections)
        {
            Node n = g.GetComponent<Node>();
            open.Add(n);
            n.distance = start.connectionLengths[counter];
            n.origin = start;
            ++counter;
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
            counter = 0;
            foreach (GameObject g in optimal.connections)
            {
                Node n = g.GetComponent<Node>();
                if (!closed.Contains(n))
                {
                    float distanceBetween = optimal.connectionLengths[counter];
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
        totalDistance = goal.distance;
        resetOrigins();
        return thePath.ToArray();
    }

    private Node[][] Yens(Node start, Node goal, int numPaths)
    {
        float lowestDistance = 0.0f;
        Node[] bestPath = AStar(start, goal, out lowestDistance);
        Node[][] bestPaths = new Node[numPaths][];
        bestPaths[0] = bestPath;
        List<Node[]> possiblePaths = new List<Node[]>();
        List<float> possibleLengths = new List<float>();
        Node[] currentPath = bestPath;
        int numPathsFound = 1;
        while (numPathsFound != numPaths)
        {
            int brokenSpurs = 0;
            for (int i = 0; i < currentPath.Length - 2; ++i)
            {
                Node spur = currentPath[i];
                List<int> indicesChanged = new List<int>();
                List<float> formerLengths = new List<float>();
                for (int j = 0; j < spur.connections.Length; ++j)
                {
                    Node connection = spur.connections[j].GetComponent<Node>();
                    bool found = connectionFound(bestPaths, connection, spur);
                    bool valid = (i > 0) ? !connection.Equals(currentPath[i - 1]) && found : found;
                    if (valid)
                    {
                        indicesChanged.Add(j);
                        formerLengths.Add(spur.connectionLengths[j]);
                        spur.connectionLengths[j] = float.MaxValue;
                    }
                }
                bool allMax = true;
                for (int j = 0; j < spur.connectionLengths.Length; ++j)
                {
                    float currentLength = spur.connectionLengths[j];
                    if (currentLength != float.MaxValue)
                    {
                        allMax = false;
                        break;
                    }
                }
                if (allMax) { 
                    ++brokenSpurs;
                    restoreConnections(indicesChanged.ToArray(), formerLengths.ToArray(), spur);
                    break; 
                }
                Node[] addition = AStar(spur, goal, out lowestDistance);
                Node[] possible = addPaths(i, currentPath, addition);
                possiblePaths.Add(possible);
                possibleLengths.Add(lowestDistance);
                restoreConnections(indicesChanged.ToArray(), formerLengths.ToArray(), spur);
            }
            if (brokenSpurs == currentPath.Length)
            {
                break;
            }
            float lowestCost = float.MaxValue;
            Node[] cheapest = null;
            for (int i = 0; i < possiblePaths.Count; ++i)
            {
                float cost = possibleLengths[i];
                if (cost < lowestCost)
                {
                    lowestCost = cost;
                    cheapest = possiblePaths[i];
                }
            }
            possiblePaths.Remove(cheapest);
            bestPaths[numPathsFound++] = cheapest;
        }
        return bestPaths;
    }

    private void restoreConnections(int[] indicesChanged, float[] formerLengths, Node restoring)
    {
        for (int j = 0; j < indicesChanged.Length; ++j)
        {
            int indice = indicesChanged[j];
            float formerLength = formerLengths[j];
            restoring.connectionLengths[indice] = formerLength;
        }
    }

    private Node[] mostGoalsPath(Node start, Node goal, int numPaths)
    {
        Node[][] options = Yens(start, goal, numPaths);
        float highestTotalWeight = float.MinValue;
        Node[] optimal = null;
        foreach (Node[] path in options)
        {
            if (path == null)
            {
                continue;
            }
            float currentTotalWeight = 0.0f;
            foreach (Node n in path)
            {
                currentTotalWeight += n.Weight;
            }
            if (currentTotalWeight > highestTotalWeight)
            {
                optimal = path;
                highestTotalWeight = currentTotalWeight;
            }
        }
        return optimal;
    }

    private bool connectionFound(Node[][] paths, Node firstNode, Node secondNode)
    {
        for (int i = 0; i < paths.Length; ++i)
        {
            Node[] currentPath = paths[i];
            if (currentPath == null) { continue; }
            for (int j = 0; j < currentPath.Length - 1; ++j)
            {
                Node first = currentPath[j];
                Node second = currentPath[j + 1];
                if ((first.Equals(firstNode) && second.Equals(secondNode)) || (first.Equals(secondNode) && second.Equals(firstNode)))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private Node[] addPaths(int firstEndpoint, Node[] firstPath, Node[] secondPath)
    {
        if (firstPath.Length == 0)
        {
            return secondPath;
        }
        if (secondPath.Length == 0)
        {
            return firstPath;
        }
        Node[] result = new Node[firstEndpoint + secondPath.Length];
        int i = 0;
        while (i < firstEndpoint) {
            result[i] = firstPath[i];
            ++i;
        }
        for (int j = 0; j < secondPath.Length; ++j)
        {
            result[i++] = secondPath[j];
        }
        return result;
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
            n.distance = 0.0f;
            n.origin = null;
        }
    }
}
