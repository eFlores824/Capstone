using UnityEngine;
using System.Collections;
using System.Text;

public class InfoManager : MonoBehaviour {

    private Node[] nodes;

    private float timePassedSinceGoal = 0.0f;
    private int[] goalReachedOrder = new int[4];
    private float[] timeBetweenGoals = new float[4];

    private int[][] theOrders;
    private float[][] theTimes;

    private int currentGoalIndex = 0;
    private int currentTimeIndex = 0;
    private bool orderFound = false;

	// Use this for initialization
	void Start () {
        PlayerPrefs.DeleteAll();
        nodes = FindObjectsOfType<Node>();
        //load();
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
        //writeNodes();
        //writeGoals();
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
        for (int i = currentGoalIndex; i < goalReachedOrder.Length; ++i)
        {
            goalReachedOrder[i] = 0;
            timeBetweenGoals[i] = float.MaxValue;
        }
        foreach (int[] order in theOrders) {
            if (equalArrays(goalReachedOrder, order))
            {
                ++order[4];
                orderFound = true;
                break;
            }
        }
        if (!orderFound)
        {

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
        
        //builder = new StringBuilder();
        //string alreadyFound = PlayerPrefs.GetString("GoalsTime");
        //if (!string.IsNullOrEmpty(alreadyFound))
        //{
        //    builder.Append(alreadyFound);
        //    builder.Append('\n');
        //}
        //for (int i = 0; i < timeBetweenGoals.Length; ++i)
        //{
        //    builder.Append(timeBetweenGoals[i]);
        //    if (i != timeBetweenGoals.Length - 1)
        //    {
        //        builder.Append("-");
        //    }
        //}
        //PlayerPrefs.SetString("GoalsTime", builder.ToString());
        PlayerPrefs.Save();
    }

    private void readGoals()
    {
        string goalsReached = PlayerPrefs.GetString("GoalsOrder");
        string[] eachGame = goalsReached.Split(new char[] {'\n'});
        theOrders = new int[eachGame.Length][];
        int currentOrder = 0;
        foreach (string s in eachGame)
        {
            if (string.IsNullOrEmpty(s))
            {
                continue;
            }
            int[] orderReached = new int[4];
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
            if (string.IsNullOrEmpty(s))
            {
                continue;
            }
            float[] timeBetweenGoals = new float[4];
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

    private Node[] optimalNodes()
    {
        Node first = new Node();
        Node second = new Node();
        Node third = new Node();
        Node fourth = new Node();
        foreach (Node n in nodes)
        {
            float nWeight = n.Weight;
            if (nWeight > first.Weight)
            {
                first = n;
            }
            else if (nWeight > second.Weight)
            {
                second = n;
            }
            else if (nWeight > third.Weight)
            {
                third = n;
            }
            else if (nWeight > fourth.Weight)
            {
                fourth = n;
            }
        }
        Node[] results = new Node[] { first, second, third, fourth};
        return results;
    }

}
