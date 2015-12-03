using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class InfoManager : MonoBehaviour {

    public static bool loadInfo;
    public Node[] goals;

    private Node[] nodes;

    private float timePassedSinceGoal = 0.0f;
    private int[] goalReachedOrder = new int[4];
    private float[] timeBetweenGoals = new float[8];

    private int[][] theOrders;
    private float[][] theTimes;

    private int currentGoalIndex = 0;
    private int currentTimeIndex = 0;
    private bool orderFound = false;

	// Use this for initialization
	public void realStart () {
        nodes = FindObjectsOfType<Node>();
        if (loadInfo)
        {
            load();
        }
	}
	
	// Update is called once per frame
	void Update () {
        timePassedSinceGoal += Time.deltaTime;
	}

    public Node nearestNode(Vector3 position)
    {
        Node closest = null;
        float leastDistance = float.MaxValue;
        foreach (Node n in nodes)
        {
            Vector3 toNode = n.gameObject.transform.position - position;
            float distance = toNode.magnitude;
            if (distance < leastDistance && Physics.Raycast(position,toNode, distance))
            {
                closest = n;
                leastDistance = distance;       
            }
        }
        return closest;
    }

    private Node findNode(int id)
    {
        Node result = null;
        if (id <= nodes.Length)
        {
            foreach (Node n in nodes)
            {
                if (n.id == id)
                {
                    result = n;
                    break;
                }
            }
        }
        return result;
    }

    public void save()
    {
        checkGoals();
        writeNodes();
        writeGoals();
        loadInfo = true;
    }

    private void load()
    {
        readGoals();
        readNodes();
    }

    public void goalReached(int reached)
    {
        goalReachedOrder[currentGoalIndex++] = reached;
        timeBetweenGoals[currentTimeIndex++] = timePassedSinceGoal;
        timePassedSinceGoal = 0.0f;
    }

    private void checkGoals()
    {
        int timesToAdjust = 0;
        for (int i = currentGoalIndex; i < goalReachedOrder.Length - 1; ++i)
        {
            goalReachedOrder[i] = 0;
            timeBetweenGoals[i] = float.MaxValue;
        }
        if (theOrders == null)
        {
            return;
        }
        foreach (int[] order in theOrders) {
            if (equalArrays(goalReachedOrder, order))
            {
                ++order[4];
                orderFound = true;
                break;
            }
            ++timesToAdjust;
        }
        if (orderFound)
        {
            float[] adjusting = theTimes[timesToAdjust];
            for (int i = 0; i < adjusting.Length; ++i)
            {
                if (timeBetweenGoals[i] == float.MaxValue)
                {
                    adjusting[i] = float.MaxValue;
                }
                else
                {
                    adjusting[i] = (adjusting[i + 4] + timeBetweenGoals[i]) / (float)theOrders[timesToAdjust][4];
                }
            }
        }
    }

    private bool equalArrays(int[] firstArray, int[] secondArray)
    {
        for (int i = 0; i < firstArray.Length; ++i)
        {
            if (firstArray[i] != secondArray[i])
                return false;
        }
        return true;
    }

    private void writeGoals()
    {
        if (theOrders == null)
        {
            return;
        }
        StringBuilder builder = new StringBuilder();
        for (int j = 0; j < theOrders.Length; ++j) {
            int[] currentOrder = theOrders[j];
            for (int i = 0; i < currentOrder.Length; ++i)
            {
                builder.Append(currentOrder[i]);
                if (i != currentOrder.Length - 1)
                {
                    builder.Append("-");
                }
            }
            if (j != theOrders.Length - 1)
            {
                builder.Append('\n');
            }
        }
        if (!orderFound)
        {
            if (theOrders != null)
                builder.Append('\n');
            for (int i = 0; i < goalReachedOrder.Length; ++i)
            {
                builder.Append(goalReachedOrder[i]);
                if (i != goalReachedOrder.Length - 1)
                {
                    builder.Append("-");
                }
            }
        }
        PlayerPrefs.SetString("GoalsOrder", builder.ToString());
        builder = new StringBuilder();
        for (int j = 0; j < theTimes.Length; ++j)
        {
            float[] currentOrder = theTimes[j];
            for (int i = 0; i < currentOrder.Length; ++i)
            {
                builder.Append(currentOrder[i]);
                if (i != currentOrder.Length - 1)
                {
                    builder.Append("-");
                }
            }
            if (j != currentOrder.Length - 1)
            {
                builder.Append('\n');
            }
        }
        if (!orderFound)
        {
            if (theTimes != null)
                builder.Append('\n');
            for (int i = 0; i < timeBetweenGoals.Length; ++i)
            {
                builder.Append(timeBetweenGoals[i]);
                if (i != timeBetweenGoals.Length - 1)
                {
                    builder.Append("-");
                }
            }
            for (int i = 0; i < timeBetweenGoals.Length; ++i)
            {
                builder.Append(timeBetweenGoals[i]);
                if (i != timeBetweenGoals.Length - 1)
                {
                    builder.Append("-");
                }
            }
        }
        PlayerPrefs.SetString("GoalsTime", builder.ToString());
        PlayerPrefs.Save();
    }

    private void readGoals()
    {
        string goalsReached = PlayerPrefs.GetString("GoalsOrder");
        if (string.IsNullOrEmpty(goalsReached))
            return;
        string[] eachGame = goalsReached.Split(new char[] {'\n'});
        theOrders = new int[eachGame.Length][];
        int currentOrder = 0;
        foreach (string s in eachGame)
        {
            int[] orderReached = new int[5];
            string[] eachGoal = s.Split(new char[] {'-'});
            for (int i = 0; i < eachGoal.Length; ++i)
            {
                orderReached[i] = int.Parse(eachGoal[i]);
            }
            theOrders[currentOrder++] = orderReached;
        }

        string timesBetween = PlayerPrefs.GetString("GoalsTime");
        eachGame = timesBetween.Split(new char[] {'\n'});
        theTimes = new float[eachGame.Length][];
        int currentTime = 0;
        foreach (string s in eachGame)
        {
            float[] timeBetweenGoals = new float[8];
            string[] eachTime = s.Split(new char[] {'-'});
            for (int i = 0; i < eachTime.Length; ++i)
            {
                timeBetweenGoals[i] = float.Parse(eachTime[i]);
            }
            theTimes[currentTime++] = timeBetweenGoals;
        }

    }

    private void writeNodes()
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < nodes.Length; ++i)
        {
            builder.Append(nodes[i].ToString());
            if (i != nodes.Length - 1)
            {
                builder.Append("\n");
            }
        }
        PlayerPrefs.SetString("Nodes", builder.ToString());
        PlayerPrefs.Save();
    }

    private void readNodes()
    {
        int currentGame = PlayerPrefs.GetInt("GameCount") + 1;
        PlayerPrefs.SetInt("GameCount", currentGame + 1);
        PlayerPrefs.Save();
        string nodesString = PlayerPrefs.GetString("Nodes");
        if (string.IsNullOrEmpty(nodesString))
        {
            return;
        }
        string[] eachNode = nodesString.Split(new char[] { '\n' });
        foreach (string s in eachNode)
        {
            string[] values = s.Split(new char[]{'-'});
            int id = int.Parse(values[0]);
            int soundCount = int.Parse(values[1]);
            int foundCount = int.Parse(values[2]);
            int lastTriggered = int.Parse(values[3]);
            Node theNode = findNode(id);
            theNode.setInfo(soundCount, foundCount, lastTriggered, currentGame);
        }
    }

    public int[] mostCommonPattern()
    {
        int highestCount = int.MinValue;
        int[] highestOrder = null;
        for (int i = 0; i < theOrders.Length; ++i)
        {
            int[] currentOrder = theOrders[i];
            if (currentOrder[4] > highestCount)
            {
                highestCount = currentOrder[4];
                highestOrder = currentOrder;
            }
        }
        return highestOrder;
    }

    private Node chooseRandomNextGoal()
    {
        int currentGoal = goalReachedOrder[currentGoalIndex - 1];
        int[] possibleNextGoals = new int[2];
        if (currentGoal == 1)
        {
            possibleNextGoals[0] = goalReachedOrder.Length;
            possibleNextGoals[1] = 2;
        }
        else if (currentGoal == goalReachedOrder.Length)
        {
            possibleNextGoals[0] = 1;
            possibleNextGoals[1] = goalReachedOrder.Length - 1;
        }
        else
        {
            possibleNextGoals[0] = currentGoal - 1;
            possibleNextGoals[1] = currentGoal + 1;
        }
        int random = Random.Range(0, 2);
        int randomGoal = possibleNextGoals[random];
        return goals[randomGoal - 1];
    }

    public Node nextLikelyGoal()
    {
        if (theOrders == null)
        {
            return chooseRandomNextGoal();
        }
        int highestCount = int.MinValue;
        int[] highestOrder = null;
        for (int i = 0; i < theOrders.Length; ++i)
        {
            int[] currentOrder = theOrders[i];
            bool match = true;
            for (int j = 0; j < currentGoalIndex; ++j)
            {
                if (currentOrder[j] != goalReachedOrder[j])
                {
                    match = false;
                    break;
                }
            }
            if (!match)
                break;
            if (currentOrder[4] > highestCount)
            {
                highestCount = currentOrder[4];
                highestOrder = currentOrder;
            }
        }
        int nextGoal = highestOrder[currentGoalIndex];
        return goals[nextGoal - 1];
    }

    public Node nearestWeightedNode(Node root, int levelsDeep)
    {
        List<Node> nodesChecking = new List<Node>();
        Queue<Node> theQueue = new Queue<Node>();
        Queue<int> levels = new Queue<int>();
        theQueue.Enqueue(root);
        levels.Enqueue(0);
        while (theQueue.Count > 0)
        {
            Node current = theQueue.Dequeue();
            int currentLevel = levels.Dequeue();
            if (currentLevel <= levelsDeep)
            {
                foreach (GameObject n in current.connections)
                {
                    Node theNode = n.GetComponent<Node>();
                    nodesChecking.Add(theNode);
                    theQueue.Enqueue(theNode);
                    levels.Enqueue(currentLevel + 1);
                }
            }
        }
        float greatestValue = float.MinValue;
        Node greatestNode = null;
        foreach (Node n in nodesChecking)
        {
            float distanceToRoot = (root.transform.position - n.transform.position).magnitude * 0.5f;
            float weight = n.Weight / distanceToRoot;
            if (weight > greatestValue)
            {
                greatestValue = weight;
                greatestNode = n;
            }
        }
        return greatestNode;
    }

    public Node[] optimalNodes()
    {
        Node first = null;
        Node second = null;
        Node third = null;
        Node fourth = null;
        float firstWeight = -1.0f;
        float secondWeight = -1.0f;
        float thirdWeight = -1.0f;
        float fourthWeight = -1.0f;
        foreach (Node n in nodes)
        {
            float nWeight = n.Weight;
            if (nWeight > firstWeight)
            {
                firstWeight = nWeight;
                second = first;
                first = n;
            }
            else if (nWeight > secondWeight)
            {
                secondWeight = nWeight;
                third = second;
                second = n;
            }
            else if (nWeight > thirdWeight)
            {
                thirdWeight = nWeight;
                fourth = third;
                third = n;
            }
            else if (nWeight > fourthWeight)
            {
                fourthWeight = nWeight;
                fourth = n;
            }
        }
        Node[] results = new Node[] { first, second, third, fourth};
        return results;
    }

}
